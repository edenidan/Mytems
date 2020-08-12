using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Sale
    {
        public int SaleID { get; set; }
        public DateTime time { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}