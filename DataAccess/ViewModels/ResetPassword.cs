using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ViewModels
{
    public class ResetPassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد کد اعتبارسنجی پست الکترونیکی الزامی است")]
        public string EmailCode { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد رمز عبور الزامی می باشد")]
        [IdentityPassword]
        public string NewPassword { get; set; }
        
        /*-----------------------------------------------------------*/
        
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
    }
}