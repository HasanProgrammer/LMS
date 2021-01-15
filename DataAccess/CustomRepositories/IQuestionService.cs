using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IQuestionService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IQuestionService<TViewModel, TModel>
    {
        public Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count);
    }
    
    /*Model*/
    public partial interface IQuestionService<TViewModel, TModel>
    {
        
    }
}