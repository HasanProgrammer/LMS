using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ViewModels
{
    public class CreateVideoViewModel
    {
        public int? Chapter { get; set; }
        
        [Required(ErrorMessage = "فیلد ( دوره ) فیلم الزامی است")]
        public int? Term { get; set; }
        
        [Required(ErrorMessage = "فیلد ( رایگان بودن یا نبودن ) فیلم الزامی است")]
        [CheckIsFreeKey(ErrorMessage = "فیلد ( رایگان بودن یا نبودن ) فیلم باید ")]
        public int? IsFree { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان فیلم الزامی است")]
        [UniqueTitle(ErrorMessage = "فیلد عنوان فیلم قبلا انتخاب شده است")]
        public string Title { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد مدت زمان فیلم الزامی است")]
        public string Duration { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public class UniqueTitle : ValidationAttribute
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
                                     .Equals("Video.Create")
                )
                {
                    if(validationContext.GetRequiredService<DatabaseContext>().Videos.SingleOrDefault(Video => Video.Title.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
            
                return ValidationResult.Success;
            }
        }
        
        public class CheckIsFreeKey : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    if((int)value != 0 && (int)value != 1)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
                
                return ValidationResult.Success;
            }
        }
    }
}