using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace Service.Entity.UserServices.V2
{
    /*Configs*/
    public partial class UserService : IUserService<UsersViewModel, User>
    {
        //Config
        private readonly IConfiguration _Config;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        
        public UserService
        (
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            IConfiguration    Config
        )
        {
            //Config
            _Config = Config;
            
            //Managers
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }
    }
    
    /*ViewModel*/
    public partial class UserService
    {
        public IEnumerable<UsersViewModel> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UsersViewModel> FindAllWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UsersViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<UsersViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PaginatedList<UsersViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _UserManager.Users.Include(User => User.UserRoles)
                                           .ThenInclude(UR => UR.Role)
                                           .AsNoTracking()
                                           .Select(User => new UsersViewModel
                                           {
                                               Id       = User.Id,
                                               UserName = User.UserName,
                                               Email    = User.Email,
                                               Image    = User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPath").Replace("\\", "/") }{ User.Image.Path }" : null,
                                               Roles    = User.UserRoles.Select(UR => new RolesViewModel
                                               {
                                                   RoleId   = UR.Role.Id, 
                                                   RoleName = UR.Role.Name
                                               }).ToList()
                                           }).ToPaginatedListAsync(count, page);
        }

        public UsersViewModel FindWithId(int? id)
        {
            throw new System.NotImplementedException();
        }

        public UsersViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsersViewModel> FindWithIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsersViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UsersViewModel> FindAllActive()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UsersViewModel> FindAllActiveAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<UsersViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<UsersViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UsersViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<UsersViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public UsersViewModel FindWithIdActive(int? id)
        {
            throw new System.NotImplementedException();
        }

        public UsersViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsersViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsersViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public UsersViewModel ConvertToViewModel(User model)
        {
            throw new System.NotImplementedException();
        }
    }
    
    /*Model*/
    public partial class UserService
    {
        public IEnumerable<User> FindAllEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> FindAllEntityWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<User> FindAllEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<User> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> FindAllEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> FindAllEntityWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<User>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<User>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> FindAllActiveEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> FindAllActiveEntityAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<User> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<User> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> FindAllActiveEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<User>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<User>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdActiveEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdActiveEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public User FindWithIdEntityWithEagerLoading(string id)
        {
            return _UserManager.Users.Include(User => User.Image).FirstOrDefault(User => User.Id.Equals(id));
        }
    }
}