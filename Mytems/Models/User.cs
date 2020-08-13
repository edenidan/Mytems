using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required, Index(IsUnique = true)]
        public string UserName { get; set; }
        [Required, MinLength(8), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; }
        [Required]
        public bool IsAdmin { get; set; }

        // At least one will be null
        public Seller Seller { get; set; }
        public Customer Customer { get; set; }
    }
}
