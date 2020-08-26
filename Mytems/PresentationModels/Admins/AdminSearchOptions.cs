using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mytems.Models;

namespace Mytems.PresentationModels.Admins
{
    public class AdminSearchOptions
    {
        public string Username { get; set; }

        [Display(Name = "Joined At Minimum")]
        public DateTime? JoinedAtMin { get; set; }
        [Display(Name = "Joined At Maximum")]
        public DateTime? JoinedAtMax { get; set; }

        public IQueryable<Admin> ApplyOn(IQueryable<Admin> admins)
        {
            if (Username != null) admins = admins.Where(a => a.Username.StartsWith(Username));
            if (JoinedAtMin != null) admins = admins.Where(a => a.JoinedAt >= JoinedAtMin);
            if (JoinedAtMax != null) admins = admins.Where(a => a.JoinedAt <= JoinedAtMax);

            return admins;
        }
    }
}
