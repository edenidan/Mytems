using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mytems.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public string Description { get; set; }

        //Location:
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public bool Sold { get; set; }

        public int SellerID { get; set; }
        public Seller Seller { get; set; }
    }
}