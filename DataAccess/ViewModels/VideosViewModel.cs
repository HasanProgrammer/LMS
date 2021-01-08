using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class VideosViewModel : IViewEntity /* Version-I */
    {
        public string UserId    { get; set; }
        public string UserName  { get; set; }
        public string UserImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? ChapterId      { get; set; }
        public string ChapterTitle { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? TermId     { get; set; }
        public string TermName { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? Id            { get; set; }
        public string Title       { get; set; }
        public string Duration    { get; set; }
        public string Video       { get; set; }
        public int? IsFreeKey     { get; set; }
        public string IsFreeValue { get; set; }
        public int StatusKey      { get; set; }
        public string StatusValue { get; set; }
        public string DateCreate  { get; set; }
        public string DateUpdate  { get; set; }
    }
}