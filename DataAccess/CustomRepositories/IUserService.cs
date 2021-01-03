using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IUserService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IUserService<TViewModel, TModel>
    {
        
    }
    
    /*Model*/
    public partial interface IUserService<TViewModel, TModel>
    {
        public TModel FindWithIdEntityWithEagerLoading(string id);
    }
}