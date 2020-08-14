using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Customer : User
    {

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required, ScaffoldColumn(false)]
        public string CategoryViewsJson { get; set; } // JSON representation of Dictionary<string, int>, from a caterory to the number of times the customer viewed it
        [NotMapped]
        public Dictionary<string, int> CategoryViews => null; // TODO use JsonConvert to deserialize CategoryViewsJson
    }
}
