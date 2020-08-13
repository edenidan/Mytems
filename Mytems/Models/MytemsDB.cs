using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class MytemsDB: DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}