using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Seller : User
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required, DataType(DataType.PhoneNumber), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public Location Location { get; set; }

        [ScaffoldColumn(false)]
        public double? Rating { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
