using Microsoft.VisualStudio.TestTools.UnitTesting;
using example.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using example.DAL;
using NSubstitute;
using example.Models;
using System.Data.Entity;

namespace example.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        private IShopContext db;

        [TestInitialize()]
        public void Setup()
        {
            db = Substitute.For<IShopContext>();

            var data = new List<Product> { new Product { ID = 1, Name = "Table", Price = 1 } }.AsQueryable();
            var products = Substitute.For<DbSet<Product>, IQueryable<Product>>();
            ((IQueryable<Product>)products).Provider.Returns(data.Provider);
            ((IQueryable<Product>)products).Expression.Returns(data.Expression);
            ((IQueryable<Product>)products).ElementType.Returns(data.ElementType);
            ((IQueryable<Product>)products).GetEnumerator().Returns(_ => data.GetEnumerator());
            products.AsNoTracking().Returns(products);

            db.Products = products;
        }
        [TestMethod()]
        public void BuyTest()
        {
            var controller = new HomeController(db);
            int id = 1;
            var result = controller.Buy(id) as ViewResult;
            Assert.AreEqual(id, result.ViewData["ProductId"]);
        }

        [TestMethod()]
        public void IndexTest()
        {
            var controller = new HomeController(db);
            var result = controller.Index() as ViewResult;
            CollectionAssert.AreEqual(db.Products.ToList(), ((IEnumerable<Product>)result.ViewBag.Products).ToList());
        }
    }
}