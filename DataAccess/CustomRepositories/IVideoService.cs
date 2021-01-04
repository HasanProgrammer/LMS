using System.Threading.Tasks;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IVideoService <TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IVideoService <TViewModel, TModel>
    {
        
    }
    
    /*Model*/
    public partial interface IVideoService <TViewModel, TModel>
    {
        
    }
}