using System.Linq;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Areas.Admin.CategoryController
{
    public class CheckUniqueName : ActionFilterAttribute
    {
        //Services
        private readonly ICategoryService<CategoriesViewModel, Category> _CategoryService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUniqueName
        (
            ICategoryService<CategoriesViewModel, Category> CategoryService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _CategoryService = CategoryService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            EditCategoryViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is EditCategoryViewModel) as EditCategoryViewModel;
            
            //II
            if (!model.Name.Equals( (context.HttpContext.GetRouteData().Values["Category"] as Category)?.Name ) )
            {
                if (_CategoryService.FindWithNameEntityWithNoTracking(model.Name) != null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotUniqueNameField);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotUniqueNameField, _StatusMessage.IsNotUniqueNameField, new {});
                }
            }
        }
    }
}