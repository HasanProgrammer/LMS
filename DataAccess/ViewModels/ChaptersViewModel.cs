using System.Collections.Generic;
using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class ChaptersViewModel : IViewEntity /* Version-I */
    {
        public string UserId    { get; set; }
        public string UserName  { get; set; }
        public string UserImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? TermId      { get; set; }
        public string TermName  { get; set; }
        public string TermImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public int? Id           { get; set; }
        public string Title      { get; set; }
        public string DateCreate { get; set; }
        public string DateUpdate { get; set; }
        
        /*-----------------------------------------------------------*/

        public List<VideosViewModel> Videos { get; set; }
    }
}