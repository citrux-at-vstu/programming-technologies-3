using example.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace example.DAL
{
    public class ShopInitializer : DropCreateDatabaseAlways<ShopContext>
    {
        protected override void Seed(ShopContext context)
        {
            var products = new List<Product>
            {
            new Product { Name = "Стол", Price = 2000 },
            new Product { Name = "Стул", Price = 1000 },
            new Product { Name = "Табурет", Price = 500 },
            };
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}