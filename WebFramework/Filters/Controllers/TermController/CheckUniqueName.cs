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
    public class CheckUniqueName : ActionFilterAttribute
    {
        //Services
        private readonly ITermService<TermsViewModel, Term> _TermService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckUniqueName
        (
            ITermService<TermsViewModel, Term> TermService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //Services
            _TermService = TermService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            EditTermViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is EditTermViewModel) as EditTermViewModel;
            
            //II
            if (!model.Name.Equals( (context.HttpContext.GetRouteData().Values["Term"] as Term)?.Name ) )
            {
                if (_TermService.FindWithNameEntityWithNoTracking(model.Name) != null)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotUniqueNameField);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotUniqueNameField, _StatusMessage.IsNotUniqueNameField, new {});
                }
            }
        }
    }
}