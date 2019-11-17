using Microsoft.AspNetCore.Mvc.Rendering;
using ProSoLoPortal.Models;
using System.Collections.Generic;

namespace ProSoLoPortal.ViewModels
{
    public class UserViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public SelectList RoleNameList { get; set; }
        public string FirstName { get; set; }
        public string SearchString { get; set; }
        public string Role { get; set; }
    }
}