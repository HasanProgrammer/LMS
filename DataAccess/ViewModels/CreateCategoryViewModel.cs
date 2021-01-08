using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام دسته بندی الزامی می باشد")]
        [DataType(DataType.Text, ErrorMessage = "فیلد نام دسته بندی باید یک مقدار متنی باشد")]
        [MaxLength(50, ErrorMessage = "فیلد نام دسته بندی باید حداکثر 50 کاراکتر داشته باشد")]
        [UniqueName(ErrorMessage = "نام فعلی قبلا انتخاب شده است")]
        public string Name { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public class UniqueName : ValidationAttribute
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
                                     .Equals("Admin.Category.Create")
                )
                {
                    if(validationContext.GetRequiredService<DatabaseContext>().Categories.SingleOrDefault(RC => RC.Name.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
            
                return ValidationResult.Success;
            }
        }
    }
}