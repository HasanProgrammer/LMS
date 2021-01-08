using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace WebFramework.Jobs
{
    public class DeleteUnVerifyAccounts
    {
        private readonly IServiceProvider _Provider;
        
        public DeleteUnVerifyAccounts(IServiceProvider provider)
        {
            _Provider = provider;
        }
        
        /*در این متد ، کاربرانی که عضویت آنها ( توسط خودشان ) بعد از گذشت 15 دقیقه تایید نشده است ،حذف خواهند شد*/
        public void Process()
        {
            DatabaseContext context = _Provider.GetRequiredService<DatabaseContext>();
            
            context.Users.RemoveRange (
                _Provider.GetRequiredService<UserManager<User>>().Users.Where(User => !User.IsVerifyEmail && !User.IsVerifyPhone)
                                                                       .Where(User => Time.TimeStampNow() - User.CreatedAtTimeStamp >= _Provider.GetRequiredService<IConfiguration>().GetValue<int>("RemoveUserAccountAuto:TimerSeconds"))
                                                                       .ToList()
            );

            context.SaveChanges();
        }
    }
}