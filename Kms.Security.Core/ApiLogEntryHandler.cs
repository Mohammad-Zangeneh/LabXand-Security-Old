using LabXand.Logging.Core;
using LabXand.Security.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Kms.Security.Core
{
    public static class ApiLogEntryHandler
    {

        public static ApiLogEntry CreateApiLogEntry(IUserContextDetector<KmsUserContext> userContextDetector, HttpRequestMessage httpRequestMessage = null, string description = null, string actionName = null)
        {
            var context = ((HttpContextBase)httpRequestMessage?.Properties["MS_HttpContext"]);
            var routeData = httpRequestMessage?.GetRouteData();
            var config = GlobalConfiguration.Configuration;
            var controllerSelector = new DefaultHttpControllerSelector(config);
            HttpControllerDescriptor descriptor = null;
            // descriptor here will contain information about the controller to which the request will be routed. If it's null (i.e. controller not found), it will throw an exception
            if (httpRequestMessage != null)
            {
                descriptor = controllerSelector.SelectController(httpRequestMessage);
            }
            //var controllerContext = new HttpControllerContext(config, routeData, httpRequestMessage)
            //{
            //    ControllerDescriptor = descriptor
            //};
            //var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);

            return new ApiLogEntry()
            {
                Application = ConfigurationManager.AppSettings["ApplicationName"],
                ControllerName = descriptor?.ControllerName,
                ActionName = actionName ?? "نامشخص",
                User = userContextDetector?.UserContext?.UserName,
                Machine = Environment.MachineName,
                RequestContentType = context?.Request.ContentType,
                RequestRouteTemplate = routeData?.Route.RouteTemplate,
                RequestIpAddress = context?.Request.UserHostAddress,
                RequestMethod = httpRequestMessage?.Method.Method,
                RequestHeaders = SerializeHeaders(httpRequestMessage?.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = httpRequestMessage?.RequestUri.ToString(),
                Description = description,
                ResponseStatusCode = 200
            };
        }

        public static ApiLogEntry CreateApiLogEntry(string controllerName, string actionName, string user = null, string description = null, int responseStatusCode = 200)
        {
            return new ApiLogEntry()
            {
                Application = ConfigurationManager.AppSettings["ApplicationName"],
                ControllerName = controllerName ?? "نامشخص",
                ActionName = actionName ?? "نامشخص",
                User = user ?? null,
                Machine = Environment.MachineName,
                RequestIpAddress = GetIP(),
                RequestTimestamp = DateTime.Now,
                RequestUri = GetIP(),
                Description = description,
                ResponseStatusCode = responseStatusCode
            };
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            if (headers == null)
            {
                return null;
            }
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
        private static string GetIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }
    }
}
