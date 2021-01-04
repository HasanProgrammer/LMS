using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace WebFramework.Filters.Areas.Admin.UserController
{
    public class RegisterHandling : ActionFilterAttribute
    {
        //Managers
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;

        public RegisterHandling
        (
            UserManager<User>           UserManager, 
            RoleManager<Role>           RoleManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Managers
            _UserManager   = UserManager;
            _RoleManager   = RoleManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //I
            CreateUserViewModel model = context.ActionArguments.Values.FirstOrDefault(Parameter => Parameter is CreateUserViewModel) as CreateUserViewModel;
            
            //II
            if (await _UserManager.FindWithUsernameAsync(model.Username) != null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.AlreadyUsedUsername);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.AlreadyUsedUsername, _StatusMessage.AlreadyUsedUsername, new {});
                return;
            }
            
            //III
            User user = new User
            {
                Id        = Guid.NewGuid().ToString(),
                UserName  = model.Username,
                Email     = model.Email,
                CreatedAt = PersianDatetime.Now(),
                UpdatedAt = PersianDatetime.Now()
            };
            
            //IV
            context.HttpContext.GetRouteData().Values.Add("User", user);

            //V
            await next();
        }
    }
}