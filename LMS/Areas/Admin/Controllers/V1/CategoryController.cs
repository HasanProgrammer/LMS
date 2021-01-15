using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Filters;
using WebFramework.Filters.Areas.Admin.CategoryController;
using CheckCategory = WebFramework.Filters.Areas.Admin.CategoryController.CheckCategory;

namespace LMS.Areas.Admin.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CategoryController : BaseCategoryController
    {
        //DataService
        private readonly CategoryService<CategoriesViewModel, Category> _CategoryService;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        //HttpContext
        private readonly HttpContext _HttpContext;
        
        //Context
        private readonly DatabaseContext _Context;
        
        public CategoryController
        (
            CategoryService<CategoriesViewModel, Category> CategoryService,
            IHttpContextAccessor        HttpContextAccessor,
            IOptions<Config.StatusCode> StatusCode,
            IOptions<Config.Messages>   StatusMessage,
            UserManager<User>           UserManager,
            DatabaseContext             Context
        )
        {
            //DataService
            _CategoryService = CategoryService;
            
            //Managers
            _UserManager     = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            
            //HttpContext
            _HttpContext = HttpContextAccessor.HttpContext;
            
            //Context
            _Context = Context;
        }

        [HttpGet]
        [Route(template: "", Name = "Admin.Category.All.Paginate")]
        public virtual async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            //I
            PaginatedList<CategoriesViewModel> Categories = await _CategoryService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage);
            
            //II
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Categories.CurrentPage,
                Categories.CountSizePerPage,
                Categories.TotalPages,
                Categories.HasNext,
                Categories.HasPrev
            });
            
            //III
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Categories });
        }
        
        [HttpPut]
        [Route(template: "create", Name = "Admin.Category.Create")]
        [ServiceFilter(type: typeof(ModelValidation))]
        public virtual async Task<JsonResult> Create([FromBody] CreateCategoryViewModel model)
        {
            //I
            Category category = new Category
            {
                Name      = model.Name,
                CreatedAt = PersianDatetime.Now(), 
                UpdatedAt = PersianDatetime.Now()
            };

            //II
            await _Context.Categories.AddAsync(category);
            
            //III
            if (Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessCreate, _StatusMessage.SuccessCreate, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPatch]
        [Route(template: "edit/{id:int}", Name = "Admin.Category.Edit")]
        [ServiceFilter(type: typeof(CheckCategory))]
        [ServiceFilter(type: typeof(ModelValidation))]
        [ServiceFilter(type: typeof(CheckUniqueName))]
        public virtual async Task<JsonResult> Edit(int id, [FromBody] EditCategoryViewModel model)
        {
            //I
            Category category  = _HttpContext.GetRouteData().Values["Category"] as Category;
            category.Name      = model.Name;
            category.UpdatedAt = PersianDatetime.Now();

            //II
            _Context.Categories.Update(category);
            
            //III
            if(Convert.ToBoolean(await _Context.SaveChangesAsync()))
                return JsonResponse.Return(_StatusCode.SuccessEdit, _StatusMessage.SuccessEdit, new { });
            return JsonResponse.Return(_StatusCode.ErrorCreate, _StatusMessage.ErrorCreate, new { });
        }

        [HttpPost]
        [Route(template: "delete/{id:int}", Name = "Admin.Category.Delete")]
        [ServiceFilter(type: typeof(CheckCategory))]
        public virtual JsonResult Delete(int id)
        {
            //I
            Category category = _HttpContext.GetRouteData().Values["Category"] as Category;

            //II
            _Context.Categories.Remove(category);

            //III
            if(Convert.ToBoolean(_Context.SaveChanges()))
                return JsonResponse.Return(_StatusCode.SuccessDelete, _StatusMessage.SuccessDelete, new { });
            return JsonResponse.Return(_StatusCode.ErrorDelete, _StatusMessage.ErrorDelete, new { });
        }
    }
}