using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Seller
    {
        public int SellerID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public Location Location { get; set; }

        public double? Rating { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
