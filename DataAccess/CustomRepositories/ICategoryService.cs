using System.Threading.Tasks;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface ICategoryService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface ICategoryService<TViewModel, TModel>
    {
        
    }
    
    /*Model*/
    public partial interface ICategoryService<TViewModel, TModel>
    {
        public TModel FindWithNameEntityWithNoTracking(string name);
        public Task<TModel> FindWithNameEntityWithNoTrackingAsync(string name);
    }
}