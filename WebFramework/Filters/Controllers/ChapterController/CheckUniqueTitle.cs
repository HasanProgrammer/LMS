using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Controllers.ChapterController
{
    public class CheckUniqueTitle : ActionFilterAttribute
    {
        //DataService
        private readonly ChapterService<ChaptersViewModel, Chapter> _ChapterService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUniqueTitle
        (
            ChapterService<ChaptersViewModel, Chapter> ChapterService,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _ChapterService = ChapterService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            EditChapterViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is EditChapterViewModel) as EditChapterViewModel;
            
            //II
            if (!model.Title.Equals( (context.HttpContext.GetRouteData().Values["Chapter"] as Chapter)?.Title ) )
            {
                if (_ChapterService.FindWithTitleEntityWithNoTracking(model.Title) != null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotUniqueTitleField);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotUniqueTitleField, _StatusMessage.IsNotUniqueTitleField, new {});
                }
            }
        }
    }
}