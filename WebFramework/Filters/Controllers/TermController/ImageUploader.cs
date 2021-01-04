using System;
using System.IO;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace WebFramework.Filters.Controllers.TermController
{
    public class ImageUploader : ActionFilterAttribute
    {
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        private readonly Config.File       _File;
        
        //Environment
        private readonly IWebHostEnvironment _Environment;

        public ImageUploader
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
            //I
            IFormFile file = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is IFormFile) as IFormFile;
            var UploadPath = Path.Combine(_Environment.WebRootPath, _File.UploadPathVideo);
            
            //II
            if (file != null)
            {
                //III
                if (!file.IsImage())
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectImageType);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectImageType, _StatusMessage.IsNotCorrectImageType, new {});
                    return;
                }
                
                //IV
                if (file.Length > _File.MaxSizeImage)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.MaxSizeImage);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.MaxSizeImage, _StatusMessage.MaxSizeImage, new {});
                    return;
                }
                
                //V
                var UploadedPathAlready = (context.HttpContext.GetRouteData().Values["Term"] as Term)?.Image?.Path;
                var RootPath = _Environment.WebRootPath + "\\" + _File.UploadPath;
                if(File.Exists(RootPath + UploadedPathAlready))
                    File.Delete(RootPath + UploadedPathAlready);

                //VI
                var FileExtension = Path.GetExtension(file.FileName);
                var NewFileName   = Guid.NewGuid().ToString().Replace("-", "") + FileExtension;
                var FileStream    = new FileStream(Path.Combine(UploadPath, NewFileName) , FileMode.Create);
                file.CopyTo(FileStream);
                
                //VII
                context.HttpContext.GetRouteData().Values.Add("ImagePath", NewFileName);
                context.HttpContext.GetRouteData().Values.Add("ImageType", FileExtension);
            }
        }
    }
}