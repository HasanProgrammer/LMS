using System.Collections.Generic;
using Model.Bases;

namespace DataAccess.ViewModels
{
    public partial class UsersViewModel : IViewEntity /* Version-I */
    {
        // public string Id       { get; set; }
        // public string UserName { get; set; }
        // public string Email    { get; set; }
        
        // public List<RolesViewModel> Roles { get; set; }
    }
    
    public partial class UsersViewModel : IViewEntity /* Version-II */
    {
        public string Id        { get; set; }
        public string UserName  { get; set; }
        public string Email     { get; set; }
        public string Image     { get; set; }

        public List<RolesViewModel> Roles { get; set; }
    }
}