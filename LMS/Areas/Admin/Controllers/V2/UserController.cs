using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Filters;
using WebFramework.Filters.Areas.Admin.UserController;
using RegisterHandling = WebFramework.Filters.Areas.Admin.UserController.RegisterHandling;

namespace LMS.Areas.Admin.Controllers.V2
{
    [ApiVersion(version: "2.0")]
    public class UserController : V1.UserController
    {
        protected readonly DatabaseContext _Context;
        
        public UserController
        (
            UserManager<User>                  UserManager,
            RoleManager<Role>                  RoleManager,
            DatabaseContext                    Context,
            UserService<UsersViewModel, User>  UserService,
            IOptions<Config.StatusCode>        StatusCode,
            IOptions<Config.Messages>          StatusMessage
        )
        : base(UserService, UserManager, RoleManager, StatusCode, StatusMessage)
        {
            _Context = Context;
        }

        [NonAction]
        public override Task<JsonResult> Create(CreateUserViewModel model)
        {
            return base.Create(model);
        }

        [HttpPut]
        [Route(template: "create", Name = "Admin.User.Create")]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(RegisterHandling))]
        [ServiceFilter(type: typeof(ImageUploader))]
        public virtual async Task<JsonResult> Create([FromForm] CreateUserViewModel model, IFormFile image)
        {
            //I
            Image newImage = null;
            if (image != null)
            {
                newImage = new Image
                {
                    Path      = Request.HttpContext.GetRouteData().Values["ImagePath"] as string,
                    Type      = Request.HttpContext.GetRouteData().Values["ImageType"] as string,
                    CreatedAt = PersianDatetime.Now(),
                    UpdatedAt = PersianDatetime.Now()
                };
                
                await _Context.Images.AddAsync(newImage);
                await _Context.SaveChangesAsync();
            }

            //II
            User user = Request.HttpContext.GetRouteData().Values["User"] as User;
            
            //III
            user.ImageId = newImage?.Id;
            
            //IV
            IdentityResult result = await _UserManager.CreateAsync(user, model.Password);
            
            //V
            if (result.Succeeded)
            {
                //VI
                IdentityResult final = await _UserManager.AddToRolesAsync(user, Request.HttpContext.GetRouteData().Values["Roles"] as List<string>);
                
                //VII
                if (final.Succeeded)
                    return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
                return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
            }
            
            //VI
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPatch]
        [Route(template: "edit/{id}", Name = "Admin.User.Edit")]
        [ServiceFilter(type: typeof(CheckUser))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(CheckUniqueUserName))]
        [ServiceFilter(type: typeof(CheckUniqueEmail))]
        [ServiceFilter(type: typeof(CheckPassword))]
        [ServiceFilter(type: typeof(ImageUploader))]
        public virtual async Task<JsonResult> Edit(string id, [FromForm] EditUserViewModel model, IFormFile image)
        {
            //I
            User user          = Request.HttpContext.GetRouteData().Values["User"]      as User;
            List<string> roles = Request.HttpContext.GetRouteData().Values["Roles"]     as List<string>;
            string ImagePath   = Request.HttpContext.GetRouteData().Values["ImagePath"] as string;
            string ImageType   = Request.HttpContext.GetRouteData().Values["ImageType"] as string;
            
            //II
            Image newImage = null;
            if (image != null)
            {
                if (user.Image != null) /*در این قسمت ؛ تصویر قبلی کاربر بروز رسانی می گردد*/
                {
                    user.Image.Path      = ImagePath;
                    user.Image.Type      = ImageType;
                    user.Image.UpdatedAt = PersianDatetime.Now();
                    await _Context.SaveChangesAsync();
                }
                else /*در این قسمت ؛ تصویر جدیدی برای کاربر اعمال می گردد*/
                {
                    newImage = new Image
                    {
                        Path      = ImagePath,
                        Type      = ImageType,
                        CreatedAt = PersianDatetime.Now(),
                        UpdatedAt = PersianDatetime.Now()
                    };
                
                    await _Context.Images.AddAsync(newImage);
                    await _Context.SaveChangesAsync();
                }
            }
            
            //III
            if(newImage != null) user.ImageId = newImage.Id;
            user.UserName    = model.Username;
            user.Phone       = model.Phone;
            user.Email       = model.Email;
            user.Description = model.Description;
            user.Expert      = model.Expert;
            user.UpdatedAt   = PersianDatetime.Now();
            
            //IV
            if(model.Password != null) await _UserManager.ResetPasswordAsync(user, await _UserManager.GeneratePasswordResetTokenAsync(user), model.Password);
            IdentityResult result = await _UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                //V
                await _UserManager.RemoveFromRolesAsync(user, await _UserManager.GetRolesAsync(user));
                IdentityResult final = await _UserManager.AddToRolesAsync(user, roles);
                if (final.Succeeded)
                    return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
                return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
            }
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }

        [HttpPatch]
        [Route(template: "active/{id}", Name = "Admin.User.Active")]
        [ServiceFilter(type: typeof(CheckUser))]
        public async Task<JsonResult> Active(string id)
        {
            User User = Request.HttpContext.GetRouteData().Values["User"] as User;
                
            User.Status    = Model.Enums.User.Status.Active;
            User.UpdatedAt = PersianDatetime.Now();

            IdentityResult result = await _UserManager.UpdateAsync(User);
            if(result.Succeeded)
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "inactive/{id}", Name = "Admin.User.InActive")]
        [ServiceFilter(type: typeof(CheckUser))]
        public async Task<JsonResult> InActive(string id)
        {
            User User = Request.HttpContext.GetRouteData().Values["User"] as User;
                
            User.Status    = Model.Enums.User.Status.Inactive;
            User.UpdatedAt = PersianDatetime.Now();
            
            IdentityResult result = await _UserManager.UpdateAsync(User);
            if(result.Succeeded)
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
    }
}