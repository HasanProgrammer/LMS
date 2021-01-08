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

namespace WebFramework.Filters.Controllers.HomeController
{
    public class CheckVideo : ActionFilterAttribute
    {
        //DataService
        private readonly VideoService<VideosViewModel, Video> _VideoService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckVideo
        (
            VideoService<VideosViewModel, Video> VideoService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _VideoService = VideoService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Video Video = _VideoService.FindWithIdEntityAsNoTracking( Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"]) );
            
            //II
            if (Video == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            //III
            if (Video.Status == Model.Enums.Video.Status.Inactive)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            //IV
            context.HttpContext.GetRouteData().Values.Add("Video", Video);
        }
    }
}