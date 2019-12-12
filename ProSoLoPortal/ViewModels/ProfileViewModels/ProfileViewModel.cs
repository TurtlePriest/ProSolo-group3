using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.ViewModels.ProfileViewModels
{
    public class ProfileViewModel
    {
        public string ProfileText { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserRefId { get; set; }

        public string UserName { get; set; }

        public string EmployeeId { get; set; }

        public double Rating { get; set; }
    }
}
