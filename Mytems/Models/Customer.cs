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
        public string CategoryViewsJson { get; set; } // JSON representation of Dictionary<Category, int>, from a category to the number of times the customer viewed it
        [NotMapped]
        public Dictionary<Category, int> CategoryViews => JsonConvert.DeserializeObject<Dictionary<Category, int>>(CategoryViewsJson);

        public void IncrementViewsFor(Category category)
        {
            var dict = CategoryViews;
            dict[category] = dict.ContainsKey(category) ? dict[category] + 1 : 1;
            CategoryViewsJson = JsonConvert.SerializeObject(dict);
        }
    }
}
