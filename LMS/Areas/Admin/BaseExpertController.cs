using System.Net.Http;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace LMS.Areas.Admin
{
    [Area(areaName: "Admin")]
    [Route(template: Config.Routing.BaseRoute + "admin/expert")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseExpertController : BaseController
    {
        
    }
}