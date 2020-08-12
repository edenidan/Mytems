using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }


        public DateTime JoinedDate { get; set; }
        public bool IsAdmin { get; set; }
       

        //at least one pair of these will be NULL:
        public int SellerID { get; set; }
        public Seller Seller { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}