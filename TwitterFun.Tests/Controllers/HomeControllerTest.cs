using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterFun;
using TwitterFun.Controllers;

namespace TwitterFun.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Description:", result.ViewBag.Message);
        }

        [TestMethod]
        public void GetTenRecordsForSalesForce()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            JsonResult result = controller.GetDetails(10, "Salesforce");
            Assert.Fail("No URL provided for authentication");
        }

    }
}
