using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mytems.Models
{
    public class MytemsDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
