using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class VerifyAccount
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد کد اعتبارسنجی پست الکترونیکی الزامی است")]
        public string EmailCode { get; set; }
        
        [Required(ErrorMessage = "فیلد کد اعتبارسنجی شماره تماس ( تلفن همراه ) الزامی است")]
        public int? PhoneCode { get; set; }
    }
}