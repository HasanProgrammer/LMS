using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ViewModels
{
    public class CreateChapterViewModel
    {
        [Required(ErrorMessage = "فیلد دوره برنامه نویسی الزامی می باشد")]
        public int? Term { get; set; }
        
        [Required(ErrorMessage = "فیلد عنوان دوره برنامه نویسی الزامی می باشد")]
        [UniqueTitle(ErrorMessage = "فیلد عنوان دوره برنامه نویسی قبلا انتخاب شده است")]
        public string Title { get; set; }
        
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
                                     .Equals("Chapter.Create")
                )
                {
                    if(validationContext.GetRequiredService<DatabaseContext>().Chapters.SingleOrDefault(Chapter => Chapter.Title.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
            
                return ValidationResult.Success;
            }
        }
    }
}