using System.Collections.Generic;
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

namespace DataService.Entity.ChapterServices.V1
{
    /*Configs*/
    public partial class ChapterService : ChapterService<ChaptersViewModel, Chapter>
    {
        //Context
        private readonly DatabaseContext _Context;
        
        //Configs
        private readonly IConfiguration _Config;
        
        public ChapterService(DatabaseContext Context, IConfiguration Config)
        {
            //Context
            _Context = Context;
            
            //Configs
            _Config = Config;
        }
    }

    /*ViewModel*/
    public partial class ChapterService
    {
        public override async Task<PaginatedList<ChaptersViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Chapters.AsNoTracking().OrderByDescending(Chapter => Chapter.Id).Select(Chapter => new ChaptersViewModel
            {
                Id         = Chapter.Id,
                UserId     = Chapter.UserId,
                UserName   = Chapter.User.UserName,
                UserImage  = Chapter.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Chapter.User.Image.Path }" : null,
                TermId     = Chapter.TermId,
                TermName   = Chapter.Term != null ? Chapter.Term.Name : null,
                TermImage  = Chapter.Term != null && Chapter.Term.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Chapter.Term.Image.Path }" : null,
                Title      = Chapter.Title,
                DateCreate = Chapter.CreatedAt,
                DateUpdate = Chapter.UpdatedAt
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<ChaptersViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            return await _Context.Chapters.AsNoTracking().OrderByDescending(Chapter => Chapter.Id).Where(Chapter => Chapter.UserId.Equals(user.Id)).Select(Chapter => new ChaptersViewModel
            {
                Id         = Chapter.Id,
                UserId     = Chapter.UserId,
                UserName   = Chapter.User.UserName,
                UserImage  = Chapter.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Chapter.User.Image.Path }" : null,
                TermId     = Chapter.TermId,
                TermName   = Chapter.Term != null ? Chapter.Term.Name : null,
                TermImage  = Chapter.Term != null && Chapter.Term.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Chapter.Term.Image.Path }" : null,
                Title      = Chapter.Title,
                DateCreate = Chapter.CreatedAt,
                DateUpdate = Chapter.UpdatedAt
            }).ToPaginatedListAsync(count, page);
        }
    }

    /*Model*/
    public partial class ChapterService
    {
        public override Chapter FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            return _Context.Chapters.AsNoTracking().Include(Chapter => Chapter.Videos).SingleOrDefault(Chapter => Chapter.Id == id);
        }
        
        public override async Task<List<Chapter>> FindAllEntityForTermWithNoTrackingAsync(int id)
        {
            return await _Context.Chapters.AsNoTracking().Where(Chapter => Chapter.TermId == id).ToListAsync();
        }
        
        public override async Task<List<Chapter>> FindAllEntityForUserAndTermWithNoTrackingAsync(User user, int id)
        {
            return await _Context.Chapters.AsNoTracking().Where(Chapter => Chapter.UserId.Equals(user.Id) && Chapter.TermId == id).ToListAsync();
        }
        
        public override Chapter FindWithIdEntity(int? id)
        {
            return _Context.Chapters.FirstOrDefault(Chapter => Chapter.Id == id);
        }

        public override Chapter FindWithIdEntityAsNoTracking(int? id)
        {
            return _Context.Chapters.AsNoTracking().FirstOrDefault(Chapter => Chapter.Id == id);
        }

        public override Chapter FindWithTitleEntityWithNoTracking(string title)
        {
            return _Context.Chapters.AsNoTracking().FirstOrDefault(Chapter => Chapter.Title.Equals(title));
        }
    }
}