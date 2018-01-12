using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TwitterFun.Controllers
{
    public class HomeController : Controller
    {
        #region Action for Read Content/Home View

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method to get the details from twitter
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDetails(int count, string name)
        {
           return Json(ReadTweets(count, name), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Action for About View

        /// <summary>
        /// Return view for About link
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Description:";

            return View();
        }

        #endregion

        #region Action for Write Content View

        // ToDo: Can be used if write content page is needed       

        #endregion
        
        #region Private Method

        /// <summary>
        /// Read the tweets from the twitter api
        /// </summary>
        /// <param name="count">Number of tweets needed</param>
        /// <param name="name">Name of the account</param>
        /// <returns>Return the tweets</returns>
        private string ReadTweets(int count, string name)
        {
            string response = string.Empty; // Variable to store the response and will be returned back to the caller
            // If no value is passed then return nothing
            if (count <= 0 || string.IsNullOrWhiteSpace(name))
            {
                return response;
            }

            // Call the read content controller to get the tweets
            ReadContentController contentController = new ReadContentController();
            response = contentController.ReadTwitterContent(count, name);                    

            // Return the response back to caller
            return response;
        }

        /// <summary>
        /// Method used to write tweet to an account
        /// </summary>
        /// <param name="tweet">Tweet that needs to be written</param>
        /// <param name="account">Name of the account</param>
        private void WriteTweet(string tweet, string account)
        {
            // Call write content controller to write tweet to an account
        }

        #endregion
    }
}