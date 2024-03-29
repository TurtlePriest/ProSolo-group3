﻿using ProSoLoPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.ViewModels.AdministrationViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}