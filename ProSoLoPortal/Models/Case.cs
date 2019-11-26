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

        [Display(Name = "Name of product")]
        public string Name { get; set; }

        [Display(Name = "Expected time frame")]
        public string TimeFrame { get; set; }

        [Display(Name = "Number of products")]
        public int NumberOfProducts { get; set; }

        [Display(Name = "Name of seller")]
        public string Seller { get; set; }

        [Display(Name = "Proposed price of product")]
        public int ProposedPrice { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserRefId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsLocked { get; set; }
        public bool IsFinished { get; set; }
        public bool TimeFrameFexible { get; set; }
    }
}