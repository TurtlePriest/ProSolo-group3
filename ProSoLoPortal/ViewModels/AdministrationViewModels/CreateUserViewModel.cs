using Microsoft.AspNetCore.Mvc.Rendering;
using ProSoLoPortal.Helpers;
using ProSoLoPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.ViewModels.AdministrationViewModels
{
    public class CreateUserViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = " Office Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public RolesNames? Roles { get; set; }
    }
}
