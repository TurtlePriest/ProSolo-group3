using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProSoLoPortal.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey("Profile")]
        public int ProfileRefId { get; set; }

        public Profile Profile { get; set; }

        public int RatingNum { get; set; }

    }
}
