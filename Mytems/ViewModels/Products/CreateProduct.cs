using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.ViewModels.Products
{
    public class CreateProduct
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required] // required only for admins
        public int? SellerID { get; set; }

        public Product ToProduct() => new Product
        {
            Name = Name,
            Price = Price,
            Sold = false,
            SoldAt = null,
            Category = Category,
            Description = Description,
            ImagePath = "/assets/img/default_image.png",
            NumberOfViews = 0,
            SellerID = SellerID.GetValueOrDefault(0),
        };
    }
}
