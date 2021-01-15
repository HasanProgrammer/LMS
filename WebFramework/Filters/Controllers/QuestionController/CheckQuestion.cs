using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Controllers.QuestionController
{
    public class CheckQuestion : ActionFilterAttribute
    {
        //DataService
        private readonly QuestionService<CommentsViewModel, Comment> _QuestionService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckQuestion
        (
            QuestionService<CommentsViewModel, Comment> QuestionService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _QuestionService = QuestionService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Comment Comment = _QuestionService.FindWithIdEntityWithEagerLoading( Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"]) );
            
            //II
            if (Comment == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }

            //III
            context.HttpContext.GetRouteData().Values.Add("Comment", Comment);
        }
    }
}