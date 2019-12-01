using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Case
    {
        [Display(Name = "ID")]
        public int CaseId { get; set; }

        
        [Required, Display(Name = "Name of product")]
        public string Name { get; set; }

        [Display(Name = "Description of the case")]
        public string CaseDescription { get; set; }

        [Required,Display(Name = "Expected time frame")]
        public string TimeFrame { get; set; }

        
        [Required, Display(Name = "Number of products")]
        public int NumberOfProducts { get; set; }

        [Display(Name = "Name of seller")]
        public string Seller { get; set; }

        
        [Required, Display(Name = "Proposed price of product")]
        public int ProposedPrice { get; set; }

        [ForeignKey("ApplicationUser")]
        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }

        
        [Display(Name = "Customer name"), ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [Display(Name = "Locked")]
        public bool IsLocked { get; set; }
        [Display(Name = "Finished")]
        public bool IsFinished { get; set; }

        [Display(Name = "Time Frame Flexible")]
        public bool TimeFrameFexible { get; set; }
    }
}