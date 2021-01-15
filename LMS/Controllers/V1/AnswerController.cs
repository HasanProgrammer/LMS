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
using WebFramework.Filters.Controllers.AnswerController;

namespace LMS.Controllers.V1
{
    [ApiVersion(version: "1.0")]
    public class AnswerController : BaseAnswerController
    {
        //Services
        private readonly IServiceProvider                        _Provider;
        private readonly AnswerService<AnswersViewModel, Answer> _AnswerService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        //Context
        private readonly DatabaseContext _Context;
        
        public AnswerController (
            AnswerService<AnswersViewModel, Answer> AnswerService,
            DatabaseContext             Context,
            IServiceProvider            Provider,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _Provider      = Provider;
            _AnswerService = AnswerService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            
            //Context
            _Context = Context;
        }

        [HttpGet]
        [Route(template: "{id:int}", Name = "Answer.All.Paginate")]
        [ServiceFilter(type: typeof(CheckQuestion))]
        [ServiceFilter(type: typeof(QuestionPolicy))]
        public async Task<JsonResult> Index(int id, [FromQuery] PaginateQueryViewModel model)
        {
            UserManager<User> UserManager = _Provider.GetRequiredService< UserManager<User> >();
            
            PaginatedList<AnswersViewModel> Answers = await UserManager.HasRoleAsync(Request.HttpContext, "Admin") ?
                                                      await _AnswerService.FindAllForQuestionWithNoTrackingAndPaginateAsync(id, model.PageNumber, model.CountSizePerPage) :
                                                      await _AnswerService.FindAllForUserAndQuestionWithNoTrackingAndPaginateAsync(await UserManager.GetCurrentUserAsync(Request.HttpContext), id, model.PageNumber, model.CountSizePerPage);

            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Answers.CurrentPage,
                Answers.CountSizePerPage,
                Answers.TotalPages,
                Answers.HasNext,
                Answers.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Answers });
        }

        [HttpPut]
        [Route(template: "answer/{id:int}", Name = "Answer.Answer")]
        [ServiceFilter(type: typeof(CheckQuestion))]
        [ServiceFilter(type: typeof(QuestionPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> Answer(int id, CreateAnswerViewModel model)
        {
            Answer Answer = new Answer
            {
                UserId    = (await _Provider.GetRequiredService< UserManager<User> >().GetCurrentUserAsync(Request.HttpContext)).Id,
                CommentId = id,
                Show      = true,
                Content   = model.Content,
                CreatedAt = PersianDatetime.Now(),
                UpdatedAt = PersianDatetime.Now()
            };

            await _Context.Answers.AddAsync(Answer);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }
        
        [HttpPatch]
        [Route(template: "answer/edit/{id:int}", Name = "Answer.Answer.Edit")]
        [ServiceFilter(type: typeof(CheckAnswer))]
        [ServiceFilter(type: typeof(AnswerPolicy))]
        [ServiceFilter(type: typeof(ModelValidation))]
        public async Task<JsonResult> Edit(int id, EditAnswerViewModel model)
        {
            Answer Answer = Request.HttpContext.GetRouteData().Values["Answer"] as Answer;

            Answer.Content   = model.Content;
            Answer.UpdatedAt = PersianDatetime.Now();

            _Context.Answers.Update(Answer);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }
        
        [HttpPatch]
        [Route(template: "active/{id:int}", Name = "Answer.Active")]
        [ServiceFilter(type: typeof(CheckAnswer))]
        [ServiceFilter(type: typeof(AnswerPolicy))]
        public async Task<JsonResult> Active(int id)
        {
            Answer Answer = Request.HttpContext.GetRouteData().Values["Answer"] as Answer;

            Answer.Show      = true;
            Answer.UpdatedAt = PersianDatetime.Now();

            _Context.Answers.Update(Answer);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "inactive/{id:int}", Name = "Answer.InActive")]
        [ServiceFilter(type: typeof(CheckAnswer))]
        [ServiceFilter(type: typeof(AnswerPolicy))]
        public async Task<JsonResult> InActive(int id)
        {
            Answer Answer = Request.HttpContext.GetRouteData().Values["Answer"] as Answer;

            Answer.Show      = false;
            Answer.UpdatedAt = PersianDatetime.Now();

            _Context.Answers.Update(Answer);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
    }
}