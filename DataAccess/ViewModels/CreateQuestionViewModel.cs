using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class CreateQuestionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان سوال ( دیدگاه ) الزامی می باشد")]
        [MaxLength(50, ErrorMessage = "فیلد عنوان سوال ( دیدگاه ) نباید از 50 عبارت بیشتر باشد")]
        public string Title { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد متن سوال ( دیدگاه ) الزامی می باشد")]
        [MaxLength(1500, ErrorMessage = "فیلد متن سوال ( دیدگاه ) نباید از 1500 عبارت بیشتر باشد")]
        public string Content { get; set; }
    }
}