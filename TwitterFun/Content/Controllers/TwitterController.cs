using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace TwitterFun.Controllers
{
    /// <summary>
    /// Abstract class to have the common set of methods
    /// </summary>
    public abstract class TwitterController
    {
        /// <summary>
        /// Static variable to store the access token so we shouldn't make the call with every refresh
        /// </summary>
        public static string _accesstoken = null; 

        /// <summary>
        /// Method used for authentication purpose only
        /// </summary>
        /// <returns></returns>
        public string Authentication()
        {
            /* Access token is needed for a valid (OAuth) twitter authentication, and
                #1. We can either pass token directly OR
                #2. Get them programmatically while application runs (best practice)
             * we will be using the 2nd approach here
            */

            // If the access token is yet to get 
            // (Out of scope handling: If the token gets expired when can handle within a login screen or make a direct call )
            if (string.IsNullOrEmpty(_accesstoken))
            {
                // Get the url, reuired for authentication, from web config
                var authUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["AuthUrl"];
                if (string.IsNullOrWhiteSpace(authUrl))
                {
                    throw new Exception("No URL provided for authentication");
                }

                // Get the credentials
                var consumerKey = System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"];
                var consumerSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"];
                if (string.IsNullOrWhiteSpace(consumerKey) || string.IsNullOrWhiteSpace(consumerSecret))
                {
                    throw new Exception("No Consumer key provided for authentication");
                }
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(consumerKey + ":" + consumerSecret)); // Base64 encoded string

                // Create the post request to get the access token            
                var postrequest = GetPostRequestInstance(authUrl, credentials, "grant_type=client_credentials");
                                    
                try
                {
                    // Get the response stream
                    using (var responseStream = postrequest.GetResponse().GetResponseStream())
                    {
                        // Read the stream
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            _accesstoken = responseReader.ReadToEnd();
                        }
                    }
                    _accesstoken = ParseResponseString(_accesstoken);
                }
                catch
                {
                    throw new Exception("Error while getting the access token");
                }                
            }
            
            // Return the token to the caller
            return _accesstoken;
        }
                
        /// <summary>
        /// Creates a post request instance which will be used to get the response stream
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="authHeader">Credentials need for authentication</param>
        /// <param name="requestbodyType">Body type of the request to create</param>
        /// <returns></returns>
        public HttpWebRequest GetPostRequestInstance(string url, string authHeader, string requestbodyType)
        {
            var postrequest = WebRequest.Create(url) as HttpWebRequest;
            postrequest.Method = "POST";
            postrequest.ContentType = "application/x-www-form-urlencoded";
            postrequest.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeader;
            var requestBody = Encoding.UTF8.GetBytes(requestbodyType);
            postrequest.ContentLength = requestBody.Length;

            // Create and fill the request stream
            using (var requestStream = postrequest.GetRequestStream())
            {
                requestStream.Write(requestBody, 0, requestBody.Length);
            }

            return postrequest;
        }

        /// <summary>
        /// Parses the response string
        /// </summary>
        /// <param name="response">String to be parsed</param>
        /// <returns></returns>
        private string ParseResponseString(string response)
        {
            return response.Substring(response.IndexOf("access_token\":\"") + "access_token\":\"".Length, response.IndexOf("\"}")
                                        - (response.IndexOf("access_token\":\"") + "access_token\":\"".Length));
        }
    }
}