using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Filters;
using RegisterHandling = WebFramework.Filters.Areas.Admin.UserController.RegisterHandling;

namespace LMS.Areas.Admin.Controllers.V1
{
    [ApiVersion(version: "1.0")]
    public class UserController : BaseUserController
    {
        //Services
        protected readonly IUserService<UsersViewModel, User> _UserService;
        
        //Managers
        protected readonly UserManager<User> _UserManager;
        protected readonly RoleManager<Role> _RoleManager;
        
        //Configs
        protected readonly Config.StatusCode _StatusCode;
        protected readonly Config.Messages   _StatusMessage;
        
        public UserController
        (
            IUserService<UsersViewModel, User> UserService,
            UserManager<User>           UserManager,
            RoleManager<Role>           RoleManager,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _UserService = UserService;
            
            //Managers
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        [HttpGet]
        [Route(template: "", Name = "Admin.User.All.Paginate")]
        public virtual async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            //I
            PaginatedList<UsersViewModel> Users = await _UserService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage);
            
            //II
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Users.CurrentPage,
                Users.CountSizePerPage,
                Users.TotalPages,
                Users.HasNext,
                Users.HasPrev
            });
            
            //III
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Users });
        }

        [HttpPut]
        [Route(template: "create", Name = "Admin.User.Create")]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(RegisterHandling))]
        public virtual async Task<JsonResult> Create([FromBody] CreateUserViewModel model)
        {
            //I
            User user = Request.HttpContext.GetRouteData().Values["User"] as User;
            
            //II
            IdentityResult result = await _UserManager.CreateAsync(user, model.Password);
            
            //III
            if (result.Succeeded)
            {
                //IV
                IdentityResult final = await _UserManager.AddToRolesAsync(user, Request.HttpContext.GetRouteData().Values["Roles"] as List<string>);
                
                //V
                if (final.Succeeded)
                    return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
                return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
            }
            
            //IV
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }
    }
}