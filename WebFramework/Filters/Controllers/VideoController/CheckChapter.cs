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
    public class CheckChapter : ActionFilterAttribute
    {
        //DataService
        private readonly TermService<TermsViewModel, Term>          _TermService;
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
            
            TermService<TermsViewModel, Term>          TermService,
            ChapterService<ChaptersViewModel, Chapter> ChapterService
        )
        {
            //DataService
            _TermService    = TermService;
            _ChapterService = ChapterService;
            
            //Managers
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CreateVideoViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is CreateVideoViewModel) as CreateVideoViewModel; int? ChapterId = model.Chapter;
            
            if(_TermService.FindWithIdEntityAsNoTracking(model.Term).HasChapter && ChapterId == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, "دوره ی برنامه نویسی مدنظر دارای فصل بندی است . بنابراین فیلم مورد نظر باید در قالب یک فصل منتشر گردد", new {});
                return;
            }
            
            Chapter Chapter = _ChapterService.FindWithIdEntityAsNoTracking(ChapterId);
            
            if (ChapterId != null && Chapter == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
            }
            
            context.HttpContext.GetRouteData().Values.Add("Chapter", Chapter);
        }
    }
}