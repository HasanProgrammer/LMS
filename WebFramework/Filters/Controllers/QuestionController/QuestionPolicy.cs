using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace WebFramework.Filters.Controllers.QuestionController
{
    public class QuestionPolicy : ActionFilterAttribute
    {
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public QuestionPolicy
        (
            UserManager<User>           UserManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Managers
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Comment Comment = context.HttpContext.GetRouteData().Values["Comment"] as Comment;

            //II
            if (!_UserManager.HasRole(context.HttpContext, "Admin"))
            {
                if (!_UserManager.GetCurrentUser(context.HttpContext).Id.Equals(Comment.Term.UserId))
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.UnAuthorized);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.UnAuthorized, _StatusMessage.UnAuthorized, new {});
                }
            }
        }
    }
}