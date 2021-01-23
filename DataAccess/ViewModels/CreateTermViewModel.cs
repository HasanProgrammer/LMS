using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ViewModels
{
    public class CreateTermViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام دوره الزامی است")]
        [MaxLength(100, ErrorMessage = "فیلد نام دوره نباید از 100 کاراکتر بیشتر باشد")]
        [UniqueName(ErrorMessage = "فیلد نام دوره قبلا انتخاب شده است")]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد توضیحات دوره الزامی است")]
        [MinLength(350, ErrorMessage = "فیلد توضیحات دوره باید حداقل 350 کاراکتر داشته باشد")]
        [MaxLength(1500, ErrorMessage = "فیلد توضیحات دوره نباید از 1500 کاراکتر بیشتر باشد")]
        public string Description { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد ( مناسب برای ) الزامی است")]
        [MaxLength(500, ErrorMessage = "فیلد ( مناسب برای ) نباید از 500 کاراکتر بیشتر باشد")]
        public string Suitable { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نتایج گذراندن دوره الزامی است")]
        [MaxLength(1000, ErrorMessage = "فیلد نتایج گذراندن دوره نباید از 1000 کاراکتر بیشتر باشد")]
        public string Result { get; set; }
        
        [Required(ErrorMessage = "فیلد قیمت دوره الزامی است")]
        public int? Price { get; set; }
        
        [Required(ErrorMessage = "فیلد دسته بندی دوره الزامی است")]
        [CheckCategory(ErrorMessage = "فیلد دسته بندی مورد نظر وجود خارجی ندارد")]
        public int? Category { get; set; }
        
        [Required(ErrorMessage = "فیلد ( داشتن یا نداشتن فصل بندی ) الزامی است")]
        [CheckChapterKey(ErrorMessage = "فیلد ( فصل ) باید یک مقدار عددی برابر با 0 و یا 1 باشد")]
        public int? HasChapter { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ شروع دوره برنامه نویسی الزامی می باشد")]
        [RegularExpression(@"([1-9]\d{3})\/(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])", ErrorMessage = "فرمت تاریخ شروع دوره صحیح نمی باشد")]
        public string DateStart { get; set; }
        
        [CheckEndDate(ErrorMessage = "فرمت تاریخ پایان دوره صحیح نمی باشد")]
        public string DateEnd { get; set; }
        
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
                                     .Equals("Term.Create")
                )
                {
                    if(validationContext.GetRequiredService<DatabaseContext>().Terms.SingleOrDefault(Term => Term.Name.Equals(value)) != null)
                        return new ValidationResult(ErrorMessage);
                    return ValidationResult.Success;
                }
            
                return ValidationResult.Success;
            }
        }
        
        public class CheckCategory : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if(validationContext.GetRequiredService<DatabaseContext>().Categories.Find(value) == null)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }
        
        public class CheckEndDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null && !Regex.IsMatch(value.ToString(), @"([1-9]\d{3})\/(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])"))
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }
        
        public class CheckChapterKey : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if(Convert.ToInt32(value) != 0 && Convert.ToInt32(value) != 1)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }
    }
}