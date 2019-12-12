using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        [ForeignKey("ApplicationUser")]
        [Display(Name = "UserId")]
        public string UserRefId { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string EmployeeId { get; set; }

        public ApplicationUser Employee { get; set; }

        public double Rating { get; set; }

        public string ProfileText { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}