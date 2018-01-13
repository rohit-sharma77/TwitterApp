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
        public void TestIndexPageSuccess()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestAboutPageSuccess()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Description:", result.ViewBag.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No URL provided for authentication")]
        public void AuthUrlMissingFailTest()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            JsonResult result = controller.GetDetails(10, "Salesforce");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), "Error while getting the access token")]
        public void GetTenTweetsFailureForIncorrectCreds()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "test";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "test";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ReadTweetUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            ReadContentController contentController = new ReadContentController();
            contentController.ReadTwitterContent(10, "Salesforce");
        }

        [TestMethod]
        public void GetTenRecordsForSalesForceUserSuccess()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "LKfy4Pv8AOHEntNBwU567Clzg";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "NJTlecFoyE89FvZEGcH2hzrwTCddZu4e2EcsbE36rtwK4HpPVO";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ReadTweetUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            ReadContentController contentController = new ReadContentController();
            string response = contentController.ReadTwitterContent(10, "Salesforce");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException), "The remote server returned an error: (401) Unauthorized.")]
        public void GetTenTweetsFailsForIncorrectUsername()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "LKfy4Pv8AOHEntNBwU567Clzg";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "NJTlecFoyE89FvZEGcH2hzrwTCddZu4e2EcsbE36rtwK4HpPVO";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ReadTweetUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            ReadContentController contentController = new ReadContentController();
            contentController.ReadTwitterContent(10, "Test");
        }
    }
}
