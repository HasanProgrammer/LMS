using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IVideoService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IVideoService <TViewModel, TModel>
    {
        public Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count);
    }
    
    /*Model*/
    public partial interface IVideoService <TViewModel, TModel>
    {
        
    }
}