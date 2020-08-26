using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.PresentationModels.Sellers
{
    public class SellerSearchOptions
    {
        public string Username { get; set; }

        [Display(Name = "Joined At Minimum")]
        public DateTime? JoinedAtMin { get; set; }
        [Display(Name = "Joined At Maximum")]
        public DateTime? JoinedAtMax { get; set; }
        public double? Rating { get; set; }

        public IQueryable<Seller> ApplyOn(IQueryable<Seller> sellers)
        {
            if (Username != null) sellers = sellers.Where(s => s.Username.StartsWith(Username));
            if (JoinedAtMin != null) sellers = sellers.Where(s => s.JoinedAt >= JoinedAtMin);
            if (JoinedAtMax != null) sellers = sellers.Where(s => s.JoinedAt <= JoinedAtMax);
            if (Rating != null) sellers = sellers.Where(s => s.Rating == Rating);

            return sellers;
        }
    }
}
