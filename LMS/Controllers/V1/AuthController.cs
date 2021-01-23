using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.ViewModels;
using Kavenegar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;
using WebFramework.Filters;
using WebFramework.Filters.Controllers.AuthController;

namespace LMS.Controllers.V1
{
    [ApiController]
    [Route(template: Config.Routing.BaseRoute + "auth")]
    [ApiVersion(version: "1.0")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        private readonly Config.JWT        _JWT;
        private readonly Config.File       _File;
        private readonly IMailSender       _Mail;
        private readonly IConfiguration    _Config;
        
        public AuthController
        (
            UserManager<User>           UserManager  ,
            IOptions<Config.StatusCode> StatusCode   ,
            IOptions<Config.Messages>   StatusMessage,
            IOptions<Config.JWT>        JWT,
            IOptions<Config.File>       File,
            IMailSender                 Mail,
            IConfiguration              Config
        )
        {
            _UserManager   = UserManager;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            _JWT           = JWT.Value;
            _File          = File.Value;
            _Mail          = Mail;
            _Config        = Config;
        }

        [HttpPut]
        [Route(template: "register", Name = "Auth.Register")]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(RegisterHandling))]
        public async Task<JsonResult> Register(RegisterUserViewModel model)
        {
            User user = Request.HttpContext.GetRouteData().Values["User"] as User;
            
            IdentityResult result = await _UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string EmailCode = Request.HttpContext.GetRouteData().Values["EmailCode"] as string;
                string PhoneCode = $"PhoneVerifyCode: { Request.HttpContext.GetRouteData().Values["PhoneCode"] }";
                
                await _Mail.SendAsync(new List<string>{ model.Email }, "EmailVerifyCode", EmailCode);

                try
                {
                    KavenegarApi sms = new KavenegarApi(_Config.GetValue<string>("SMS:Key"));
                    sms.Send (
                        _Config.GetValue<string>("SMS:Sender"),
                        model.Phone,
                        PhoneCode
                    );
                }
                catch (Exception e)
                {
                    /*در این قسمت باید علت ارسال نشدن پیامک را به مدیر سیستم به روشی معین گزارش داد*/
                    await _Mail.SendAsync(new List<string>{ _Config.GetValue<string>("AdminData:Email") }, "ErrorSendSMS", e.Message);
                    return JsonResponse.Return(_StatusCode.ErrorCreate, "متاسفانه سامانه ارسال پیامک دچار مشکل شده است و سیستم قادر به ارسال کد اعتبارسنجی به شماره تماس شما نمی باشد . این مشکل در اسرع وقت برطرف خواهد شد", new { });
                }
                
                return JsonResponse.Return(_StatusCode.SuccessRegister, _StatusMessage.SuccessRegister, new { });
            }
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPatch]
        [Route(template: "verify-account", Name = "Auth.Verify.Account")]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> VerifyAccount(VerifyAccount model)
        {
            User user = await _UserManager.FindWithEmailCodeAsync(model.EmailCode);
            if ( user != null && await _UserManager.FindWithPhoneCodeAsync(model.PhoneCode) != null )
            {
                user.IsVerifyEmail = true;
                user.IsVerifyPhone = true;
                user.UpdatedAt     = PersianDatetime.Now();
                
                IdentityResult result = await _UserManager.UpdateAsync(user);
                if (result.Succeeded)
                    return JsonResponse.Return(_StatusCode.SuccessVerifyAccount, _StatusMessage.SuccessVerifyAccount, new { });
                return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
            }
            return JsonResponse.Return(_StatusCode.ErrorVerifyAccount, _StatusMessage.ErrorVerifyAccount, new { });
        }
        
        [HttpPatch]
        [Route(template: "verify-email", Name = "Auth.Verify.Email")]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> VerifyEmail(VerifyEmail model)
        {
            User user = await _UserManager.FindByEmailAsync(model.Email);
            if ( user != null )
            {
                if (user.IsVerifyEmail && user.IsVerifyPhone)
                {
                    string EmailCode = $"{user.UserName}-{Guid.NewGuid().ToString()}";
                    user.EmailCode   = EmailCode;
                    user.UpdatedAt   = PersianDatetime.Now();
                    
                    IdentityResult result = await _UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _Mail.SendAsync(new List<string>{ model.Email }, "EmailVerifyCode", EmailCode);
                        
                        return JsonResponse.Return(_StatusCode.SuccessVerifyEmail, _StatusMessage.SuccessVerifyEmail, new { });
                    }
                    return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
                }
                return JsonResponse.Return(_StatusCode.UnVerifyEmailAndPhone, _StatusMessage.UnVerifyEmailAndPhone, new { });
            }
            return JsonResponse.Return(_StatusCode.ErrorVerifyEmail, _StatusMessage.ErrorVerifyEmail, new { });
        }

        [HttpPatch]
        [Route(template: "reset-password", Name = "Auth.ResetPassword")]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> ResetPassword(ResetPassword model)
        {
            User user = await _UserManager.FindWithEmailCodeAsync(model.EmailCode);
            if (user != null)
            {
                if (user.IsVerifyEmail && user.IsVerifyPhone)
                {
                    user.UpdatedAt = PersianDatetime.Now();
                    IdentityResult resultUpdate   = await _UserManager.UpdateAsync(user);
                    IdentityResult resultPassword = await _UserManager.ResetPasswordAsync(user, await _UserManager.GeneratePasswordResetTokenAsync(user), model.NewPassword);
                    if (resultUpdate.Succeeded && resultPassword.Succeeded)
                        return JsonResponse.Return(_StatusCode.SuccessResetPassword, _StatusMessage.SuccessResetPassword, new { });
                    return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
                }
                return JsonResponse.Return(_StatusCode.UnVerifyEmailAndPhone, _StatusMessage.UnVerifyEmailAndPhone, new { });
            }
            return JsonResponse.Return(_StatusCode.ErrorResetPassword, _StatusMessage.ErrorResetPassword, new { });
        }

        [HttpPatch]
        [Route(template: "login", Name = "Auth.Login")]
        [ServiceFilter(type: typeof(LoginHandling))]
        public JsonResult Login(LoginUserViewModel model)
        {
            User user = Request.HttpContext.GetRouteData().Values["User"] as User;
            JWT code  = new JWT(_JWT.Key);

            /*PayLoad; Data*/
            return code.SetClaims
            (
                new Claim("UserPhone", user.Phone),
                new Claim("UserEmail", user.Email)
            )
            .SetClaimsIdentity(identity =>
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName)); /*این قسمت ضروری است . و از این ویژگی برای شناسایی کاربر لاگین کرده استفاده می شود*/
                
                return Task.CompletedTask;
            })
            .SetTokenDescriptor((descriptor, identity, credentials) =>
            {
                descriptor.Issuer             = _JWT.Issuer;
                descriptor.Audience           = _JWT.Audience;
                descriptor.Subject            = identity; /*محتوای اصلی توکن که حاوی رول های کاربر نیز می باشد*/
                descriptor.Expires            = DateTime.UtcNow.AddMinutes(_JWT.Expire);
                descriptor.SigningCredentials = credentials;

                return descriptor;
            })
            .Execute(result => JsonResponse.Return(_StatusCode.SuccessLogin, _StatusMessage.SuccessLogin, new
            {
                Token = result
            }));
        }
    }
}