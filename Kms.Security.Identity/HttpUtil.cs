using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kms.Security.Identity
{
    /// <summary>
    /// A helper utility class to facilitate outbound HTTP GET and POST request
    /// </summary>
    /// <author>Scott Holodak</author>
    public static class HttpUtil
    {
        /// <summary>
        /// Executes an HTTP GET request against the Url specified, returning the 
        /// entire response body in string form.
        /// </summary>
        /// <param name="url">The URL to request</param>
        public static HttpWebResponse PerformHttpGet(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Executes an HTTP POST against the Url specified with the supplied post data, 
        /// returning the entire response body in string form.
        /// </summary>
        /// <param name="url">The URL to post to</param>
        /// <param name="postData">The x-www-form-urlencoded data to post to the URL</param>        
        public static HttpWebResponse PerformHttpPost(string url, string postData,
            string contentType = "Application/Json",
            List<KeyValuePair<string, string>> headers = null,
            Cookie cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);
            if (AuthenticationHelper.GetAuthenticationMode() == AuthenticationMode.Windows)
                request.Credentials = CredentialCache.DefaultCredentials;

            if (cookie != null)
            {
                if (request.CookieContainer == null)
                    request.CookieContainer = new CookieContainer();

                request.CookieContainer.Add(cookie);
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(postData);
            }

            return (HttpWebResponse)request.GetResponse();
        }

        public static string ParsResponseBody(HttpWebResponse response)
        {
            string responseBody = null;
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(responseStream))
                    {
                        responseBody = responseReader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }
    }
}
