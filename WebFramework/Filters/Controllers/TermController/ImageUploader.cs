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
            IFormFile file = context.ActionArguments.Values.SingleOrDefault(Parameter => Parameter is IFormFile) as IFormFile;
            var UploadPath = Path.Combine(_Environment.WebRootPath, _File.UploadPathImagePublic);
            
            if
            (
                context.HttpContext.GetEndpoint().Metadata.GetMetadata<EndpointNameMetadata>().EndpointName.Equals("Term.Create")
            )
            {
                if (file == null || file.Length == 0)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.NotFoundImage);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.NotFoundImage, _StatusMessage.NotFoundImage, new {});
                    return;
                }
                
                if (!file.IsImage())
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectImageType);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectImageType, _StatusMessage.IsNotCorrectImageType, new {});
                    return;
                }
                
                if (file.Length > _File.MaxSizeImage)
                {
                    JsonResponse.Handle(context.HttpContext, _StatusCode.MaxSizeImage);
                    context.Result = new EmptyResult();
                    context.Result = JsonResponse.Return(_StatusCode.MaxSizeImage, _StatusMessage.MaxSizeImage, new {});
                    return;
                }

                var FileExtension = Path.GetExtension(file.FileName);
                var NewFileName   = Guid.NewGuid().ToString().Replace("-", "") + FileExtension;
                var FileStream    = new FileStream(Path.Combine(UploadPath, NewFileName) , FileMode.Create);
                file.CopyTo(FileStream);
                FileStream.Close();
                
                context.HttpContext.GetRouteData().Values.Add("ImagePath", NewFileName);
                context.HttpContext.GetRouteData().Values.Add("ImageType", FileExtension);
            }
            else
            {
                if (file != null)
                {
                    if (!file.IsImage())
                    {
                        JsonResponse.Handle(context.HttpContext, _StatusCode.IsNotCorrectImageType);
                        context.Result = new EmptyResult();
                        context.Result = JsonResponse.Return(_StatusCode.IsNotCorrectImageType, _StatusMessage.IsNotCorrectImageType, new {});
                        return;
                    }
                
                    if (file.Length > _File.MaxSizeImage)
                    {
                        JsonResponse.Handle(context.HttpContext, _StatusCode.MaxSizeImage);
                        context.Result = new EmptyResult();
                        context.Result = JsonResponse.Return(_StatusCode.MaxSizeImage, _StatusMessage.MaxSizeImage, new {});
                        return;
                    }
                
                    var UploadedPathAlready = (context.HttpContext.GetRouteData().Values["Term"] as Term).Image.Path;
                    if(File.Exists(UploadPath + UploadedPathAlready))
                        File.Delete(UploadPath + UploadedPathAlready);

                    var FileExtension = Path.GetExtension(file.FileName);
                    var NewFileName   = Guid.NewGuid().ToString().Replace("-", "") + FileExtension;
                    var FileStream    = new FileStream(Path.Combine(UploadPath, NewFileName) , FileMode.Create);
                    file.CopyTo(FileStream);
                    FileStream.Close();
                
                    context.HttpContext.GetRouteData().Values.Add("ImagePath", NewFileName);
                    context.HttpContext.GetRouteData().Values.Add("ImageType", FileExtension);
                }
            }
        }
    }
}