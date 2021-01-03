using System;
using System.Threading.Tasks;
using Hangfire;
using WebFramework.Jobs;

namespace WebFramework.Extensions
{
    public static class IRecurringJobManagerExtension
    {
        /*در این قسمت کلاس های JOB به عنوان Task قرار میگیرد*/
        public static void UseBackgroundTasks(this IRecurringJobManager jobs, IServiceProvider provider)
        {
            jobs.AddOrUpdate("RemoveUnVerifyAccountUsers", () => new DeleteUnVerifyAccounts(provider).Process(), Cron.Minutely);
        }
    }
}