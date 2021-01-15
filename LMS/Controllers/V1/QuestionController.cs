using System;
using System.Collections.Generic;
using System.Linq;
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
using WebFramework.Filters.Controllers.QuestionController;

namespace LMS.Controllers.V1
{
    [ApiVersion(version: "1.0")]
    public class QuestionController : BaseQuestionController
    {
        //Services
        private readonly IServiceProvider                            _Provider;
        private readonly QuestionService<CommentsViewModel, Comment> _QuestionService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        //Context
        private readonly DatabaseContext _Context;
        
        public QuestionController (
            QuestionService<CommentsViewModel, Comment> QuestionService,
            DatabaseContext             Context,
            IServiceProvider            Provider,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _Provider        = Provider;
            _QuestionService = QuestionService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            
            //Context
            _Context = Context;
        }

        [HttpGet]
        [Route(template: "", Name = "Question.All.Paginate")]
        public async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            UserManager<User> UserManager = _Provider.GetRequiredService< UserManager<User> >();
            
            PaginatedList<CommentsViewModel> Questions = await UserManager.HasRoleAsync(Request.HttpContext, "Admin") ?
                                                         await _QuestionService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage) :
                                                         await _QuestionService.FindAllForUserWithNoTrackingAndPaginateAsync(await UserManager.GetCurrentUserAsync(Request.HttpContext), model.PageNumber, model.CountSizePerPage);
            
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Questions.CurrentPage,
                Questions.CountSizePerPage,
                Questions.TotalPages,
                Questions.HasNext,
                Questions.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Questions });
        }

        [HttpPatch]
        [Route(template: "active/{id:int}", Name = "Question.Active")]
        [ServiceFilter(type: typeof(CheckQuestion))]
        [ServiceFilter(type: typeof(QuestionPolicy))]
        public async Task<JsonResult> Active(int id)
        {
            Comment Comment = Request.HttpContext.GetRouteData().Values["Comment"] as Comment;

            Comment.Show      = true;
            Comment.UpdatedAt = PersianDatetime.Now();

            _Context.Comments.Update(Comment);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "inactive/{id:int}", Name = "Question.InActive")]
        [ServiceFilter(type: typeof(CheckQuestion))]
        [ServiceFilter(type: typeof(QuestionPolicy))]
        public async Task<JsonResult> InActive(int id)
        {
            Comment Comment = Request.HttpContext.GetRouteData().Values["Comment"] as Comment;

            Comment.Show      = false;
            Comment.UpdatedAt = PersianDatetime.Now();

            _Context.Comments.Update(Comment);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
    }
}