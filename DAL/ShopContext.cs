using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using example.Models;

namespace example.DAL
{
    public class ShopContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
}