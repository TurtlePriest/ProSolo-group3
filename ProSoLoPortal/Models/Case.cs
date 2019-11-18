using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Case
    {
        public int CaseId { get; set; }
        public string CaseInformation { get; set; }
        public string kussemåtte { get; set; }

        public enum CaseStatus
        {
            OPEN,
            LOCKED,
            CLOSED
        }
    }
}