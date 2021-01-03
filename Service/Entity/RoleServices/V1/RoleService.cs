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

namespace Service.Entity.RoleServices.V1
{
    /*Configs*/
    public partial class RoleService : IRoleService<RolesViewModel, Role>
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
        public IEnumerable<RolesViewModel> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RolesViewModel> FindAllWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RolesViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<RolesViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PaginatedList<RolesViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
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

        public RolesViewModel FindWithId(int? id)
        {
            throw new System.NotImplementedException();
        }

        public RolesViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RolesViewModel> FindWithIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RolesViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RolesViewModel> FindAllActive()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RolesViewModel> FindAllActiveAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<RolesViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<RolesViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RolesViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<RolesViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public RolesViewModel FindWithIdActive(int? id)
        {
            throw new System.NotImplementedException();
        }

        public RolesViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RolesViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RolesViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public RolesViewModel ConvertToViewModel(Role model)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithNameEntityWithNoTracking(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithNameEntityWithNoTrackingAsync(string name)
        {
            throw new System.NotImplementedException();
        }
    }
    
    /*Model*/
    public partial class RoleService
    {
        public IEnumerable<Role> FindAllEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> FindAllEntityWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Role> FindAllEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Role> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Role>> FindAllEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Role>> FindAllEntityWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Role>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Role>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> FindAllActiveEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> FindAllActiveEntityAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Role> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Role> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Role>> FindAllActiveEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Role>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Role>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Role>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdActiveEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Role FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdActiveEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }
    }
}