using System.Threading.Tasks;
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
        
    }
    
    /*Model*/
    public partial interface IChapterService<TViewModel, TModel>
    {
        
    }
}