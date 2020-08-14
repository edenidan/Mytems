using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class MytemsDB: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public System.Data.Entity.DbSet<Mytems.Models.Customer> Customers { get; set; }
    }
}
