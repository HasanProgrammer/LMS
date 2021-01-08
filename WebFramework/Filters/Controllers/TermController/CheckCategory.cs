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

namespace WebFramework.Filters.Controllers.TermController
{
    public class CheckCategory : ActionFilterAttribute
    {
        //DataService
        private readonly CategoryService<CategoriesViewModel, Category> _CategoryService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckCategory
        (
            CategoryService<CategoriesViewModel, Category> CategoryService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _CategoryService = CategoryService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Category Category = _CategoryService.FindWithIdEntityAsNoTracking (
                ( context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is CreateTermViewModel) as CreateTermViewModel )?.Category
            );
            
            //II
            if (Category == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
            }
        }
    }
}