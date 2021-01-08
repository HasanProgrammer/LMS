using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class AnswersViewModel : IViewEntity /* Version-I */
    {
        public string UserName  { get; set; }
        public string UserImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public string Content { get; set; }
    }
}