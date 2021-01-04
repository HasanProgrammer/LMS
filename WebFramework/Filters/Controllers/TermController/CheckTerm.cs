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
    public class CheckTerm : ActionFilterAttribute
    {
        //Services
        private readonly ITermService<TermsViewModel, Term> _TermService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckTerm
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
            Term term = _TermService.FindWithIdEntity( Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"]) );
            
            //II
            if (term == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
            }
            
            //III
            context.HttpContext.GetRouteData().Values.Add("Term", term);
        }
    }
}