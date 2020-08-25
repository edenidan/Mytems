using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.ViewModels.Products
{
    public class IndexProduct
    {
        [ScaffoldColumn(false)]
        public int ProductID { get; set; }
        public string Name { get; set; }
        public bool Sold { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "Seller Username")]
        public string SellerUsername { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public IndexProduct(Product product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Sold = product.Sold;
            Price = product.Price;
            SellerUsername = product.Seller.Username;
            Category = product.Category;
            Description = product.Description;
            ImagePath = product.ImagePath;
        }

        public bool CanBeEditedBy(User user) => user is Admin || (user is Seller && user.Username == SellerUsername);
    }
}
