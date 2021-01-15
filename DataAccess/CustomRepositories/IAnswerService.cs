using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface IAnswerService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface IAnswerService<TViewModel, TModel>
    {
        public Task<PaginatedList<TViewModel>> FindAllForQuestionWithNoTrackingAndPaginateAsync(int id, int page, int count);
        public Task<PaginatedList<TViewModel>> FindAllForUserAndQuestionWithNoTrackingAndPaginateAsync(User user, int id, int page, int count);
    }
    
    /*Model*/
    public partial interface IAnswerService<TViewModel, TModel>
    {
        
    }
}