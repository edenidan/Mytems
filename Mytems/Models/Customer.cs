using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CategoryViews { get; set; }//Dictionary<string,int> represented by JSON
        //From caterory to the number of times the customer viewed it.
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}