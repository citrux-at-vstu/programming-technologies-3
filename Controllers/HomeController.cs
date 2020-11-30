using example.DAL;
using example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace example.Controllers
{
    public class HomeController : Controller
    {
        private IShopContext db;
        public HomeController(IShopContext context)
        {
            db = context;
        }
        public HomeController() : this(new ShopContext()) { }
        public ActionResult Index()
        {
            IEnumerable<Product> products = db.Products;
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            db.Purchases.Add(purchase);
            db.SaveChanges();
            return "Спасибо за покупку, " + purchase.Person + "!";
        }
    }
}