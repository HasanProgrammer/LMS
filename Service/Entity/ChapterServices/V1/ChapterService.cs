using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Model;

namespace Service.Entity.ChapterServices.V1
{
    /*Configs*/
    public partial class ChapterService : IChapterService<ChaptersViewModel, Chapter>
    {
        private readonly DatabaseContext _Context;
        
        public ChapterService(DatabaseContext Context)
        {
            _Context = Context;
        }
    }

    /*ViewModel*/
    public partial class ChapterService
    {
        public IEnumerable<ChaptersViewModel> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ChaptersViewModel> FindAllWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ChaptersViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<ChaptersViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<ChaptersViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public ChaptersViewModel FindWithId(int? id)
        {
            throw new System.NotImplementedException();
        }

        public ChaptersViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChaptersViewModel> FindWithIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChaptersViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ChaptersViewModel> FindAllActive()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ChaptersViewModel> FindAllActiveAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<ChaptersViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<ChaptersViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ChaptersViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<ChaptersViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public ChaptersViewModel FindWithIdActive(int? id)
        {
            throw new System.NotImplementedException();
        }

        public ChaptersViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChaptersViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChaptersViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public ChaptersViewModel ConvertToViewModel(Chapter model)
        {
            throw new System.NotImplementedException();
        }
    }

    /*Models*/
    public partial class ChapterService
    {
        public IEnumerable<Chapter> FindAllEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Chapter> FindAllEntityWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Chapter> FindAllEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Chapter> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Chapter>> FindAllEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Chapter>> FindAllEntityWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Chapter>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Chapter>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Chapter> FindAllActiveEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Chapter> FindAllActiveEntityAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Chapter> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Chapter> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Chapter>> FindAllActiveEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Chapter>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Chapter>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Chapter>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdActiveEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Chapter FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdActiveEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Chapter> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }
    }
}