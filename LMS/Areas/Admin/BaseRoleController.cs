using Common;
using LMS.Areas.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin
{
    [Area(areaName: "Admin")]
    [Route(template: Config.Routing.BaseRoute + "admin/role")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseRoleController : BaseController
    {
        
    }
}