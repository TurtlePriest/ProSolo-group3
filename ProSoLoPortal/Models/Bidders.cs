using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Bidders
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public bool Checked { get; set; }

        public Bidders(string Name, bool Checked)
        {
            this.Name = Name;
            this.Checked = Checked;
        }
    }
}