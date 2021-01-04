using System.Linq;
using System.Text.RegularExpressions;
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
    public class CheckPassword : ActionFilterAttribute
    {
        //Services
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        private readonly Config.Password   _Password;
        
        public CheckPassword
        (
            UserManager<User>           UserManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage,
            IOptions<Config.Password>   Password
        )
        {
            //Services
            _UserManager = UserManager;
            
            //Configs
            _Password      = Password.Value;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            EditUserViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is EditUserViewModel) as EditUserViewModel;
            
            //II
            if (model.Password != null)
            {
                if (!Regex.IsMatch(model.Password, _Password.Regex))
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectPassword);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectPassword, _Password.RegexMessage, new {});
                }
            }
        }
    }
}