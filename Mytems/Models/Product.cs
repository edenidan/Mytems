using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytems.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public Category Category { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        [Required, ScaffoldColumn(false)]
        public bool? Sold { get; set; }

        [Required, ScaffoldColumn(false)]
        public int? NumberOfViews { get; set; }

        [Required]
        public Seller Seller { get; set; }
    }
}
