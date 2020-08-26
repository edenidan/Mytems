using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.PresentationModels.Products
{
    public class ProductSearchOptions
    {
        public string Name { get; set; }

        [Display(Name = "Seller Username")]
        public string SellerUsername { get; set; }
        public Category? Category { get; set; }
        [Display(Name = "Minimum Price")]
        public int? MinPrice { get; set; }
        [Display(Name = "Maximum Price")]
        public int? MaxPrice { get; set; }
        public bool? Sold { get; set; }
        [Display(Name = "Minimum Number Of Views")]
        public int? MinNumberOfViews { get; set; }

        public IQueryable<Product> ApplyOn(IQueryable<Product> products)
        {
            if (Name != null) products = products.Where(p => p.Name.StartsWith(Name));
            if (SellerUsername != null) products = products.Where(p => p.Seller.Username.StartsWith(SellerUsername));
            if (Category != null) products = products.Where(p => p.Category == Category);
            if (MinPrice != null) products = products.Where(p => p.Price >= MinPrice);
            if (MaxPrice != null) products = products.Where(p => p.Price <= MaxPrice);
            if (Sold != null) products = products.Where(p => p.Sold == Sold);
            if (MinNumberOfViews != null) products = products.Where(p => p.NumberOfViews >= MinNumberOfViews);

            return products;
        }
    }
}
