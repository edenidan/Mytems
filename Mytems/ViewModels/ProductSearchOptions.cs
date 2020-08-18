using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.ViewModels
{
    public class ProductSearchOptions
    {
        public string Name { get; set; }

        public string SellerName { get; set; }
        public Category? Category { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? Sold { get; set; }
        public int? MinNumberOfViews { get; set; }

        public IQueryable<Product> ApplyOn(IQueryable<Product> products)
        {
            if (Name != null) products = products.Where(p => p.Name.StartsWith(Name));
            if (SellerName != null) products = products.Where(p => p.Seller.Username.StartsWith(SellerName));
            if (Category != null) products = products.Where(p => p.Category == Category);
            if (MinPrice != null) products = products.Where(p => p.Price >= MinPrice);
            if (MaxPrice != null) products = products.Where(p => p.Price <= MaxPrice);
            if (Sold != null) products = products.Where(p => p.Sold == Sold);
            if (MinNumberOfViews != null) products = products.Where(p => p.NumberOfViews >= MinNumberOfViews);

            return products;
        }
    }
}
