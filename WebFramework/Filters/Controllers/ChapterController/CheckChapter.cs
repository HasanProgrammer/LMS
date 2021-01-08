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

namespace WebFramework.Filters.Controllers.ChapterController
{
    public class CheckChapter : ActionFilterAttribute
    {
        //DataService
        private readonly ChapterService<ChaptersViewModel, Chapter> _ChapterService;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckChapter 
        (
            UserManager<User>           UserManager,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage,
            
            ChapterService<ChaptersViewModel, Chapter> ChapterService
        )
        {
            //DataService
            _ChapterService = ChapterService;
            
            //Managers
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Chapter Chapter = _ChapterService.FindWithIdEntity (
                Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"])
            );
            
            //II
            if (Chapter == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            //III
            context.HttpContext.GetRouteData().Values.Add("Chapter", Chapter);
        }
    }
}