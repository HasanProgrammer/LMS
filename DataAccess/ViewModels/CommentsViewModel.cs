using System.Collections.Generic;
using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class CommentsViewModel : IViewEntity /* Version-I */
    {
        public string UserName  { get; set; }
        public string UserImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public string TermName { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? Id           { get; set; }
        public string Title      { get; set; }
        public string Content    { get; set; }
        public int? CountAnswer  { get; set; }
        public bool Status       { get; set; }
        public string DateCreate { get; set; }
        
        /*-----------------------------------------------------------*/

        public List<AnswersViewModel> Answers { get; set; } = new List<AnswersViewModel>();
    }
}