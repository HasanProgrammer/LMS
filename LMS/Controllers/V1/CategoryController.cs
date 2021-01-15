using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace LMS.Controllers.V1
{
    [ApiVersion(version: "1.0")]
    public class CategoryController : BaseCategoryController
    {
        //Services
        private readonly IServiceProvider _Provider;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;

        public CategoryController(IServiceProvider Provider, IOptions<Config.StatusCode> StatusCode, IOptions<Config.Messages> StatusMessage)
        {
            //Services
            _Provider = Provider;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }
        
        [HttpGet]
        [Route(template: "all", Name = "Category.All")]
        public async Task<JsonResult> All()
        {
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new
            {
                Categories = await _Provider.GetRequiredService< CategoryService<CategoriesViewModel , Category> >().FindAllWithNoTrackingAsync()
            });
        }
    }
}