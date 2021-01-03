using System;
using Hangfire;

namespace WebFramework.Extensions
{
    public static class IBackgroundJobClientExtension
    {
        /*در این قسمت متدهای بی نام و در لحظه به عنوان Task مورد استفاده قرار میگیرد*/
        public static void UseBackgroundTasks(this IBackgroundJobClient jobs, IServiceProvider provider)
        {
            
        }
    }
}