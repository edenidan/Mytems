using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CategoriesBuyingHistory { get; set; }//dictionary<string,int> as JSON
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}