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

namespace DataService.Entity.QuestionServices.V1
{
    /*Configs*/
    public partial class QuestionService : QuestionService<CommentsViewModel, Comment>
    {
        //Context
        private readonly DatabaseContext _Context;
        
        //Config
        private readonly IConfiguration _Config;
        
        public QuestionService(DatabaseContext Context, IConfiguration Config)
        {
            //Context
            _Context = Context;
            
            //Config
            _Config = Config;
        }
    }

    /*ViewModel*/
    public partial class QuestionService
    {
        public override async Task<PaginatedList<CommentsViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Comments.AsNoTracking().Select(Comment => new CommentsViewModel
            {
                UserName    = Comment.User.UserName,
                UserImage   = Comment.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Comment.User.Image.Path }" : null,
                TermName    = Comment.Term.Name,
                Id          = Comment.Id,
                Title       = Comment.Title,
                Content     = Comment.Content,
                CountAnswer = Comment.Answers.Count,
                Status      = Comment.Show,
                DateCreate  = Comment.CreatedAt 
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<CommentsViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            return await _Context.Comments.AsNoTracking().Where(Comment => Comment.UserId.Equals(user.Id)).Select(Comment => new CommentsViewModel
            {
                UserName    = Comment.User.UserName,
                UserImage   = Comment.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Comment.User.Image.Path }" : null,
                TermName    = Comment.Term.Name,
                Title       = Comment.Title,
                Id          = Comment.Id,
                Content     = Comment.Content,
                CountAnswer = Comment.Answers.Count,
                Status      = Comment.Show,
                DateCreate  = Comment.CreatedAt 
            }).ToPaginatedListAsync(count, page);
        }
    }

    /*Model*/
    public partial class QuestionService
    {
        public override Comment FindWithIdEntity(int? id)
        {
            return _Context.Comments.SingleOrDefault(Comment => Comment.Id == id);
        }

        public override Comment FindWithIdEntityWithEagerLoading(int? id)
        {
            return _Context.Comments.Include(Comment => Comment.User).Include(Comment => Comment.Term).SingleOrDefault(Comment => Comment.Id == id);
        }
    }
}