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
        public decimal Price { get; set; }

        [Required]
        public bool Sold { get; set; }

        public DateTime? SoldAt { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required] 
        public string ImagePath { get; set; }

        [Required]
        public int NumberOfViews { get; set; }

        [Required, ForeignKey("Seller")]
        public int SellerID { get; set; }
        public Seller Seller { get; set; }
    }
}
