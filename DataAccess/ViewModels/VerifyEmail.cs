using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class VerifyEmail
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد پست الکترونیکی الزامی است")]
        public string Email { get; set; }
    }
}