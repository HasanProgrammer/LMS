using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DataAccess.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام کاربری الزامی می باشد")]
        [MaxLength(20, ErrorMessage = "فیلد نام کاربری نباید از 20 کاراکتر بیشتر باشد")]
        [MinLength(5, ErrorMessage = "فیلد نام کاربری نباید از 5 کاراکتر کمتر باشد")]
        [DataType(DataType.Text, ErrorMessage = "فرمت نام کاربری وارده صحیح نمی باشد")]
        [UniqueUsernameUser(ErrorMessage = "فیلد نام کاربری مورد نظر قبلا انتخاب شده است")]
        public string Username { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام کاربری الزامی می باشد")]
        [IdentityPassword]
        public string Password { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد پست الکترونیکی الزامی می باشد")]
        [DataType(DataType.EmailAddress, ErrorMessage = "فرمت پست الکترونیکی وارده صحیح نمی باشد")]
        [UniqueEmailUser(ErrorMessage = "پست الکترونیکی مورد نظر قبلا استفاده شده است")]
        public string Email { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نقش سیستمی الزامی می باشد")]
        [DataType(DataType.Text, ErrorMessage = "فرمت نقش سیستمی صحیح نمی باشد")]
        [CheckRole(ErrorMessage = "نقش مورد نظر در سیستم تعریف نشده است")]
        public List<string> Roles { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public class UniqueUsernameUser : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if
                (
                    validationContext.GetRequiredService<IHttpContextAccessor>()
                                     .HttpContext
                                     .GetEndpoint()
                                     .Metadata
                                     .GetMetadata<EndpointNameMetadata>()
                                     .EndpointName
                                     .Equals("Admin.User.Create")
                )
                {
                    if(validationContext.GetRequiredService<UserManager<User>>().Users.SingleOrDefault(User => User.UserName.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
                return ValidationResult.Success;
            }
        }
        
        public class UniqueEmailUser : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if
                (
                    validationContext.GetRequiredService<IHttpContextAccessor>()
                                     .HttpContext
                                     .GetEndpoint()
                                     .Metadata
                                     .GetMetadata<EndpointNameMetadata>()
                                     .EndpointName
                                     .Equals("Admin.User.Create")
                )
                {
                    if(validationContext.GetRequiredService<UserManager<User>>().Users.SingleOrDefault(User => User.Email.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
                return ValidationResult.Success;
            }
        }
        
        public class IdentityPassword : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                IConfiguration configuration = validationContext.GetRequiredService<IConfiguration>();
                
                if(value != null && !Regex.IsMatch(value as string, configuration.GetValue<string>("Password:Regex")))
                    return new ValidationResult(configuration.GetValue<string>("Password:Message"));
                return ValidationResult.Success;
            }
        }
        
        public class CheckRole : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                HttpContext context           = validationContext.GetRequiredService<IHttpContextAccessor>().HttpContext;
                RoleManager<Role> RoleManager = validationContext.GetRequiredService<RoleManager<Role>>();
            
                List<string> rolesName = new List<string>();
                List<string> roles     = value as List<string>;

                if (roles != null)
                {
                    foreach (string role in roles)
                    {
                        Role targetRole = RoleManager.FindByIdAsync(role).Result;
                        if(targetRole == null)
                            return new ValidationResult(ErrorMessage);
                    
                        rolesName.Add(targetRole.Name);
                    }
                }
            
                context.GetRouteData().Values.Add("Roles", rolesName);
                return ValidationResult.Success;
            }
        }
    }
}