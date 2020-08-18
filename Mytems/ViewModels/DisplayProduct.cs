using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.ViewModels
{
    public class DisplayProduct
    {
        [ScaffoldColumn(false)]
        public int ProductID { get; set; }
        public string SellerName { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
