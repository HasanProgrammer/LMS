using System;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using LMS.Controllers;
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
using WebFramework.Filters.Controllers.TermController;

namespace LMS.Controllers.V1
{
    [ApiVersion("1.0")]
    public class TermController : BaseTermController
    {
        private readonly IServiceProvider  _Provider;
        private readonly DatabaseContext   _Context;
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public TermController (
            IServiceProvider            Provider,
            DatabaseContext             Context,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            _Provider      = Provider;
            _Context       = Context;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        [HttpGet]
        [Route(template: "", Name = "Term.All.Paginate")]
        public async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            UserManager<User> UserManager                 = _Provider.GetRequiredService< UserManager<User> >();
            TermService<TermsViewModel, Term> TermService = _Provider.GetRequiredService< TermService<TermsViewModel , Term> >();
            
            PaginatedList<TermsViewModel> Terms = await UserManager.HasRoleAsync(Request.HttpContext, "Admin") ?
                                                  await TermService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage) :
                                                  await TermService.FindAllForUserWithNoTrackingAndPaginateAsync(await UserManager.GetCurrentUserAsync(Request.HttpContext), model.PageNumber, model.CountSizePerPage);

            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Terms.CurrentPage,
                Terms.CountSizePerPage,
                Terms.TotalPages,
                Terms.HasNext,
                Terms.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Terms });
        }

        [HttpPut]
        [Route(template: "create", Name = "Term.Create")]
        [ServiceFilter(type: typeof(CheckCategory))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(ImageUploader))]
        public async Task<JsonResult> Create([FromForm] CreateTermViewModel model, IFormFile image)
        {
            UserManager<User> Manager = _Provider.GetRequiredService<UserManager<User>>();

            Image IMG = new Image
            {
                Path      = Request.HttpContext.GetRouteData().Values["ImagePath"] as string,
                Type      = Request.HttpContext.GetRouteData().Values["ImageType"] as string,
                CreatedAt = PersianDatetime.Now(),
                UpdatedAt = PersianDatetime.Now()
            };

            await _Context.Images.AddAsync(IMG);
            await _Context.SaveChangesAsync();

            Term Term = new Term
            {
                UserId      = (await Manager.GetCurrentUserAsync(Request.HttpContext)).Id,
                CategoryId  = model.Category,
                ImageId     = IMG.Id, 
                Name        = model.Name,
                Description = model.Description,
                Suitable    = model.Suitable,
                Result      = model.Result,
                Price       = model.Price,
                HasChapter  = Convert.ToBoolean(model.HasChapter),
                Status      = Model.Enums.Term.Status.Inactive, 
                CreatedAt   = PersianDatetime.Now(),
                UpdatedAt   = PersianDatetime.Now()
            };

            await _Context.Terms.AddAsync(Term);

            if (Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }
        
        [HttpPatch]
        [Route(template: "edit/{id:int}", Name = "Term.Edit")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        [ServiceFilter(type: typeof(CheckCategory))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(CheckUniqueName))]
        [ServiceFilter(type: typeof(ImageUploader))]
        public async Task<JsonResult> Edit(int id, [FromForm] EditTermViewModel model, IFormFile image)
        {
            Term Term = Request.HttpContext.GetRouteData().Values["Term"] as Term;
            
            if (image != null)
            {
                Image Image = Term.Image;
                
                Image.Path      = Request.HttpContext.GetRouteData().Values["ImagePath"] as string;
                Image.Type      = Request.HttpContext.GetRouteData().Values["ImageType"] as string;
                Image.UpdatedAt = PersianDatetime.Now();

                _Context.Images.Update(Image);
            }

            Term.CategoryId  = model.Category;
            Term.Name        = model.Name;
            Term.Description = model.Description;
            Term.Suitable    = model.Suitable;
            Term.Result      = model.Result;
            Term.Price       = model.Price;
            Term.HasChapter  = Convert.ToBoolean(model.HasChapter);
            Term.UpdatedAt   = PersianDatetime.Now();

            _Context.Terms.Update(Term);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }

        [HttpPatch]
        [Route(template: "active/{id:int}", Name = "Term.Active")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        public async Task<JsonResult> Active(int id)
        {
            Term Term = Request.HttpContext.GetRouteData().Values["Term"] as Term;
                
            Term.Status    = Model.Enums.Term.Status.Active;
            Term.UpdatedAt = PersianDatetime.Now();

            _Context.Terms.Update(Term);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }
        
        [HttpPatch]
        [Route(template: "inactive/{id:int}", Name = "Term.InActive")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        public async Task<JsonResult> InActive(int id)
        {
            Term Term = Request.HttpContext.GetRouteData().Values["Term"] as Term;
                
            Term.Status    = Model.Enums.Term.Status.Inactive;
            Term.UpdatedAt = PersianDatetime.Now();
            
            _Context.Terms.Update(Term);
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorEdit, _StatusMessage.ErrorEdit, new { });
        }

        [HttpPost]
        [Route(template: "delete/{id:int}", Name = "Term.Delete")]
        [ServiceFilter(type: typeof(CheckTerm))]
        [ServiceFilter(type: typeof(TermPolicy))]
        public async Task<JsonResult> Delete(int id)
        {
            Term Term = Request.HttpContext.GetRouteData().Values["Term"] as Term;

            string UploadedPathAlready = Term.Image.Path;
            string RootPath = _Provider.GetRequiredService<IWebHostEnvironment>().WebRootPath + "\\" + _Provider.GetRequiredService<IOptions<Config.File>>().Value.UploadPathImagePublic;
            if(System.IO.File.Exists(RootPath + UploadedPathAlready))
                System.IO.File.Delete(RootPath + UploadedPathAlready);

            _Context.Images.Remove(Term.Image);
            _Context.Terms.Remove(Term);
            
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessDelete, _StatusMessage.SuccessDelete, new { });
            return JsonResponse.Return(_StatusCode.ErrorDelete, _StatusMessage.ErrorDelete, new { });
        }
    }
}