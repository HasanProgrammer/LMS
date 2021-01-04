using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;

namespace WebFramework.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<User> FindWithUsernameAsync(this UserManager<User> UserManager, string Username)
        {
            return await UserManager.Users.SingleOrDefaultAsync(User => User.UserName.Equals(Username));
        }
        
        public static async Task<User> FindWithEmailCodeAsync(this UserManager<User> UserManager, string EmailCode)
        {
            return await UserManager.Users.SingleOrDefaultAsync(User => User.EmailCode.Equals(EmailCode));
        }
        
        public static async Task<User> FindWithPhoneCodeAsync(this UserManager<User> UserManager, int? PhoneCode)
        {
            return await UserManager.Users.SingleOrDefaultAsync(User => User.PhoneCode == PhoneCode);
        }

        public static async Task<User> GetCurrentUserAsync(this UserManager<User> UserManager, HttpContext context)
        {
            return await UserManager.Users.FirstOrDefaultAsync(User => User.UserName.Equals( context.User.Identity.Name ));
        }
        
        public static User GetCurrentUser(this UserManager<User> UserManager, HttpContext context)
        {
            return UserManager.Users.FirstOrDefault(User => User.UserName.Equals( context.User.Identity.Name ));
        }
        
        public static async Task<bool> HasRoleAsync(this UserManager<User> UserManager, HttpContext context, string role)
        {
            return await UserManager.IsInRoleAsync(await UserManager.GetCurrentUserAsync(context), role);
        }
        
        public static bool HasRole(this UserManager<User> UserManager, HttpContext context, string role)
        {
            return UserManager.IsInRoleAsync(UserManager.GetCurrentUser(context), role).Result;
        }
    }
}