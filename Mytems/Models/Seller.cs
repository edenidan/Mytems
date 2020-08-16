using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Seller : NonAdmin
    {
        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public Location Location { get; set; }

        [ScaffoldColumn(false)]
        public double? Rating { get; set; }

        [ScaffoldColumn(false)]
        public int NumberOfRators { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
