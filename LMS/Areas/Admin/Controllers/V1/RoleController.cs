using System;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Filters;

namespace LMS.Areas.Admin.Controllers.V1
{
    [ApiVersion("1.0")]
    public class RoleController : BaseRoleController
    {
        //Services
        private readonly IRoleService<RolesViewModel, Role> _RoleService;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public RoleController
        (
            IRoleService<RolesViewModel, Role> RoleService,
            UserManager<User>           UserManager,
            RoleManager<Role>           RoleManager,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _RoleService = RoleService;
            
            //Managers
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        [HttpGet]
        [Route(template: "", Name = "Admin.Role.All.Paginate")]
        public virtual async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            //I
            PaginatedList<RolesViewModel> Roles = await _RoleService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage);
            
            //II
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Roles.CurrentPage,
                Roles.CountSizePerPage,
                Roles.TotalPages,
                Roles.HasNext,
                Roles.HasPrev
            });
            
            //III
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Roles });
        }

        [HttpPut]
        [Route(template: "create", Name = "Admin.Role.Create")]
        [ServiceFilter(type: typeof(ModelValidation))]
        public virtual async Task<JsonResult> Create([FromBody] CreateRoleViewModel model)
        {
            //I
            Role role = new Role
            {
                Id   = Guid.NewGuid().ToString(),
                Name = model.Name 
            };

            //II
            IdentityResult result = await _RoleManager.CreateAsync(role);
            
            //III
            if (result.Succeeded)
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }
    }
}