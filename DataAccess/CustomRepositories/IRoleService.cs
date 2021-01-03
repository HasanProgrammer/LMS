using System.Threading.Tasks;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IRoleService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IRoleService<TViewModel, TModel>
    {
        
    }
    
    /*Model*/
    public partial interface IRoleService<TViewModel, TModel>
    {
        public TModel FindWithNameEntityWithNoTracking(string name);
        public Task<TModel> FindWithNameEntityWithNoTrackingAsync(string name);
    }
}