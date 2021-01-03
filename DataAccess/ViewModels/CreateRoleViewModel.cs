using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DataAccess.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام نقش الزامی می باشد")]
        [DataType(DataType.Text, ErrorMessage = "فرمت فیلد نام نقش صحیح نمی باشد")]
        [MaxLength(50, ErrorMessage = "فیلد نام نقش نباید از 50 کاراکتر بیشتر باشد")]
        [UniqueName(ErrorMessage = "فیلد نام نقش باید یکتا باشد")]
        public string Name { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public class UniqueName : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if(validationContext.GetRequiredService< RoleManager<Role> >().Roles.SingleOrDefault(Role => Role.Name.Equals(value)) != null)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }
    }
}