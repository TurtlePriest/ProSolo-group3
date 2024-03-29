﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Bids
    {
        [Key]
        public int BidId { get; set; }

        [ForeignKey("Case")]
        public int CaseRefId { get; set; }

        [Display(Name = "Name of case")]
        public string CaseName { get; set; }

        public Case Case { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserRefId { get; set; }

        public ApplicationUser User { get; set; }

        [Display(Name = "Proposed time frame")]
        public string ProposedTimeFrame { get; set; }

        [Display(Name = "Proposed price")]
        public int BidPrice { get; set; }

        public bool RatedByCus { get; set; }
        public bool RatedByMan { get; set; }

        [ForeignKey("Profile")]
        public int ProfileRefId { get; set; }
    }
}