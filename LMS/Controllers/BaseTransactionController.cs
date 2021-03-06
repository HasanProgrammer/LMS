﻿using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route(template: Config.Routing.BaseRoute + "transaction")]
    [Authorize(Roles = "Admin, Teacher", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseTransactionController : BaseController
    {
        
    }
}