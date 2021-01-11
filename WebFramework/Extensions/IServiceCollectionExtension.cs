using System;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Model;
using DataService.Entity.CategoryServices.V1;
using DataService.Entity.ChapterServices.V1;
using DataService.Entity.RoleServices.V1;
using DataService.Entity.TermServices.V1;
using DataService.Entity.UserServices.V2;
using DataService.Entity.VideoServices.V1;
using DataService.Web.Mail;
using WebFramework.Exceptions;
using WebFramework.Filters;

namespace WebFramework.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseContainer(this IServiceCollection service, IConfiguration configuration)
        {
            string ConnectionString = configuration.GetConnectionString("SQL Server");
            return service.AddDbContext<DatabaseContext>(Config => Config.UseSqlServer(ConnectionString));
        }
        
        public static IServiceCollection AddIocContainer(this IServiceCollection service)
        {
            //Category's DataService
            service.AddScoped<CategoryService<CategoriesViewModel, Category>, CategoryService>();
            
            //Role's DataService
            service.AddScoped<RoleService<RolesViewModel, Role>, RoleService>();
            //Role's DataService
            
            //User's DataService
            service.AddScoped<UserService<UsersViewModel, User>, UserService>();
            //User's DataService
            
            //Term's DataService
            service.AddScoped<TermService<TermsViewModel, Term>, TermService>();
            //Term's DataService
            
            //Chapter's DataService
            service.AddScoped<ChapterService<ChaptersViewModel, Chapter>, ChapterService>();
            //Chapter's DataService
            
            //Video's DataService
            service.AddScoped<VideoService<VideosViewModel, Video>, VideoService>();
            //Video's DataService
            
            //Other's DataService
            service.AddScoped<IMailSender, EmailSender>();
            //Other's DataService
            
            return service;
        }

        public static IServiceCollection AddAllServiceContainers(this IServiceCollection service, IConfiguration configuration)
        {
            //Global's Filter
            service.AddScoped<ModelValidation>();
            
            /*-------------------------------------------------------Admin's Filter-------------------------------------------------------*/

            //Category's Controller
            service.AddScoped<Filters.Areas.Admin.CategoryController.CheckCategory>();
            service.AddScoped<Filters.Areas.Admin.CategoryController.CheckUniqueName>();
            //Category's Controller
            
            //User's Controller
            service.AddScoped<Filters.Areas.Admin.UserController.CheckPassword>();
            service.AddScoped<Filters.Areas.Admin.UserController.CheckUniqueEmail>();
            service.AddScoped<Filters.Areas.Admin.UserController.CheckUniqueUserName>();
            service.AddScoped<Filters.Areas.Admin.UserController.CheckUser>();
            service.AddScoped<Filters.Areas.Admin.UserController.ImageUploader>();
            service.AddScoped<Filters.Areas.Admin.UserController.RegisterHandling>();
            //User's Controller
            
            /*-------------------------------------------------------Admin's Filter-------------------------------------------------------*/
            
            //Auth's Controller
            service.AddScoped<Filters.Controllers.AuthController.LoginHandling>();
            service.AddScoped<Filters.Controllers.AuthController.RegisterHandling>();
            //Auth's Controller
            
            //Term's Controller
            service.AddScoped<Filters.Controllers.TermController.CheckCategory>();
            service.AddScoped<Filters.Controllers.TermController.CheckTerm>();
            service.AddScoped<Filters.Controllers.TermController.CheckUniqueName>();
            service.AddScoped<Filters.Controllers.TermController.ImageUploader>();
            service.AddScoped<Filters.Controllers.TermController.TermPolicy>();
            //Term's Controller
            
            //Chapter's Controller
            service.AddScoped<Filters.Controllers.ChapterController.CheckTerm>();
            service.AddScoped<Filters.Controllers.ChapterController.TermPolicy>();
            service.AddScoped<Filters.Controllers.ChapterController.ChapterPolicy>();
            service.AddScoped<Filters.Controllers.ChapterController.CheckChapter>();
            service.AddScoped<Filters.Controllers.ChapterController.CheckUniqueTitle>();
            //Chapter's Controller
            
            //Video's Controller
            service.AddScoped<Filters.Controllers.VideoController.ChapterPolicy>();
            service.AddScoped<Filters.Controllers.VideoController.CheckChapter>();
            service.AddScoped<Filters.Controllers.VideoController.CheckTerm>();
            service.AddScoped<Filters.Controllers.VideoController.CheckVideo>();
            service.AddScoped<Filters.Controllers.VideoController.TermPolicy>();
            service.AddScoped<Filters.Controllers.VideoController.VideoPolicy>();
            service.AddScoped<Filters.Controllers.VideoController.VideoUploader>();
            //Video's Controller
            
            //Home's Controller
            service.AddScoped<Filters.Controllers.HomeController.CheckCategory>();
            service.AddScoped<Filters.Controllers.HomeController.CheckTerm>();
            service.AddScoped<Filters.Controllers.HomeController.CheckVideo>();
            service.AddScoped<Filters.Controllers.HomeController.VideoPolicy>();
            //Home's Controller
            
            return service;
        }

        public static IServiceCollection AddIdentityContainer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit     = configuration.GetValue<bool>("Password:RequireDigit");
                options.Password.RequiredLength   = configuration.GetValue<int> ("Password:RequiredLength");
                options.Password.RequireLowercase = configuration.GetValue<bool>("Password:RequireLowercase");
                options.Password.RequireUppercase = configuration.GetValue<bool>("Password:RequireUppercase");
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
            
            return service;
        }

        public static IServiceCollection AddPureApiVersion(this IServiceCollection service)
        {
            service.AddApiVersioning();

            return service;
        }

        public static IServiceCollection AddJWTContainer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddAuthentication().AddJwtBearer(Config =>
            {
                /*در این قسمت موقع ارسال درخواست به سرور ، موارد ( اطلاعات ) موجود در سرور با اطلاعات ارسالی ( توکن ) بررسی می گردد*/
                /*در صورت وجود هر گونه تفاوتی بین داده های تنظیم شده در سرور با اطلاعات ارسالی از سمت کاربر ( توکن ) ؛ اعتبارسنجی کاربر نامعتبر خواهد شد*/
                Config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer      = configuration.GetValue<string>("JWT:Issuer"),   /*صادر کننده*/
                    ValidAudience    = configuration.GetValue<string>("JWT:Audience"), /*مصرف کننده*/
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( configuration.GetValue<string>("JWT:Key") )),
                    
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime         = true
                };

                /*در این قسمت بررسی می شود که در صورت بروز هر گونه خطایی از سمت سرور ، چه واکنش مناسبی به کلاینت ( کاربر ) ارسال گردد*/
                Config.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        /*در این قسمت ؛ صحت توکن ارسالی کاربر بررسی می گردد و در صورت نادرست بودن توکن ارسالی ؛ خطای مناسب برای کاربر صادر می گردد*/
                        if (context.Exception.GetType() == typeof(SecurityTokenSignatureKeyNotFoundException)) throw new TokenNotValidException();
                            
                        /*در این قسمت ؛ دلیل عدم موفقیت آمیز بودن احراز هویت ، منقضی شدن زمان توکن می باشد که باید در این صورت خطای مناسب به کاربر صادر گردد*/
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException)) throw new TokenExpireException();
                        
                        return Task.CompletedTask;
                    }
                    
                    ,
                    
                    /*این قسمت مربوط به سطوح دسترسی یا همان ACL می باشد*/
                    OnForbidden = context => throw new UnAuthorizedException()
                    
                    ,
                    
                    OnChallenge = delegate(JwtBearerChallengeContext context)
                    {
                        if (context.AuthenticateFailure != null) throw new AuthenticationFaildException();
                        return Task.CompletedTask;
                    }
                };
            });
            
            return service;
        }
        
        public static IServiceCollection AddTaskSchedulingContainer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHangfire
            (
                config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                .UseSimpleAssemblyNameTypeSerializer()
                                .UseRecommendedSerializerSettings()
                                .UseSqlServerStorage(configuration.GetConnectionString("SQL Server"))
            );

            service.AddHangfireServer();

            return service;
        }
        
        public static IServiceCollection AddCorsContainer(this IServiceCollection service)
        {
            service.AddCors(Option =>
            {
                Option.AddPolicy("CORS", Builder =>
                {
                    Builder.WithOrigins("http://localhost:3000")
                           .WithOrigins("http://localhost:3001")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("X-Pagination"); /*این یک Header شخصی است که برای ارسال داده ها به شکل صفحه بندی شده مورد استفاده قرار می گیرد*/
                });
            });
            
            return service;
        }

        public static IServiceCollection AddSessionContainer(this IServiceCollection service, IConfiguration configuration)
        {
            // service.AddDistributedMemoryCache();
            // service.AddSession(Config =>
            // {
            //     Config.IdleTimeout = TimeSpan.FromMinutes(configuration.GetValue<int>("Session:Timer"));
            // });
            // service.AddMvc();

            return service;
        }

        public static IServiceCollection AddConfigurationsContainer(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<Config.StatusCode>(config: configuration.GetSection("StatusCode"))
                   .Configure<Config.Messages>  (config: configuration.GetSection("Messages"))
                   .Configure<Config.JWT>       (config: configuration.GetSection("JWT"))
                   .Configure<Config.Password>  (config: configuration.GetSection("Password"))
                   .Configure<Config.File>      (config: configuration.GetSection("File"))
                   .Configure<Config.ZarinPal>  (config: configuration.GetSection("ZarinPal"))
                   .Configure<Config.AdminData> (config: configuration.GetSection("AdminData"))
                   .Configure<Config.Mail>      (config: configuration.GetSection("SMTP"));
            
            return service;
        }
    }
}