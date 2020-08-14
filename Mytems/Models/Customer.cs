using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Customer
    {
        [ForeignKey("User"),Required]
        public int CustomerID { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public string CategoryViewsJson { get; set; } // JSON representation of Dictionary<string, int>, from a caterory to the number of times the customer viewed it
        [NotMapped]
        public Dictionary<string, int> CategoryViews => null; // TODO use JsonConvert to deserialize CategoryViewsJson
    }
}
