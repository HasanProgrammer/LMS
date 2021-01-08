using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataService.Entity.RoleServices.V1
{
    /*Configs*/
    public partial class RoleService : RoleService<RolesViewModel, Role>
    {
        private readonly RoleManager<Role> _RoleManager;
        private readonly DatabaseContext   _Context;

        public RoleService(RoleManager<Role> RoleManager, DatabaseContext context)
        {
            _Context     = context;
            _RoleManager = RoleManager;
        }
    }

    /*ViewModel*/
    public partial class RoleService
    {
        public override async Task<PaginatedList<RolesViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await (
                           from role in _RoleManager.Roles.OrderByDescending(role => role.Id).Include(Role => Role.UserRoles).Include(Role => Role.User)
                           select new RolesViewModel
                           {
                               RoleId    = role.Id,
                               RoleName  = role.Name,
                               CountUser = role.UserRoles.Count
                           }
                         ).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class RoleService
    {
        
    }
}