using System;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;
using WebFramework.Filters;
using WebFramework.Filters.Controllers.ChapterController;

namespace LMS.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ChapterController : BaseChapterController
    {
        //DataService
        private readonly IServiceProvider _Provider;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Context
        private readonly DatabaseContext _Context;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public ChapterController (
            IServiceProvider            Provider,
            UserManager<User>           UserManager,
            DatabaseContext             Context,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _Provider = Provider;
            
            //Managers
            _UserManager = UserManager;
            
            //Context
            _Context = Context;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }
        
        [HttpGet]
        [Route(template: "", Name = "Chapter.All.Paginate")]
        public async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            ChapterService<ChaptersViewModel, Chapter> ChapterService = _Provider.GetRequiredService< ChapterService<ChaptersViewModel , Chapter> >();
            
            PaginatedList<ChaptersViewModel> Chapter = await _UserManager.HasRoleAsync(Request.HttpContext, "Admin") ? 
                                                       await ChapterService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage) : 
                                                       await ChapterService.FindAllForUserWithNoTrackingAndPaginateAsync(await _UserManager.GetCurrentUserAsync(Request.HttpContext), model.PageNumber, model.CountSizePerPage);

            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Chapter.CurrentPage,
                Chapter.CountSizePerPage,
                Chapter.TotalPages,
                Chapter.HasNext,
                Chapter.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Chapter });
        }

        [HttpPut]
        [Route(template: "create", Name = "Chapter.Create")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> Create(CreateChapterViewModel model)
        {
            Chapter Chapter = new Chapter
            {
                UserId    = (await _UserManager.GetCurrentUserAsync(Request.HttpContext)).Id,
                TermId    = model.Term,
                Title     = model.Title,
                CreatedAt = PersianDatetime.Now(),
                UpdatedAt = PersianDatetime.Now()
            };

            await _Context.Chapters.AddAsync(Chapter);
            if (Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPatch]
        [Route(template: "edit/{id:int}", Name = "Chapter.Edit")]
        [ServiceFilter(type: typeof(CheckChapter))]
        [ServiceFilter(type: typeof(ChapterPolicy))]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(CheckUniqueTitle))]
        public async Task<JsonResult> Edit(int id, EditChapterViewModel model)
        {
            Chapter Chapter = Request.HttpContext.GetRouteData().Values["Chapter"] as Chapter;

            Chapter.TermId    = model.Term;
            Chapter.Title     = model.Title;
            Chapter.UpdatedAt = PersianDatetime.Now();

            _Context.Chapters.Update(Chapter);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }

        [HttpPost]
        [Route(template: "delete/{id:int}", Name = "Chapter.Delete")]
        [ServiceFilter(type: typeof(CheckChapter))]
        [ServiceFilter(type: typeof(ChapterPolicy))]
        public async Task<JsonResult> Delete(int id)
        {
            _Context.Chapters.Remove(Request.HttpContext.GetRouteData().Values["Chapter"] as Chapter);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessDelete, _StatusMessage.SuccessDelete, new { });
            return JsonResponse.Return(_StatusCode.ErrorDelete, _StatusMessage.ErrorDelete, new { });
        }
    }
}