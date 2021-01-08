using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;

namespace WebFramework.Filters.Controllers.ChapterController
{
    public class CheckTerm : ActionFilterAttribute
    {
        //DataService
        private readonly TermService<TermsViewModel, Term> _TermService;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public CheckTerm 
        (
            UserManager<User>                  UserManager,
            IOptions<Config.StatusCode>        StatusCode, 
            IOptions<Config.Messages>          StatusMessage,
            TermService<TermsViewModel, Term>  TermService
        )
        {
            //DataService
            _TermService = TermService;
            
            //Managers
            _UserManager = UserManager;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //I
            Term Term = _TermService.FindWithIdEntityAsNoTracking (
                (context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is CreateChapterViewModel) as CreateChapterViewModel).Term
            );
            
            //II
            if (Term == null)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.NotFound);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.NotFound, _StatusMessage.NotFound, new {});
                return;
            }
            
            //III
            context.HttpContext.GetRouteData().Values.Add("Term", Term);
        }
    }
}