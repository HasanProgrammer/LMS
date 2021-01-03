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
using Service.Entity.CategoryServices.V1;
using Service.Entity.RoleServices.V1;
using Service.Entity.UserServices.V2;
using Service.Web.Mail;
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
            //Category's Services
            service.AddScoped<ICategoryService<CategoriesViewModel, Category>, CategoryService>();
            
            //Role's Services
            service.AddScoped<IRoleService<RolesViewModel, Role>, RoleService>();
            //Role's Services
            
            //User's Services
            service.AddScoped<IUserService<UsersViewModel, User>, UserService>();
            //User's Services
            
            //Other's Service
            service.AddScoped<IMailSender, EmailSender>();
            //Other's Service
            
            return service;
        }

        public static IServiceCollection AddAllServiceContainers(this IServiceCollection service, IConfiguration configuration)
        {
            //Global's Filter
            service.AddScoped<ModelValidation>();
            service.AddScoped<LoginHandling>();
            service.AddScoped<RegisterHandling>();
            
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
                    ValidIssuer      = configuration.GetValue<string>("JWT:Issuer"), /*صادر کننده*/
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
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("X-Pagination"); /*این یک Header شخصی است که برای ارسال داده ها به شکل صفحه بندی شده مورد استفاده قرار می گیرد*/
                });
            });
            
            return service;
        }

        public static IServiceCollection AddConfigurationsContainer(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<Config.StatusCode>(config: configuration.GetSection("StatusCode"))
                   .Configure<Config.Messages>  (config: configuration.GetSection("Messages"))
                   .Configure<Config.JWT>       (config: configuration.GetSection("JWT"))
                   .Configure<Config.Password>  (config: configuration.GetSection("Password"))
                   .Configure<Config.File>      (config: configuration.GetSection("File"))
                   .Configure<Config.Mail>      (config: configuration.GetSection("SMTP"));
            
            return service;
        }
    }
}