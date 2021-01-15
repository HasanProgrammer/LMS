using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace DataService.Entity.AnswerServices.V1
{
    /*Configs*/
    public partial class AnswerService : AnswerService<AnswersViewModel, Answer>
    {
        //Context
        private readonly DatabaseContext _Context;
        
        //Configs
        private readonly IConfiguration _Config;

        public AnswerService(DatabaseContext Context, IConfiguration Config)
        {
            //Context
            _Context = Context;
            
            //Configs
            _Config = Config;
        }
    }
    
    /*ViewModel*/
    public partial class AnswerService
    {
        public override async Task<PaginatedList<AnswersViewModel>> FindAllForQuestionWithNoTrackingAndPaginateAsync(int id, int page, int count)
        {
            return await _Context.Answers.AsNoTracking().Where(Answer => Answer.CommentId == id).Select(Answer => new AnswersViewModel
            {
                UserName  = Answer.User.UserName,
                UserImage = Answer.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Answer.User.Image.Path }" : null,
                Id        = Answer.Id,
                Content   = Answer.Content,
                Status    = Answer.Show
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<AnswersViewModel>> FindAllForUserAndQuestionWithNoTrackingAndPaginateAsync(User user, int id, int page, int count)
        {
            return await _Context.Answers.AsNoTracking().Where(Answer => Answer.CommentId == id && Answer.UserId.Equals(user.Id)).Select(Answer => new AnswersViewModel
            {
                UserName  = Answer.User.UserName,
                UserImage = Answer.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Answer.User.Image.Path }" : null,
                Id        = Answer.Id,
                Content   = Answer.Content,
                Status    = Answer.Show
            }).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class AnswerService
    {
        public override Answer FindWithIdEntity(int? id)
        {
            return _Context.Answers.SingleOrDefault(Answer => Answer.Id == id);
        }
    }
}