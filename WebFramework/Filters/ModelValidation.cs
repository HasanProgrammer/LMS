using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebFramework.Extensions;

namespace WebFramework.Filters
{
    public class ModelValidation : ActionFilterAttribute
    {
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        public ModelValidation(IOptions<Config.StatusCode> StatusCode, IOptions<Config.Messages> StatusMessage)
        {
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.ModelValidation);
                context.Result = new EmptyResult();
                context.Result = context.ModelState.Values.ErrorValidation(_StatusCode.ModelValidation, _StatusMessage.ModelValidation);
            }
        }
    }
}