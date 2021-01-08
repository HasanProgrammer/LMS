using System.Linq;
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
    public class CheckUniqueUserName : ActionFilterAttribute
    {
        //DataService
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUniqueUserName
        (
            UserManager<User>           UserManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            EditUserViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is EditUserViewModel) as EditUserViewModel;
            
            //II
            if (!model.Username.Equals( (context.HttpContext.GetRouteData().Values["User"] as User)?.UserName ) )
            {
                if (_UserManager.FindWithUsernameAsync(model.Username) != null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotUniqueUserNameField);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotUniqueUserNameField, _StatusMessage.IsNotUniqueUserNameField, new {});
                }
            }
        }
    }
}