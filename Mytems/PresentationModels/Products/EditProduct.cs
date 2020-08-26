using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.PresentationModels.Products
{
    public class EditProduct : CreateProduct
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public bool Sold { get; set; }

        public EditProduct() { }

        public EditProduct(Product product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Price = product.Price;
            Sold = product.Sold;
            Category = product.Category;
            Description = product.Description;
            SellerID = product.SellerID;
        }
    }
}
