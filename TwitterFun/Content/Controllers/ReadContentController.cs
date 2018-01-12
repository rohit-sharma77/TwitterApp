using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace TwitterFun.Controllers
{
    /// <summary>
    /// Class to read the twitter content
    /// </summary>
    public class ReadContentController : TwitterController
    {
        /// <summary>
        /// Method to read the tweets for an account
        /// </summary>
        /// <param name="numberofRecords">Number of records to read</param>
        /// <param name="screenname">Acount whose tweets need to be read</param>
        /// <returns>Tweet content details</returns>
        public string ReadTwitterContent(int numberofRecords, string screenname)
        {
            /* 
             We are going to perform the following 2 steps here:
             1. Autheticate the request and get access token
             2. Using the access token and GET request, read the twitter contents 
            */

            string response = null; // Variable to store the tweets
            // Make an authentication call and get the access token
            string accesstoken = base.Authentication();

            if(!string.IsNullOrWhiteSpace(accesstoken))
            {
                // Region to get the tweets per the passed screen name           
                var tweetUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ReadTweetUrl"]; // Get the url required to get the tweets
                var gettweets = WebRequest.Create(tweetUrl + "?count=" + numberofRecords + "&screen_name=" + screenname) as HttpWebRequest;
                gettweets.Method = "GET";
                gettweets.Headers[HttpRequestHeader.Authorization] = "Bearer " + accesstoken;
                try
                {
                    // Best practice to use "using" as it disposes the object regardless of an error
                    using (var responseStream = gettweets.GetResponse().GetResponseStream())
                    {
                        // Read the stream to a string object
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            response = responseReader.ReadToEnd();
                        }
                    }
                }
                catch(Exception exc)
                {
                    throw exc;
                }
            }
            
            // Return the response to the caller
            return response;
        }
    }
}