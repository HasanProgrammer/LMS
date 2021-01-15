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

namespace WebFramework.Filters.Controllers.AnswerController
{
    public class CheckAnswer : ActionFilterAttribute
    {
        //Services
        private readonly AnswerService<AnswersViewModel, Answer> _AnswerService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckAnswer
        (
            AnswerService<AnswersViewModel, Answer> AnswerService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _AnswerService = AnswerService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Answer Answer = _AnswerService.FindWithIdEntity( Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"]) );
            
            //II
            if (Answer == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
            }
            
            //III
            context.HttpContext.GetRouteData().Values.Add("Answer", Answer);
        }
    }
}