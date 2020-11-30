using Microsoft.VisualStudio.TestTools.UnitTesting;
using example.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace example.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void BuyTest()
        {
            var controller = new HomeController();
            int id = 1;
            var result = controller.Buy(id) as ViewResult;
            Assert.AreEqual(id, result.ViewData["ProductId"]);
        }
    }
}