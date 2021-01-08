using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Controllers.HomeController
{
    public class CheckTerm : ActionFilterAttribute
    {
        //DataService
        private readonly TermService<TermsViewModel, Term> _TermService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckTerm
        (
            TermService<TermsViewModel, Term> TermService, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            //DataService
            _TermService = TermService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Term Term = _TermService.FindWithIdEntityAsNoTracking( Convert.ToInt32(context.HttpContext.GetRouteData().Values["id"]) );
            
            //II
            if (Term == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }

            //III
            if (Term.Status == Model.Enums.Term.Status.Inactive)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            if
            (
                context.HttpContext.GetEndpoint().Metadata.GetMetadata<EndpointNameMetadata>().EndpointName.Equals("Home.Term.Purchase.Request")
            )
                context.HttpContext.GetRouteData().Values.Add("Term", Term);
        }
    }
}