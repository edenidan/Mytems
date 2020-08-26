using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.PresentationModels.Customers
{
    public class CustomerSearchOptions
    {
        public string Username { get; set; }

        [Display(Name = "Joined At Minimum")]
        public DateTime? JoinedAtMin { get; set; }
        [Display(Name = "Joined At Maximum")]
        public DateTime? JoinedAtMax { get; set; }

        public IQueryable<Customer> ApplyOn(IQueryable<Customer> customers)
        {
            if (Username != null) customers = customers.Where(c => c.Username.StartsWith(Username));
            if (JoinedAtMin != null) customers = customers.Where(c => c.JoinedAt >= JoinedAtMin);
            if (JoinedAtMax != null) customers = customers.Where(c => c.JoinedAt <= JoinedAtMax);

            return customers;
        }
    }
}
