using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class CategoriesViewModel : IViewEntity /*Version I*/
    {
        public int? Id     { get; set; }
        public string Name { get; set; }
    }
}