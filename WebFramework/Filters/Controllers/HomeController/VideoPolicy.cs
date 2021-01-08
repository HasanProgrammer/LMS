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

namespace WebFramework.Filters.Controllers.HomeController
{
    public class VideoPolicy : ActionFilterAttribute
    {
        //DataService
        private readonly VideoService<VideosViewModel, Video> _VideoService;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public VideoPolicy
        (
            UserManager<User>                    UserManager, 
            IOptions<Config.StatusCode>          StatusCode, 
            IOptions<Config.Messages>            StatusMessage,
            VideoService<VideosViewModel, Video> VideoService
        )
        {
            //DataService
            _VideoService = VideoService;
            
            //Managers
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Video Video = context.HttpContext.GetRouteData().Values["Video"] as Video;
            
            //II
            if (!Video.IsFree)
            {
                if (Video.TermId != _UserManager.GetCurrentUserWithPurchasesTerm(context.HttpContext)?.Buys?.SingleOrDefault(Buy => Buy.TermId == Video.TermId)?.TermId)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                }
            }
        }
    }
}