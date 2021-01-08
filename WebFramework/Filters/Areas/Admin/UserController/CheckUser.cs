using System;
using System.Linq;
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

namespace WebFramework.Filters.Areas.Admin.UserController
{
    public class CheckUser : ActionFilterAttribute
    {
        //DataService
        private readonly UserService<UsersViewModel, User> _UserService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUser
        (
            IOptions<Config.StatusCode>       StatusCode, 
            IOptions<Config.Messages>         StatusMessage,
            UserService<UsersViewModel, User> UserService
        )
        {
            //DataService
            _UserService = UserService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            User user = _UserService.FindWithIdEntityWithEagerLoading( context.HttpContext.GetRouteData().Values["id"] as string );
            
            //II
            if (user == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
            }
            
            //III
            context.HttpContext.GetRouteData().Values.Add("User", user);
        }
    }
}