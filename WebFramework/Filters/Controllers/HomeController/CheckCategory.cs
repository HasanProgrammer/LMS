using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Controllers.HomeController
{
    public class CheckCategory : ActionFilterAttribute
    {
        //DataService
        private readonly CategoryService<CategoriesViewModel, Category> _CategoryService;
        
        //Configs
        private readonly IConfiguration    _Config;
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckCategory
        (
            CategoryService<CategoriesViewModel, Category> CategoryService,
            IConfiguration              Config,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _CategoryService = CategoryService;
            
            //Configs
            _Config        = Config;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            string category = null;
            switch (context.HttpContext.GetRouteData().Values["name"] as string)
            {
                case "CSH"       : category = _Config.GetValue<string>("Category:C#");         break;
                case "PHP"       : category = _Config.GetValue<string>("Category:PHP");        break;
                case "PYT"       : category = _Config.GetValue<string>("Category:Python");     break;
                case "JS"        : category = _Config.GetValue<string>("Category:JavaScript"); break;
                case "ASP"       : category = _Config.GetValue<string>("Category:ASPCore");    break;
                case "Laravel"   : category = _Config.GetValue<string>("Category:Laravel");    break;
                case "Django"    : category = _Config.GetValue<string>("Category:Django");     break;
                case "ReactJS"   : category = _Config.GetValue<string>("Category:ReactJS");    break;
                case "SQLServer" : category = _Config.GetValue<string>("Category:SQLServer");  break;
                case "MySQL"     : category = _Config.GetValue<string>("Category:MySQL");      break;
            }
            
            //II
            Category Category = _CategoryService.FindWithNameEntityWithNoTracking(category);
            
            //II
            if (Category == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            context.HttpContext.GetRouteData().Values.Add("Category", category);
        }
    }
}