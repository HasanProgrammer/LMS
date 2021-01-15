using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Dto.Payment;
using Dto.Response.Payment;
using Kavenegar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;
using WebFramework.Filters.Controllers.HomeController;
using ZarinPal.Class;
using ZarinpalSandbox;
using ZarinpalSandbox.Models;
using Payment = ZarinPal.Class.Payment;

namespace LMS.Controllers.V1
{
    [ApiController]
    [Route(template: Config.Routing.BaseRoute)]
    [ApiVersion("1.0")]
    public class HomeController : Controller
    {
        //DataService
        private readonly IServiceProvider _Provider;
        private readonly TermService<TermsViewModel, Term> _TermService;
        
        //Configs
        private readonly IConfiguration    _Config;
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        private readonly Config.ZarinPal   _ZarinPal;
        private readonly Config.AdminData  _AdminData;
        
        //Context
        private readonly DatabaseContext _Context;
        
        //Mail
        private readonly IMailSender _EmailSender;

        public HomeController (
            IServiceProvider                  Provider,
            IConfiguration                    Config,
            DatabaseContext                   Context,
            IOptions<Config.StatusCode>       StatusCode,
            IOptions<Config.Messages>         StatusMessage,
            IOptions<Config.ZarinPal>         ZarinPal,
            IOptions<Config.AdminData>        AdminData,
            IMailSender                       MailSender,
            TermService<TermsViewModel, Term> TermService
        )
        {
            //DataService
            _Provider    = Provider;
            _TermService = TermService;
            
            //Configs
            _Config        = Config;
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            _ZarinPal      = ZarinPal.Value;
            _AdminData     = AdminData.Value;
            
            //Context
            _Context = Context;
            
            //Mail
            _EmailSender = MailSender;
        }
        
