using System.Collections.Generic;
using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class TermsViewModel : IViewEntity /* Version-I */
    {
        public string UserId          { get; set; }
        public string UserName        { get; set; }
        public string UserExpert      { get; set; }
        public string UserDescription { get; set; }
        public string UserImage       { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? Id                { get; set; }
        public string Image           { get; set; }
        public string Name            { get; set; }
        public string Description     { get; set; }
        public string Suitable        { get; set; }
        public string Result          { get; set; }
        public int? Price             { get; set; }
        public int HasChapterKey      { get; set; } /*نمایش مقدار 1 | 0*/
        public string HasChapterValue { get; set; } /*برای نمایش فارسی دارد | ندارد*/
        public int StatusKey          { get; set; } /*برای نمایش مقدار 1 | 0*/
        public string StatusValue     { get; set; } /*برای نمایش فارسی فعال | غیر فعال*/
        public string DateStart       { get; set; }
        public string DateEnd         { get; set; }
        public string DateCreate      { get; set; }
        public string DateUpdate      { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? CategoryId     { get; set; }
        public string CategoryName { get; set; }
        
        /*-----------------------------------------------------------*/

        public bool IsBuyed { get; set; } = false;
        
        /*-----------------------------------------------------------*/
        
        public int CountVideos  { get; set; }
        public int CountStudent { get; set; }
        
        /*-----------------------------------------------------------*/

        public List<VideosViewModel> Videos     { get; set; } = new List<VideosViewModel>();
        public List<ChaptersViewModel> Chapters { get; set; } = new List<ChaptersViewModel>();
        public List<CommentsViewModel> Comments { get; set; } = new List<CommentsViewModel>();
    }
}