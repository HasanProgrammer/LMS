using System;
using System.IO;
using System.Linq;
using Common;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace WebFramework.Filters.Controllers.VideoController
{
    public class VideoUploader : ActionFilterAttribute
    {
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        private readonly Config.File       _File;
        
        //Environment
        private readonly IWebHostEnvironment _Environment;

        public VideoUploader
        (
            IWebHostEnvironment         HostingEnvironment,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage,
            IOptions<Config.File>       File
        )
        {
            //Configs
            _File          = File.Value;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            
            //Environment
            _Environment = HostingEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IFormFile file             = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is IFormFile) as IFormFile;
            CreateVideoViewModel model = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is CreateVideoViewModel) as CreateVideoViewModel;
            
            string UploadPathPrivate   = Path.Combine(_Environment.ContentRootPath, _File.UploadPathVideoPrivate);
            string UploadPathPublic    = Path.Combine(_Environment.WebRootPath    , _File.UploadPathVideoPublic);

            if
            (
                context.HttpContext.GetEndpoint().Metadata.GetMetadata<EndpointNameMetadata>().EndpointName.Equals("Video.Create")
            )
            {
                if (file == null || file.Length == 0)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.NotFoundVideo);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.NotFoundVideo, _StatusMessage.NotFoundVideo, new {});
                    return;
                }
                
                if (!file.IsVideo())
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectVideoType);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectVideoType, _StatusMessage.IsNotCorrectVideoType, new {});
                    return;
                }
                
                if (file.Length > _File.MaxSizeVideo)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.MaxSizeVideo);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.MaxSizeVideo, _StatusMessage.MaxSizeVideo, new {});
                    return;
                }

                var FileExtension = Path.GetExtension(file.FileName);
                var NewFileName   = Guid.NewGuid().ToString().Replace("-", "") + FileExtension;
                var FileStream    = new FileStream(Path.Combine(model.IsFree == 1 ? UploadPathPublic : UploadPathPrivate, NewFileName) , FileMode.Create);
                file.CopyTo(FileStream);
                FileStream.Close();
                
                context.HttpContext.GetRouteData().Values.Add("VideoPath", NewFileName);
            }
            else
            {
                if (file != null)
                {
                    if (!file.IsVideo())
                    {
                        JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectVideoType);
                        context.Result = new EmptyResult();
                        context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectVideoType, _StatusMessage.IsNotCorrectVideoType, new {});
                        return;
                    }
                
                    if (file.Length > _File.MaxSizeVideo)
                    {
                        JsonResponse.Handle(context.HttpContext, _StatusCode.MaxSizeVideo);
                        context.Result = new EmptyResult();
                        context.Result = JsonResponse.Return(_StatusCode.MaxSizeVideo, _StatusMessage.MaxSizeVideo, new {});
                        return;
                    }
                
                    var UploadedPathAlready = (context.HttpContext.GetRouteData().Values["Video"] as Video)?.VideoFile;
                    
                    if(File.Exists(UploadPathPublic + UploadedPathAlready))
                        File.Delete(UploadPathPublic + UploadedPathAlready);
                    
                    if(File.Exists(UploadPathPrivate + UploadedPathAlready))
                        File.Delete(UploadPathPrivate + UploadedPathAlready);
                    
                    var FileExtension = Path.GetExtension(file.FileName);
                    var NewFileName   = Guid.NewGuid().ToString().Replace("-", "") + FileExtension;
                    var FileStream    = new FileStream(Path.Combine(model.IsFree == 1 ? UploadPathPublic : UploadPathPrivate, NewFileName) , FileMode.Create);
                    file.CopyTo(FileStream);
                    FileStream.Close();
                
                    context.HttpContext.GetRouteData().Values.Add("VideoPath", NewFileName);
                }
                else
                {
                    /*در این حالت ، هیچ فیلمی به سرور ارسال نشده است ( برای ویرایش ) ولی امکان دارد ، وضعیت رایگان بودن فیلم تغییر کرده باشد*/
                    /*به همین منظور ، اگر وضعیت رایگان بودن فیلم تغییر کرده باشد ، باید دایرکتوری فیلم هم تغییر پیدا نماید و فیلم انتقال پیدا کند به مسیر دیگر*/
                    Video Video = context.HttpContext.GetRouteData().Values["Video"] as Video;
                    if (Video.IsFree != Convert.ToBoolean(model.IsFree))
                    {
                        if (Convert.ToBoolean(model.IsFree)) /*ویدئو رایگان شده است و فیلم مربوطه باید از فولدر اختصاصی به فولدر عمومی انتقال پیدا نماید*/
                        {
                            if (File.Exists(UploadPathPrivate + "\\" + Video.VideoFile))
                            {
                                File.Copy(UploadPathPrivate + "\\" + Video.VideoFile, UploadPathPublic + "\\" + Video.VideoFile, true);
                                File.Delete(UploadPathPrivate + "\\" + Video.VideoFile);
                            }
                        }
                        else /*ویدئو غیر رایگان شده است و فیلم مربوطه باید از فولدر عمومی به فولدر اختصاصی انتقال پیدا نماید*/
                        {
                            if (File.Exists(UploadPathPublic + "\\" + Video.VideoFile))
                            {
                                File.Copy(UploadPathPublic + "\\" + Video.VideoFile, UploadPathPrivate + "\\" + Video.VideoFile, true);
                                File.Delete(UploadPathPublic + "\\" + Video.VideoFile);
                            }
                        }
                    }
                }
            }
        }
    }
}