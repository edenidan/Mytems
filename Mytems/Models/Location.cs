﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    [ComplexType]
    public class Location
    {
        [Required]
        public string Address { get; set; }
    }
}
