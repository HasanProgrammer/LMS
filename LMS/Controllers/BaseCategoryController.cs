using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route(template: Config.Routing.BaseRoute + "category")]
    [Authorize(Roles = "Admin, Teacher", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseCategoryController : BaseController
    {
        
    }
}