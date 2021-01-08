using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Model;

namespace DataService.Entity.VideoServices.V1
{
    /*Configs*/
    public partial class VideoService : VideoService<VideosViewModel, Video>
    {
        //Context
        private readonly DatabaseContext _Context;

        //Configs
        private readonly Config.File _Config;
        
        //Environment
        private readonly IWebHostEnvironment _Env;
        
        public VideoService(DatabaseContext Context, IOptions<Config.File> Config, IWebHostEnvironment Env)
        {
            //Context
            _Context = Context;
            
            //Configs
            _Config = Config.Value;
            
            //Environment
            _Env = Env;
        }
    }

    /*ViewModel*/
    public partial class VideoService
    {
        public override async Task<PaginatedList<VideosViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Videos.AsNoTracking().Select(Video => new VideosViewModel
            {
                Id           = Video.Id,
                UserId       = Video.UserId,
                UserName     = Video.User.UserName,
                UserImage    = Video.User.Image != null ? $"{ _Config.UploadPathImagePublic.Replace("\\", "/") }{ Video.User.Image.Path }" : null,
                ChapterId    = Video.ChapterId,
                ChapterTitle = Video.Chapter != null ? Video.Chapter.Title : null,
                TermId       = Video.TermId,
                TermName     = Video.Term.Name,
                Title        = Video.Title,
                Duration     = Video.Duration,
                Video        = Video.IsFree ? $"{ _Config.UploadPathVideoPublic }{ Video.VideoFile }" : $"{ _Env.ContentRootPath + "\\" + _Config.UploadPathVideoPrivate }{ Video.VideoFile }",
                IsFreeKey    = Video.IsFree ? 1 : 0, 
                IsFreeValue  = Video.IsFree ? "رایگان" : "غیر رایگان",
                StatusKey    = Video.Status == Model.Enums.Video.Status.Active ? 1 : 0,
                StatusValue  = Video.Status == Model.Enums.Video.Status.Active ? "فعال" : "غیر فعال",
                DateCreate   = Video.CreatedAt,
                DateUpdate   = Video.UpdatedAt 
            }).ToPaginatedListAsync(count, page);
        }

        public override async Task<PaginatedList<VideosViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            return await _Context.Videos.AsNoTracking().Where(Video => Video.UserId.Equals(user.Id)).Select(Video => new VideosViewModel
            {
                Id           = Video.Id,
                UserId       = Video.UserId,
                UserName     = Video.User.UserName,
                UserImage    = Video.User.Image != null ? $"{ _Config.UploadPathImagePublic.Replace("\\", "/") }{ Video.User.Image.Path }" : null,
                ChapterId    = Video.ChapterId,
                ChapterTitle = Video.Chapter != null ? Video.Chapter.Title : null,
                TermId       = Video.TermId,
                TermName     = Video.Term.Name,
                Title        = Video.Title,
                Duration     = Video.Duration,
                Video        = Video.IsFree ? $"{ _Config.UploadPathVideoPublic }{ Video.VideoFile }" : $"{ _Env.ContentRootPath + "\\" + _Config.UploadPathVideoPrivate }{ Video.VideoFile }",
                IsFreeKey    = Video.IsFree ? 1 : 0, 
                IsFreeValue  = Video.IsFree ? "رایگان" : "غیر رایگان",
                StatusKey    = Video.Status == Model.Enums.Video.Status.Active ? 1 : 0,
                StatusValue  = Video.Status == Model.Enums.Video.Status.Active ? "فعال" : "غیر فعال",
                DateCreate   = Video.CreatedAt,
                DateUpdate   = Video.UpdatedAt 
            }).ToPaginatedListAsync(count, page);
        }
    }

    /*Model*/
    public partial class VideoService
    {
        public override Video FindWithIdEntity(int? id)
        {
            return _Context.Videos.FirstOrDefault(Video => Video.Id == id);
        }

        public override Video FindWithIdEntityAsNoTracking(int? id)
        {
            return _Context.Videos.AsNoTracking().FirstOrDefault(Video => Video.Id == id);
        }
    }
}