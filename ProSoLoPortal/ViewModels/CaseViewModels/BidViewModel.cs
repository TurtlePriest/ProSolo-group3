using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.ViewModels.CaseViewModels
{
    public class BidViewModel
    {
        [Display(Name = "Expected time frame")]
        public string ProposedTimeFrame { get; set; }
        [Display(Name = "Price")]
        public int BidPrice { get; set; }
        public int CaseId { get; set; }
        public string UserId { get; set; }


    }
}
