using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Seller
    {
        public int SellerID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public double Rating { get; set; }

        public string PhoneNumber { get; set; }
        
    }
}