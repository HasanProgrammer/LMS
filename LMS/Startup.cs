using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebFramework.Extensions;

namespace LMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services) /*DataService Container*/
        {
            services.AddControllersWithViews();
            services.AddMvc();
            
            /*-------------------------------------------------------*/
            
            /*Hasan's Code*/
            services.AddDatabaseContainer(Configuration);
            services.AddIdentityContainer(Configuration);
            services.AddIocContainer();
            services.AddAllServiceContainers(Configuration);
            services.AddJWTContainer(Configuration);
            services.AddConfigurationsContainer(Configuration);
            services.AddCorsContainer(Configuration);
            services.AddTaskSchedulingContainer(Configuration);
            services.AddPureApiVersion();
            services.AddSessionContainer(Configuration);
            /*Hasan's Code*/
        }
        
        public void Configure /*Middleware*/
        (
            IApplicationBuilder  app,
            IWebHostEnvironment  env,
            
            IServiceProvider     provider,
            IBackgroundJobClient jobs,
            IRecurringJobManager manager
        )
        {
            app.UseMyExceptionHandler(Configuration); /*For Test : OK*/
            
            /*-------------------------------------------------------*/
            
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config => config.UseMyExceptionHandler(Configuration)); /*For Publish : OK*/
                app.UseHsts();
            }
            
            /*-------------------------------------------------------*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            /*-------------------------------------------------------*/

            app.UseHangfireServer();
            
            /*-------------------------------------------------------*/
            
            app.UseAuthentication();

            /*-------------------------------------------------------*/

            app.UseRouting();
            
            /*-------------------------------------------------------*/
            
            app.UseCors("CORS");
            
            /*-------------------------------------------------------*/

            app.UseAuthorization();
            
            /*-------------------------------------------------------*/
            
            //app.UseSession();
            
            /*-------------------------------------------------------*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*-------------------------------------------------------*/

            jobs.UseBackgroundTasks(provider);
            manager.UseBackgroundTasks(provider);
        }
    }
}