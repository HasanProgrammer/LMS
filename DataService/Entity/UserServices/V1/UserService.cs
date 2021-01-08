using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataService.Entity.UserServices.V1
{
    /*Configs*/
    public partial class UserService : UserService<UsersViewModel, User>
    {
        //Managers
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        
        public UserService
        (
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager
        )
        {
            
            //Managers
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }
    }
    
    /*ViewModel*/
    public partial class UserService
    {
        public override async Task<PaginatedList<UsersViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _UserManager.Users.Include(User => User.UserRoles)
                                           .ThenInclude(UR => UR.Role)
                                           .AsNoTracking()
                                           .Select(User => new UsersViewModel
                                           {
                                               Id          = User.Id,
                                               UserName    = User.UserName,
                                               Email       = User.Email,
                                               Phone       = User.Phone,
                                               StatusKey   = User.Status == Model.Enums.User.Status.Active ? 1 : 0,
                                               StatusValue = User.Status == Model.Enums.User.Status.Active ? "فعال" : "غیر فعال",
                                                   
                                               Roles = User.UserRoles.Select(UR => new RolesViewModel
                                               {
                                                   RoleName = UR.Role.Name
                                               }).ToList()
                                           }).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class UserService
    {
        
    }
}