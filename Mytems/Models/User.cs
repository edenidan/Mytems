using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required, StringLength(50), Index(IsUnique = true), Display(Name = "Username")]
        public string UserName { get; set; }
        [Required, MinLength(8), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, ScaffoldColumn(false)]
        public DateTime JoinedAt { get; set; }
        [Required, ScaffoldColumn(false)]
        public bool IsAdmin { get; set; }

    }
}
