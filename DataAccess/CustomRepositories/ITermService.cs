using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface ITermService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface ITermService<TViewModel, TModel>
    {
        public Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count);
    }
    
    /*Model*/
    public partial interface ITermService<TViewModel, TModel>
    {
        public TModel FindWithNameEntityWithNoTracking(string name);
        public Task<TModel> FindWithNameEntityWithNoTrackingAsync(string name);
    }
}