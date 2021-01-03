using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class RolesViewModel : IViewEntity /* Version-I */
    {
        public string RoleId   { get; set; }
        public string RoleName { get; set; }
        public int  CountUser  { get; set; }
    }
}