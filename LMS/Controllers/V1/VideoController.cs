using System;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;
using WebFramework.Filters;
using WebFramework.Filters.Controllers.VideoController;

namespace LMS.Controllers.V1
{
    [ApiVersion("1.0")]
    public class VideoController : BaseVideoController
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
        
        public VideoController (
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
        [Route(template: "", Name = "Video.All.Paginate")]
        public async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            UserManager<User> UserManager                     = _Provider.GetRequiredService< UserManager<User> >();
            VideoService<VideosViewModel, Video> VideoService = _Provider.GetRequiredService< VideoService<VideosViewModel , Video> >();
            
            PaginatedList<VideosViewModel> Videos = await UserManager.HasRoleAsync(Request.HttpContext, "Admin") ?
                                                    await VideoService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage) :
                                                    await VideoService.FindAllForUserWithNoTrackingAndPaginateAsync(await UserManager.GetCurrentUserAsync(Request.HttpContext), model.PageNumber, model.CountSizePerPage);

            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Videos.CurrentPage,
                Videos.CountSizePerPage,
                Videos.TotalPages,
                Videos.HasNext,
                Videos.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Videos });
        }
        
        [HttpPut]
        [Route(template: "create", Name = "Video.Create")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(CheckChapter))]
        [ServiceFilter(type: typeof(TermPolicy))]
        [ServiceFilter(type: typeof(ChapterPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(VideoUploader))]
        public async Task<JsonResult> Create([FromForm] CreateVideoViewModel model, IFormFile video)
        {
            Video Video = new Video
            {
                UserId    = (await _UserManager.GetCurrentUserAsync(Request.HttpContext)).Id,
                ChapterId = model.Chapter,
                TermId    = model.Term,
                Title     = model.Title,
                Duration  = model.Duration,
                VideoFile = Request.HttpContext.GetRouteData().Values["VideoPath"] as string,
                IsFree    = Convert.ToBoolean(model.IsFree),
                Status    = Model.Enums.Video.Status.Inactive,
                CreatedAt = PersianDatetime.Now(),
                UpdatedAt = PersianDatetime.Now()
            };

            await _Context.Videos.AddAsync(Video);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPatch]
        [Route(template: "edit/{id:int}", Name = "Video.Edit")]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(CheckChapter))]
        [ServiceFilter(type: typeof(TermPolicy))]
        [ServiceFilter(type: typeof(ChapterPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(VideoUploader))]
        public async Task<JsonResult> Edit(int id, [FromForm] EditVideoViewModel model, IFormFile video)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;

            Video.TermId    = model.Term;
            Video.ChapterId = model.Chapter;
            Video.Title     = model.Title;
            Video.Duration  = model.Duration;
            Video.IsFree    = Convert.ToBoolean(model.IsFree);

            if (video != null) Video.VideoFile = Request.HttpContext.GetRouteData().Values["VideoPath"] as string;

            _Context.Videos.Update(Video);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "active/{id:int}", Name = "Video.Active")]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        public async Task<JsonResult> Active(int id)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;
                
            Video.Status    = Model.Enums.Video.Status.Active;
            Video.UpdatedAt = PersianDatetime.Now();

            _Context.Videos.Update(Video);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "inactive/{id:int}", Name = "Video.InActive")]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        public async Task<JsonResult> InActive(int id)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;
                
            Video.Status    = Model.Enums.Video.Status.Inactive;
            Video.UpdatedAt = PersianDatetime.Now();

            _Context.Videos.Update(Video);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }

        [HttpPost]
        [Route(template: "delete/{id:int}", Name = "Video.Delete")]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        public async Task<JsonResult> Delete(int id)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;

            string UploadedPathAlready = Video.VideoFile;

            IWebHostEnvironment environment = _Provider.GetRequiredService<IWebHostEnvironment>();
            IOptions<Config.File> config    = _Provider.GetRequiredService<IOptions<Config.File>>();
            
            string UploadPathPublic  = environment.WebRootPath     + "\\" + config.Value.UploadPathVideoPublic;
            string UploadPathPrivate = environment.ContentRootPath + "\\" + config.Value.UploadPathVideoPrivate;
            
            if(System.IO.File.Exists((Video.IsFree ? UploadPathPublic : UploadPathPrivate) + UploadedPathAlready))
                System.IO.File.Delete((Video.IsFree ? UploadPathPublic : UploadPathPrivate) + UploadedPathAlready);

            _Context.Videos.Remove(Video);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessDelete, _StatusMessage.SuccessDelete, new { });
            return JsonResponse.Return(_StatusCode.ErrorDelete, _StatusMessage.ErrorDelete, new { });
        }
    }
}