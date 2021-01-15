using Model.Bases;
using Model.Enums;

namespace DataAccess.ViewModels
{
    public partial class TransactionsViewModel : IViewEntity /* Version-I */
    {
        public string UserName  { get; set; }
        public string UserImage { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public string TermName { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public string RefId      { get; set; }
        public string UserEmail  { get; set; }
        public string UserPhone  { get; set; }
        public string Status     { get; set; }
        public string DateCreate { get; set; }
        public int? Price        { get; set; }
    }
}