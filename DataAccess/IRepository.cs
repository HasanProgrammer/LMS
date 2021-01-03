using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model.Bases;

namespace DataAccess
{
    /*Config*/
    public partial interface IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IRepository<TViewModel, TModel>
    {
        public IEnumerable<TViewModel> FindAll();
        public IEnumerable<TViewModel> FindAllWithNoTracking();
        public IEnumerable<TViewModel> FindAllWithPaginate(int page, int count);
        public PaginatedList<TViewModel> FindAllWithNoTrackingAndPaginate(int page, int count);
        public Task<List<TViewModel>> FindAllAsync();
        public Task<List<TViewModel>> FindAllWithPaginateAsync(int page, int count);
        public Task<List<TViewModel>> FindAllWithNoTrackingAsync();
        public Task<PaginatedList<TViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count);
        public TViewModel FindWithId(int? id);
        public TViewModel FindWithIdAsNoTracking(int? id);
        public Task<TViewModel> FindWithIdAsync(int? id);
        public Task<TViewModel> FindWithIdAsNoTrackingAsync(int? id);
        public IEnumerable<TViewModel> FindAllActive();
        public IEnumerable<TViewModel> FindAllActiveAsNoTracking();
        public PaginatedList<TViewModel> FindAllActiveWithPaginate(int page, int count);
        public PaginatedList<TViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count);
        public Task<List<TViewModel>> FindAllActiveAsync();
        public Task<List<TViewModel>> FindAllActiveAsNoTrackingAsync();
        public Task<List<TViewModel>> FindAllActiveWithPaginateAsync(int page, int count);
        public Task<PaginatedList<TViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count);
        public TViewModel FindWithIdActive(int? id);
        public TViewModel FindWithIdActiveAsNoTracking(int? id);
        public Task<TViewModel> FindWithIdActiveAsync(int? id);
        public Task<TViewModel> FindWithIdActiveAsNoTrackingAsync(int? id);
        public TViewModel ConvertToViewModel(TModel model);
    }
    
    /*Model*/
    public partial interface IRepository<TViewModel, TModel>
    {
        public IEnumerable<TModel> FindAllEntity();
        public IEnumerable<TModel> FindAllEntityWithNoTracking();
        public PaginatedList<TModel> FindAllEntityWithPaginate(int page, int count);
        public PaginatedList<TModel> FindAllEntityWithNoTrackingAndPaginate(int page, int count);
        public Task<List<TModel>> FindAllEntityAsync();
        public Task<List<TModel>> FindAllEntityWithNoTrackingAsync();
        public Task<PaginatedList<TModel>> FindAllEntityWithPaginateAsync(int page, int count);
        public Task<PaginatedList<TModel>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count);
        public TModel FindWithIdEntity(int? id);
        public TModel FindWithIdEntityAsNoTracking(int? id);
        public TModel FindWithIdEntityWithEagerLoading(int? id);
        public TModel FindWithIdEntityWithEagerLoadingAsNoTracking(int? id);
        public Task<TModel> FindWithIdEntityAsync(int? id);
        public Task<TModel> FindWithIdEntityAsNoTrackingAsync(int? id);
        public Task<TModel> FindWithIdEntityWithEagerLoadingAsync(int? id);
        public Task<TModel> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id);
        public IEnumerable<TModel> FindAllActiveEntity();
        public IEnumerable<TModel> FindAllActiveEntityAsNoTracking();
        public PaginatedList<TModel> FindAllActiveEntityWithPaginate(int page, int count);
        public PaginatedList<TModel> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count);
        public Task<List<TModel>> FindAllActiveEntityAsync();
        public Task<List<TModel>> FindAllActiveEntityAsNoTrackingAsync();
        public Task<PaginatedList<TModel>> FindAllActiveEntityWithPaginateAsync(int page, int count);
        public Task<PaginatedList<TModel>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count);
        public TModel FindWithIdActiveEntity(int? id);
        public TModel FindWithIdActiveEntityAsNoTracking(int? id);
        public TModel FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id);
        public Task<TModel> FindWithIdActiveEntityAsync(int? id);
        public Task<TModel> FindWithIdActiveEntityAsNoTrackingAsync(int? id);
        public Task<TModel> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id);
    }
}