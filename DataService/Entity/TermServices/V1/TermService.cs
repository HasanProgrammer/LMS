using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DataService.Entity.TermServices.V1
{
    /*Configs*/
    public partial class TermService : TermService<TermsViewModel, Term>
    {
        //Context
        private readonly DatabaseContext _Context;
        
        //Configs
        private readonly IConfiguration _Config;

        public TermService(IServiceProvider ServiceProvider, IConfiguration Config)
        {
            //Context
            _Context = ServiceProvider.GetRequiredService<DatabaseContext>();
            
            //Configs
            _Config = Config;
        }
    }
    
    /*ViewModel*/
    public partial class TermService
    {
        public override async Task<PaginatedList<TermsViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Terms.AsNoTracking().OrderByDescending(Term => Term.Id).Select(Term => new TermsViewModel
            {
                Id              = Term.Id,
                UserId          = Term.UserId,
                UserName        = Term.User.UserName,
                UserImage       = Term.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.User.Image.Path }" : null,
                Image           = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.Image.Path }",
                CategoryId      = Term.CategoryId,
                CategoryName    = Term.Category.Name,
                Name            = Term.Name,
                Description     = Term.Description,
                Suitable        = Term.Suitable,
                Result          = Term.Result,
                Price           = Term.Price,
                HasChapterKey   = Convert.ToInt32(Term.HasChapter),
                HasChapterValue = Term.HasChapter ? "دارد" : "ندارد",
                StatusKey       = Term.Status == Model.Enums.Term.Status.Active ? 1 : 0,
                StatusValue     = Term.Status == Model.Enums.Term.Status.Active ? "فعال" : "غیر فعال",
                DateCreate      = Term.CreatedAt,
                DateUpdate      = Term.UpdatedAt 
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<TermsViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            return await _Context.Terms.AsNoTracking().OrderByDescending(Term => Term.Id).Where(Term => Term.Status == Model.Enums.Term.Status.Active).Select(Term => new TermsViewModel
            {
                Id            = Term.Id,
                Image         = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.Image.Path }",
                CategoryName  = Term.Category.Name,
                Name          = Term.Name,
                Description   = Term.Description, 
                Price         = Term.Price,
                CountVideos   = Term.Videos.Count,
                CountStudent  = Term.Buys.Count
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<TermsViewModel> FindWithIdAsNoTrackingAsync(int id, User user)
        {
            //Eager's Loading
            var TermTarget = await _Context.Terms.Where(Term => Term.Id == id && Term.Status == Model.Enums.Term.Status.Active)
                                                 .Include(Term => Term.Image)
                                                 .Include(Term => Term.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Category)
                                                 .Include(Term => Term.Chapters)
                                                 .ThenInclude(Chapter => Chapter.Videos)
                                                 .Include(Term => Term.Videos)
                                                 .Include(Term => Term.Comments)
                                                 .ThenInclude(Comment => Comment.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Comments)
                                                 .ThenInclude(Comment => Comment.Answers)
                                                 .ThenInclude(Answer => Answer.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Buys)
                                                 .FirstOrDefaultAsync();
            

            bool BuyTerm = ( TermTarget.Buys.FirstOrDefault(Buy => Buy.UserId.Equals(user?.Id)) != null ? true : false );
            
            TermsViewModel model = new TermsViewModel
            {
                Id              = TermTarget.Id,
                UserId          = TermTarget.UserId,
                UserName        = TermTarget.User.UserName,
                UserExpert      = TermTarget.User.Expert,
                UserDescription = TermTarget.User.Description,
                UserImage       = TermTarget.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ TermTarget.User.Image.Path }" : null,
                Image           = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ TermTarget.Image.Path }",
                CategoryId      = TermTarget.CategoryId,
                CategoryName    = TermTarget.Category.Name,
                Name            = TermTarget.Name,
                Description     = TermTarget.Description,
                Suitable        = TermTarget.Suitable,
                Result          = TermTarget.Result,
                Price           = TermTarget.Price,
                HasChapterKey   = Convert.ToInt32(TermTarget.HasChapter),
                HasChapterValue = TermTarget.HasChapter ? "دارد" : "ندارد",
                StatusKey       = TermTarget.Status == Model.Enums.Term.Status.Active ? 1 : 0,
                StatusValue     = TermTarget.Status == Model.Enums.Term.Status.Active ? "فعال" : "غیر فعال",
                DateCreate      = TermTarget.CreatedAt,
                DateUpdate      = TermTarget.UpdatedAt,
                IsBuyed         = BuyTerm,
                Comments        = TermTarget.Comments.Where(Comment => Comment.Show).Select(Comment => new CommentsViewModel
                {
                    UserName  = Comment.User.UserName,
                    UserImage = Comment.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Comment.User.Image.Path }" : null,
                    Title     = Comment.Title,
                    Content   = Comment.Content,
                    Answers   = Comment.Answers.Where(Answer => Answer.Show).Select(Answer => new AnswersViewModel
                    {
                        UserName  = Answer.User.UserName,
                        UserImage = Answer.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Answer.User.Image.Path }" : null,
                        Content   = Answer.Content 
                    }).ToList()
                }).ToList()
            };

            if (TermTarget.HasChapter)
            {
                model.Chapters = TermTarget.Chapters.Select(Chapter => new ChaptersViewModel
                {
                    Title  = Chapter.Title,
                    Videos = Chapter.Videos?.Select(Video => new VideosViewModel
                    {
                        Id          = Video.Id,
                        Title       = Video.Title,
                        Duration    = Video.Duration,
                        IsFreeKey   = Video.IsFree ? 1 : 0,
                        IsFreeValue = Video.IsFree ? "رایگان" : "غیر رایگان",
                        Video       = Video.IsFree ? $"{ _Config.GetValue<string>("File:UploadPathVideoPublic").Replace("\\", "/") }{ Video.VideoFile }" : null
                    }).ToList()
                }).ToList();
            }
            else
            {
                model.Videos = TermTarget.Videos.Select(Video => new VideosViewModel
                {
                    Id          = Video.Id,
                    Title       = Video.Title,
                    Duration    = Video.Duration,
                    IsFreeKey   = Video.IsFree ? 1 : 0,
                    IsFreeValue = Video.IsFree ? "رایگان" : "غیر رایگان",
                    Video       = Video.IsFree ? $"{ _Config.GetValue<string>("File:UploadPathVideoPublic").Replace("\\", "/") }{ Video.VideoFile }" : null
                }).ToList();
            }

            return model;
        }
        
        public override async Task<TermsViewModel> FindWithIdAsNoTrackingAndActiveAsync(int id, User user)
        {
            var TermTarget = await _Context.Terms.Where(Term => Term.Id == id && Term.Status == Model.Enums.Term.Status.Active)
                                                 .AsNoTracking()
                                                 .Include(Term => Term.Image)
                                                 .Include(Term => Term.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Category)
                                                 .Include(Term => Term.Chapters)
                                                 .ThenInclude(Chapter => Chapter.Videos)
                                                 .Include(Term => Term.Videos)
                                                 .Include(Term => Term.Comments)
                                                 .ThenInclude(Comment => Comment.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Comments)
                                                 .ThenInclude(Comment => Comment.Answers)
                                                 .ThenInclude(Answer => Answer.User)
                                                 .ThenInclude(User => User.Image)
                                                 .Include(Term => Term.Buys)
                                                 .FirstOrDefaultAsync();


            if (TermTarget != null)
            {
                bool BuyTerm = ( TermTarget.Buys?.FirstOrDefault(Buy => Buy.UserId.Equals(user?.Id)) != null ? true : false );
            
                TermsViewModel model = new TermsViewModel
                {
                    Id              = TermTarget.Id,
                    UserId          = TermTarget.UserId,
                    UserName        = TermTarget.User.UserName,
                    UserExpert      = TermTarget.User.Expert, 
                    UserDescription = TermTarget.User.Description, 
                    UserImage       = TermTarget.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ TermTarget.User.Image.Path }" : null,
                    Image           = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ TermTarget.Image.Path }",
                    CategoryId      = TermTarget.CategoryId,
                    CategoryName    = TermTarget.Category.Name,
                    Name            = TermTarget.Name,
                    Description     = TermTarget.Description,
                    Suitable        = TermTarget.Suitable,
                    Result          = TermTarget.Result,
                    Price           = TermTarget.Price,
                    HasChapterKey   = Convert.ToInt32(TermTarget.HasChapter),
                    HasChapterValue = TermTarget.HasChapter ? "دارد" : "ندارد",
                    StatusKey       = TermTarget.Status == Model.Enums.Term.Status.Active ? 1 : 0,
                    StatusValue     = TermTarget.Status == Model.Enums.Term.Status.Active ? "فعال" : "غیر فعال",
                    DateCreate      = TermTarget.CreatedAt,
                    DateUpdate      = TermTarget.UpdatedAt,
                    IsBuyed         = BuyTerm,
                    Comments        = TermTarget.Comments.Where(Comment => Comment.Show).Select(Comment => new CommentsViewModel
                    {
                        UserName  = Comment.User.UserName,
                        UserImage = Comment.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Comment.User.Image.Path }" : null,
                        Title     = Comment.Title,
                        Content   = Comment.Content,
                        Answers   = Comment.Answers.Where(Answer => Answer.Show).Select(Answer => new AnswersViewModel
                        {
                            UserName  = Answer.User.UserName,
                            UserImage = Answer.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Answer.User.Image.Path }" : null,
                            Content   = Answer.Content 
                        }).ToList()
                    }).ToList()
                };
            
                if (TermTarget.HasChapter)
                {
                    model.Chapters = TermTarget.Chapters?.Select(Chapter => new ChaptersViewModel
                    {
                        Title  = Chapter.Title,
                        Videos = Chapter.Videos?.Where(Video => Video.Status == Model.Enums.Video.Status.Active).Select(Video => new VideosViewModel
                        {
                            Id          = Video.Id,
                            Title       = Video.Title,
                            Duration    = Video.Duration,
                            IsFreeKey   = Video.IsFree ? 1 : 0,
                            IsFreeValue = Video.IsFree ? "رایگان" : "غیر رایگان",
                            Video       = Video.IsFree ? $"{ _Config.GetValue<string>("File:UploadPathVideoPublic").Replace("\\", "/") }{ Video.VideoFile }" : null
                        }).ToList()
                    }).ToList();
                }
                else
                {
                    model.Videos = TermTarget.Videos?.Select(Video => new VideosViewModel
                    {
                        Id          = Video.Id, 
                        Title       = Video.Title,
                        Duration    = Video.Duration,
                        IsFreeKey   = Video.IsFree ? 1 : 0,
                        IsFreeValue = Video.IsFree ? "رایگان" : "غیر رایگان",
                        Video       = Video.IsFree ? $"{ _Config.GetValue<string>("File:UploadPathVideoPublic").Replace("\\", "/") }{ Video.VideoFile }" : null
                    }).ToList();
                }
            
                return model;
            }

            return null;
        }

        public override async Task<PaginatedList<TermsViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            return await _Context.Terms.AsNoTracking().OrderByDescending(Term => Term.Id).Where(Term => Term.UserId.Equals(user.Id)).Select(Term => new TermsViewModel
            {
                Id              = Term.Id,
                UserId          = Term.UserId,
                UserName        = Term.User.UserName,
                UserImage       = Term.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.User.Image.Path }" : null,
                Image           = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.Image.Path }",
                CategoryId      = Term.CategoryId,
                CategoryName    = Term.Category.Name,
                Name            = Term.Name,
                Description     = Term.Description,
                Suitable        = Term.Suitable,
                Result          = Term.Result,
                Price           = Term.Price,
                HasChapterKey   = Convert.ToInt32(Term.HasChapter),
                HasChapterValue = Term.HasChapter ? "دارد" : "ندارد",
                StatusKey       = Term.Status == Model.Enums.Term.Status.Active ? 1 : 0,
                StatusValue     = Term.Status == Model.Enums.Term.Status.Active ? "فعال" : "غیر فعال",
                DateCreate      = Term.CreatedAt,
                DateUpdate      = Term.UpdatedAt 
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<TermsViewModel>> FindAllWithNoTrackingAndActiveForCategoryAndPaginateAsync(string category, int page, int count)
        {
            return await _Context.Terms.AsNoTracking().OrderByDescending(Term => Term.Id).Where(Term => Term.Status == Model.Enums.Term.Status.Active).Where(Term => Term.Category.Name.Equals(category)).Select(Term => new TermsViewModel
            {
                Id            = Term.Id,
                Image         = $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Term.Image.Path }",
                CategoryName  = Term.Category.Name,
                Name          = Term.Name,
                Description   = Term.Description, 
                Price         = Term.Price,
                CountVideos   = Term.Videos.Count,
                CountStudent  = Term.Buys.Count
            }).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class TermService
    {
        public override Term FindWithIdEntityAsNoTracking(int? id)
        {
            return _Context.Terms.AsNoTracking().FirstOrDefault(Term => Term.Id == id);
        }

        public override Term FindWithIdEntityWithEagerLoading(int? id)
        {
            return _Context.Terms.Include(Term => Term.Image).FirstOrDefault(Term => Term.Id == id);
        }

        public override Term FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            return _Context.Terms.AsNoTracking().Include(Term => Term.Image)
                                                .Include(Term => Term.User)
                                                .Include(Term => Term.Chapters)
                                                .FirstOrDefault(Term => Term.Id == id);
        }

        public override Task<List<Term>> FindAllActiveEntityAsNoTrackingAsync()
        {
            return _Context.Terms.AsNoTracking().Where(Term => Term.Status == Model.Enums.Term.Status.Active).ToListAsync();
        }
        
        public override Task<List<Term>> FindAllEntityForUserWithNoTrackingAndActiveAsync(User user)
        {
            return _Context.Terms.AsNoTracking().Where(Term => Term.UserId.Equals(user.Id) && Term.Status == Model.Enums.Term.Status.Active).ToListAsync();
        }

        public override Term FindWithNameEntityWithNoTracking(string name)
        {
            return _Context.Terms.AsNoTracking().FirstOrDefault(Term => Term.Name.Equals(name));
        }

        public override async Task<(int, int, int, int, int, int, int, int, int, int)> FindCountEntityForCategoryWithNoTrackingAsync(string csh, string php, string python, string js, string asp, string laravel, string django, string reactjs, string sql, string mysql)
        {
            IQueryable<Term> TermQuery = _Context.Terms.AsNoTracking();
            
            int CountCSH       = await TermQuery.Where(Term => Term.Category.Name.Equals(csh)     && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountPHP       = await TermQuery.Where(Term => Term.Category.Name.Equals(php)     && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountPYT       = await TermQuery.Where(Term => Term.Category.Name.Equals(python)  && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountJS        = await TermQuery.Where(Term => Term.Category.Name.Equals(js)      && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountASP       = await TermQuery.Where(Term => Term.Category.Name.Equals(asp)     && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountLaravel   = await TermQuery.Where(Term => Term.Category.Name.Equals(laravel) && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountDjango    = await TermQuery.Where(Term => Term.Category.Name.Equals(django)  && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountReactJS   = await TermQuery.Where(Term => Term.Category.Name.Equals(reactjs) && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountSQLServer = await TermQuery.Where(Term => Term.Category.Name.Equals(sql)     && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            int CountMySQL     = await TermQuery.Where(Term => Term.Category.Name.Equals(mysql)   && Term.Status == Model.Enums.Term.Status.Active).CountAsync();
            
            return (CountCSH, CountPHP, CountPYT, CountJS, CountASP, CountLaravel, CountDjango, CountReactJS, CountSQLServer, CountMySQL);
        }
    }
}