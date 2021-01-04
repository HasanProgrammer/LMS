using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebFramework.Filters.Controllers.AuthController
{
    public class LoginHandling : ActionFilterAttribute
    {
        private readonly UserManager<User>   _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly Config.StatusCode   _StatusCode;
        private readonly Config.Messages     _StatusMessage;
        
        public LoginHandling
        (
            UserManager<User>           UserManager  ,
            SignInManager<User>         SignInManager,
            IOptions<Config.StatusCode> StatusCode   ,
            IOptions<Config.Messages>   StatusMessage
        )
        {
            _UserManager   = UserManager;
            _SignInManager = SignInManager;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            LoginUserViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is LoginUserViewModel) as LoginUserViewModel;
            User user = await _UserManager.FindWithUsernameAsync(model.Username);
            if (user != null)
            {
                if (user.Status == Model.Enums.User.Status.Inactive)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.LockedUser);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.LockedUser, _StatusMessage.LockedUser, new {});
                    return;
                }
                
                SignInResult result = await _SignInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.UncorrectUsernameOrPassword);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.UncorrectUsernameOrPassword, _StatusMessage.UncorrectUsernameOrPassword, new {});
                    return;
                }

                if (!user.IsVerifyEmail && !user.IsVerifyPhone)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.UnVerifyEmailAndPhone);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.UnVerifyEmailAndPhone, _StatusMessage.UnVerifyEmailAndPhone, new {});
                    return;
                }
                
                context.HttpContext.GetRouteData().Values.Add("User", user);
            }
            else
            {
                JsonResponse.Handle(context.HttpContext, _StatusCode.UncorrectUsernameOrPassword);
                context.Result = new EmptyResult();
                context.Result = JsonResponse.Return(_StatusCode.UncorrectUsernameOrPassword, _StatusMessage.UncorrectUsernameOrPassword, new {});
                return;
            }
            
            await next();
        }
    }
}