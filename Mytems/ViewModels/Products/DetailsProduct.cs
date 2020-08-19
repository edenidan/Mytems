using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.ViewModels.Products
{
    public class DetailsProduct : IndexProduct
    {
        public string ImagePath { get; set; }

        [Display(Name = "Sold At")]
        public DateTime? SoldAt { get; set; }

        public DetailsProduct(Product product) : base(product)
        {
            ImagePath = product.ImagePath;
            SoldAt = product.SoldAt;
        }
    }
}
