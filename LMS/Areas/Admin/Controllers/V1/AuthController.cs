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

namespace LMS.Areas.Admin.Controllers.V1
{
    [ApiController]
    [Route(template: Config.Routing.BaseRoute + "admin/auth")]
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

        [HttpPatch]
        [Route(template: "login", Name = "Admin.Auth.Login")]
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
                /*این قسمت ضروری است . و از این ویژگی برای شناسایی کاربر لاگین کرده استفاده می شود*/
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                
                /*این قسمت ضروری است . و از این ویژگی برای شناسایی ( نقش ) کاربر لاگین کرده استفاده می شود*/
                identity.AddClaims
                (
                    _UserManager.GetRolesAsync(user).Result.Select(role => new Claim(ClaimTypes.Role, role))
                );
                
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