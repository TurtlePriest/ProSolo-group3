using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Imagebank
    {

        [Key]
        public int PhotoID { get; set; }

        public string PhotoDescription { get; set; }




    }
}