        [HttpGet]
        [Route(template: "terms", Name = "Home.Terms")]
        public async Task<JsonResult> AllTerms([FromQuery] PaginateQueryViewModel model)
        {
            PaginatedList<TermsViewModel> Terms = await _TermService.FindAllActiveAsNoTrackingWithPaginateAsync(model.PageNumber, model.CountSizePerPage);
            
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Terms.CurrentPage,
                Terms.CountSizePerPage,
                Terms.TotalPages,
                Terms.HasNext,
                Terms.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Terms });
        }
        
        [HttpGet]
        [Route(template: "terms/category/{name}", Name = "Home.Terms.Category")]
        [ServiceFilter(type: typeof(CheckCategory))]
        public async Task<JsonResult> AllTermsForCategory(string name, [FromQuery] PaginateQueryViewModel model)
        {
            PaginatedList<TermsViewModel> Terms = await _TermService.FindAllWithNoTrackingAndActiveForCategoryAndPaginateAsync(Request.HttpContext.GetRouteData().Values["Category"] as string, model.PageNumber, model.CountSizePerPage);
            
            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Terms.CurrentPage,
                Terms.CountSizePerPage,
                Terms.TotalPages,
                Terms.HasNext,
                Terms.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Terms });
        }
        
        [HttpGet]
        [Route(template: "term/count/category")]
        public async Task<JsonResult> CountTermForCategory()
        {
            (
                int CountCSH,
                int CountPHP,
                int CountPYT,
                int CountJS,
                int CountASP,
                int CountDjango,
                int CountLaravel,
                int CountReactJS,
                int CountSQLServer,
                int CountMYSql
            )
            = await _TermService.FindCountEntityForCategoryWithNoTrackingAsync
            (
                _Config.GetValue<string>("Category:C#"),
                _Config.GetValue<string>("Category:PHP"),
                _Config.GetValue<string>("Category:Python"),
                _Config.GetValue<string>("Category:JavaScript"),
                _Config.GetValue<string>("Category:ASPCore"),
                _Config.GetValue<string>("Category:Django"),
                _Config.GetValue<string>("Category:Laravel"),
                _Config.GetValue<string>("Category:ReactJS"),
                _Config.GetValue<string>("Category:SQLServer"),
                _Config.GetValue<string>("Category:MySQL")
            );
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new
            {
                CountCSH,
                CountPHP,
                CountPYT,
                CountJS,
                CountASP,
                CountDjango,
                CountLaravel,
                CountReactJS,
                CountSQLServer,
                CountMYSql
            });
        }

        [HttpGet]
        [Route(template: "term/details/{id:int}", Name = "Home.Term.Details")]
        [ServiceFilter(type: typeof(CheckTerm))]
        public async Task<JsonResult> ShowTermDetails(int id)
        {
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new
            {
                Term = await _TermService.FindWithIdAsNoTrackingAndActiveAsync(id, null)
            });
        }
        
        [HttpGet]
        [Route(template: "auth/term/details/{id:int}", Name = "Home.Term.Details.ForUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(type: typeof(CheckTerm))]
        public async Task<JsonResult> ShowTermDetailsForUser(int id)
        {
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new
            {
                Term = await _TermService.FindWithIdAsNoTrackingAndActiveAsync(id, await _Provider.GetRequiredService<UserManager<User>>().GetCurrentUserAsync(Request.HttpContext))
            });
        }

        [HttpGet]
        [Route(template: "video/public/download/{id:int}", Name = "Home.Term.Video.Public.Download")]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        public IActionResult DownloadVideoPublic(int id)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;
            
            JsonResponse.Handle(Request.HttpContext, "X-Video-Name", new { Name = Video.VideoFile });
            
            string VideoPathPublic = _Provider.GetRequiredService<IWebHostEnvironment>().WebRootPath + "\\" + _Config.GetValue<string>("File:UploadPathVideoPublic");

            FileStream Stream       = new FileStream(VideoPathPublic + Video.VideoFile, FileMode.Open, FileAccess.Read);
            FileStreamResult Result = File(Stream, "application/octet-stream", Video.VideoFile);
            return Result;
        }

        [HttpGet]
        [Route(template: "video/download/{id:int}", Name = "Home.Term.Video.Download")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(type: typeof(CheckVideo))]
        [ServiceFilter(type: typeof(VideoPolicy))]
        public IActionResult DownloadVideo(int id)
        {
            Video Video = Request.HttpContext.GetRouteData().Values["Video"] as Video;
            
            JsonResponse.Handle(Request.HttpContext, "X-Video-Name", new { Name = Video.VideoFile });
            
            string VideoPathPublic  = _Provider.GetRequiredService<IWebHostEnvironment>().WebRootPath     + "\\" + _Config.GetValue<string>("File:UploadPathVideoPublic");
            string VideoPathPrivate = _Provider.GetRequiredService<IWebHostEnvironment>().ContentRootPath + "\\" + _Config.GetValue<string>("File:UploadPathVideoPrivate");
            string VideoFinalPath   = Video.IsFree ? VideoPathPublic + Video.VideoFile : VideoPathPrivate + Video.VideoFile;

            FileStream Stream       = new FileStream(VideoFinalPath, FileMode.Open, FileAccess.Read);
            FileStreamResult Result = File(Stream, "application/octet-stream", Video.VideoFile);
            return Result;
        }

        /*در این متد ، یک درخواست خرید ( خرید دوره برنامه نویسی ) به زرین پال ارسال می گردد*/
        [HttpGet]
        [Route(template: "purchase/term/{id:int}", Name = "Home.Term.Purchase.Request")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(type: typeof(CheckTerm))]
        public async Task<JsonResult> PurchaseTermRequest(int id)
        {
            Term Term = Request.HttpContext.GetRouteData().Values["Term"] as Term;
            User User = await _Provider.GetRequiredService<UserManager<User>>().GetCurrentUserAsync(Request.HttpContext);

            Request Result = await new ZarinPal.Class.Payment().Request(new DtoRequest
            {
                Description = Term.Name,
                Amount      = Convert.ToInt32(Term.Price),
                CallbackUrl = $"{ _ZarinPal.CallbackURL }?Term={id}&Amount={Term.Price}&User={User.Id}&UserPhone={User.Phone}&UserEmail={User.Email}",
                MerchantId  = _ZarinPal.MerchantID 
            }, Payment.Mode.sandbox);
            
            if (Result.Status == 100)
                return JsonResponse.Return(_StatusCode.SuccessGateRequest, _StatusMessage.SuccessGateRequest, new { Url = _ZarinPal.GateUrl + "/" + Result.Authority });
            return JsonResponse.Return(_StatusCode.ErrorGateRequest, _StatusMessage.ErrorGateRequest, new { });
        }

        /*در این متد ، نتیجه خرید کاربر از طرف زرین پال دریافت میشود که آیا پرداخت موفق بوده یا خیر*/ 
        [HttpGet]
        [Route(template: "purchase/verification", Name = "Home.Term.Purchase.Verification")]
        public async Task<JsonResult> VerificationPurchase()
        {
            if
            (
                !string.IsNullOrEmpty(Request.Query["Status"])    &&
                !string.IsNullOrEmpty(Request.Query["Authority"]) &&
                !string.IsNullOrEmpty(Request.Query["Term"])      &&
                !string.IsNullOrEmpty(Request.Query["Amount"])    &&
                !string.IsNullOrEmpty(Request.Query["User"])      &&
                !string.IsNullOrEmpty(Request.Query["UserPhone"]) &&
                !string.IsNullOrEmpty(Request.Query["UserEmail"]) &&
                Request.Query["Status"].ToString().ToUpper() == "OK"
            )
            {
                /*در این قسمت بررسی می گردد که آیا کاربر مبلغ مورد نظر را در تراکنش ( Authority ) به درستی پرداخت کرده است یا خیر*/
                Verification Result = await new ZarinPal.Class.Payment().Verification(new DtoVerification
                {
                    Amount     = Convert.ToInt32(Request.Query["Amount"]),
                    Authority  = Request.Query["Authority"],
                    MerchantId = _ZarinPal.MerchantID
                }, Payment.Mode.sandbox);
                
                /*-----------------------------------------------*/
                
                if (Result.Status == 100) /*تراکنش درست و مطابق همان مبلغی بوده است که مورد انتظار ما است*/
                {
                    /*در این قسمت می بایست شئ مربوط به تراکش را ساخت و اطلاعات تراکنش را در آن ذخیره کرد*/
                    await _Context.Transactions.AddAsync(new Transaction
                    {
                        UserId     = Request.Query["User"],
                        TermId     = Convert.ToInt32(Request.Query["Term"]),
                        RefId      = Result.RefId != null ? Result.RefId.ToString() : null,
                        Price      = Convert.ToInt32(Request.Query["Amount"]),
                        UserPhone  = Request.Query["UserPhone"],
                        UserEmail  = Request.Query["UserEmail"],
                        Status     = Model.Enums.Transaction.Status.Active,
                        StatusCode = 200,
                        CreatedAt  = PersianDatetime.Now(),
                        UpdatedAt  = PersianDatetime.Now()
                    });
                    await _Context.Buys.AddAsync(new Buy
                    {
                        UserId    = Request.Query["User"], 
                        TermId    = Convert.ToInt32(Request.Query["Term"]),
                        CreatedAt = PersianDatetime.Now()
                    });
                    await _Context.SaveChangesAsync();
                    
                    /*-----------------------------------------------*/
                    
                    /*در این قسمت به مدرس دوره و ادمین سایت ، پیامک و پست الکترونیکی خرید جدید ارسال می گردد*/
                    Term Term = _TermService.FindWithIdEntityWithEagerLoadingAsNoTracking(Convert.ToInt32(Request.Query["Term"]));
                    
                    string TitleMessage = "خرید جدید دوره برنامه نویسی : "  + $"{ Term.Name }"    + " | " + " اطلاعات خریدار : " + $"{ Request.Query["UserPhone"] }" + " * " + $"{ Request.Query["UserEmail"] }";
                    string MainMessage  = "یک خرید جدید به شماره تراکنش : " + $"{ Result.RefId }" + " در سیستم ثبت گردید است ";
                    
                    await _EmailSender.SendFromHtmlAsync(new List<string> { Term.User.Email , _AdminData.Email }, $"<div dir='rtl'>{ TitleMessage }</div>", $"<div dir='rtl'>{ MainMessage }</div>");

                    try
                    {
                        KavenegarApi sms = new KavenegarApi(_Config.GetValue<string>("SMS:Key"));
                        sms.Send(_Config.GetValue<string>("SMS:Sender"), _AdminData.Phone, TitleMessage + " | " + MainMessage);
                        sms.Send(_Config.GetValue<string>("SMS:Sender"), Term.User.Phone , TitleMessage + " | " + MainMessage);
                    }
                    catch (Exception e)
                    {
                        /*در این قسمت باید یک هشداری مانند ارسال پست الکترونیکی به مدیر سیستم مبنی بر اشکال در ارسال پیامک ، ارسال گردد*/
                    }

                    /*-----------------------------------------------*/
                    
                    return JsonResponse.Return(_StatusCode.SuccessPayment, _StatusMessage.SuccessPayment, new 
                    {
                        RefTransaction = Result.RefId
                    });
                }
                
                /*-----------------------------------------------*/
                
                /*در این مرحله پرداخت مطابغ مبلغ مورد انتظار ما نبوده است یا خطایی مشابه رخ داده است*/
                return JsonResponse.Return(_StatusCode.ErrorPayment, _StatusMessage.ErrorPayment, new {});
            }

            return JsonResponse.Return(_StatusCode.ErrorPayment, _StatusMessage.ErrorPayment, new {});
        }
    }
}