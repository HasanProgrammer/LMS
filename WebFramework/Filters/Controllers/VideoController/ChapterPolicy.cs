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

namespace WebFramework.Filters.Controllers.VideoController
{
    public class ChapterPolicy : ActionFilterAttribute
    {
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public ChapterPolicy
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
            Chapter Chapter = context.HttpContext.GetRouteData().Values["Chapter"] as Chapter;

            //II
            if (Chapter != null)
            {
                /*در این قسمت بررسی می گردد که فصل انتخاب شده از فصول دوره ی انتخاب شده است یا خیر | اگر خیر؛ پس این فیلم نمی تواند منتشر شود*/
                /*باید فصل انتخابی مربوط به فصول دوره ی انتخابی باشد*/
                if ((context.HttpContext.GetRouteData().Values["Term"] as Term).Chapters.SingleOrDefault(Item => Item.Id == Chapter.Id) == null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.NotFound, "فصل انتخابی باید از بین فصول دوره ی انتخابی باشد", new {});
                }
                
                if (!_UserManager.HasRole(context.HttpContext, "Admin"))
                {
                    if (!_UserManager.GetCurrentUser(context.HttpContext).Id.Equals(Chapter.UserId))
                    {
                        JsonResponse.Handle(context.HttpContext, _StatusCode.UnAuthorized);
                        context.Result = new EmptyResult();
                        context.Result = JsonResponse.Return(_StatusCode.UnAuthorized, _StatusMessage.UnAuthorized, new {});
                    }
                }
            }
        }
    }
}