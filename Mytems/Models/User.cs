using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public abstract class User
    {
        public int UserID { get; set; }
        [Required, StringLength(50), Index(IsUnique = true)]
        public string Username { get; set; }
        [Required, StringLength(400, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, ScaffoldColumn(false)]
        public DateTime JoinedAt { get; set; }

        public bool CanEditAndDelete(Product product) => this is Admin || (this is Seller && UserID == product.SellerID);
    }
}
