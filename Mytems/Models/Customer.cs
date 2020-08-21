using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Mytems.Models
{
    public class Customer : NonAdmin
    {
        [Required, ScaffoldColumn(false)]
        public string CategoryViewsJson { get; set; } // JSON representation of Dictionary<string, int>, from a caterory to the number of times the customer viewed it
        [NotMapped]
        public Dictionary<string, int> CategoryViews => JsonConvert.DeserializeObject<Dictionary<string, int>>(CategoryViewsJson); // TODO use JsonConvert to deserialize CategoryViewsJson

        public void IncreaseCategory(string category)
        {
            var d = this.CategoryViews;
            if (d.ContainsKey(category))
                d[category] = d[category] + 1;
            this.CategoryViewsJson = JsonConvert.SerializeObject(d);
            return;
        }
    }
}
