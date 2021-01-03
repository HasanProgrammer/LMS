using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using WebFramework.Jobs;
using WebFramework.Middlewares;

namespace WebFramework.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static void UseExceptionHandler(this IApplicationBuilder builder, IConfiguration configuration)
        {
            builder.UseMiddleware<ExceptionHandler>(configuration);
        }
    }
}