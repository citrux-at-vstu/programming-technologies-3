using example.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example.DAL
{
    public interface IShopContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Purchase> Purchases { get; set; }
        int SaveChanges();
    }
}
