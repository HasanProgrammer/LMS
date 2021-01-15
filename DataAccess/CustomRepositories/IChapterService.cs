using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IChapterService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IChapterService<TViewModel, TModel>
    {
        public Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count);
    }
    
    /*Model*/
    public partial interface IChapterService<TViewModel, TModel>
    {
        public Task<List<TModel>> FindAllEntityForTermWithNoTrackingAsync(int id);
        public Task<List<TModel>> FindAllEntityForUserWithNoTrackingAndActiveAsync(User user);
        public Task<List<TModel>> FindAllEntityForUserAndTermWithNoTrackingAsync(User user, int id);
        public TModel FindWithTitleEntityWithNoTracking(string title);
        public Task<TModel> FindWithTitleEntityWithNoTrackingAsync(string title);
    }
}