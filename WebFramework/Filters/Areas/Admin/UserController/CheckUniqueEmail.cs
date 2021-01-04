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
    public class CheckUniqueEmail : ActionFilterAttribute
    {
        //Services
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUniqueEmail
        (
            UserManager<User>           UserManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
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
            if (!model.Email.Equals( (context.HttpContext.GetRouteData().Values["User"] as User)?.Email ) )
            {
                if (_UserManager.FindWithUsernameAsync(model.Email) != null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotUniqueEmailField);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotUniqueEmailField, _StatusMessage.IsNotUniqueEmailField, new {});
                }
            }
        }
    }
}