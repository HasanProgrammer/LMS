using System;
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

namespace WebFramework.Filters.Controllers.AuthController
{
    public class RegisterHandling : ActionFilterAttribute
    {
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;

        public RegisterHandling
        (
            UserManager<User>           UserManager, 
            RoleManager<Role>           RoleManager, 
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage
        )
        {
            _UserManager   = UserManager;
            _RoleManager   = RoleManager;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //I
            RegisterUserViewModel model = context.ActionArguments.Values.FirstOrDefault(Parameter => Parameter is RegisterUserViewModel) as RegisterUserViewModel;

            //II
            int PhoneCode    = DigitCode.Generate8();
            string EmailCode = $"{model.Username}-{Guid.NewGuid().ToString()}";
            
            //III
            User user = new User
            {
                Id                 = Guid.NewGuid().ToString(),
                UserName           = model.Username,
                Email              = model.Email,
                Phone              = model.Phone,
                EmailCode          = EmailCode,
                PhoneCode          = PhoneCode,
                Status             = Model.Enums.User.Status.Active,
                Description        = "غیر ضروری",
                Expert             = "غیر ضروری", 
                CreatedAtTimeStamp = Time.TimeStampNow(),
                CreatedAt          = PersianDatetime.Now(),
                UpdatedAt          = PersianDatetime.Now()
            };
            
            //IV
            context.HttpContext.GetRouteData().Values.Add("User"     , user);
            context.HttpContext.GetRouteData().Values.Add("EmailCode", EmailCode);
            context.HttpContext.GetRouteData().Values.Add("PhoneCode", PhoneCode);

            //V
            await next();
        }
    }
}