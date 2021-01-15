using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*ViewModel*/
    public abstract partial class ChapterService<TViewModel, TModel> : IChapterService<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        public virtual IEnumerable<TViewModel> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TViewModel> FindAllWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual TViewModel FindWithId(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TViewModel> FindWithIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TViewModel> FindAllActive()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TViewModel> FindAllActiveAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual TViewModel FindWithIdActive(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TViewModel ConvertToViewModel(TModel model)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithTitleEntityWithNoTracking(string title)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithTitleEntityWithNoTrackingAsync(string title)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            throw new System.NotImplementedException();
        }
    }
    
    /*Model*/
    public abstract partial class ChapterService<TViewModel, TModel>
    {
        public virtual IEnumerable<TModel> FindAllEntity()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TModel> FindAllEntityWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TModel> FindAllEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TModel> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllEntityWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TModel>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TModel>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TModel> FindAllActiveEntity()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TModel> FindAllActiveEntityAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TModel> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual PaginatedList<TModel> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllActiveEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TModel>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TModel>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdActiveEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdActiveEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllEntityForTermWithNoTrackingAsync(int term)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllEntityForUserWithNoTrackingAndActiveAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TModel>> FindAllEntityForUserAndTermWithNoTrackingAsync(User user, int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<TModel> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }
    }
}