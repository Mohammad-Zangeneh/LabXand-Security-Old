using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Jwt;
using Kms.Security.Util;
using LabXand.Core.ExceptionManagement;
using LabXand.DistributedServices.WebApi;
using LabXand.Logging.Core;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Kms.Security.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new SecurityLogHandler(ObjectFactory.Current.GetInstance<ILogger>(), ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>()));
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new LabXandControllerActivator(ObjectFactory.Current));
            ExceptionHandlerFactory.RegisterExceptionHandler(DefaultExceptionHandlers.Get);
            ExceptionHandlerFactory.RegisterExceptionHandler(new ViolationSecurityPolicyExceptionHandler());
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            SetHttpHeader();
            SetCorsHeader(sender);
        }

        private void SetHttpHeader()
        {
            SetXForwardedHeader();

            //Prevent Clickjacking with iframe
            Response.Headers.Add("X-Frame-Options", "DENY");
            Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        private void SetXForwardedHeader()
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress) && !string.IsNullOrWhiteSpace(ipAddress))
                Response.Headers.Add("X_FORWARDED_FOR", ipAddress);
            else
                Response.Headers.Add("X_FORWARDED_FOR", Request.ServerVariables["REMOTE_ADDR"]);
        }
        private void SetCorsHeader(object sender)
        {
            if (AuthenticationHelper.GetAuthenticationMode() == AuthenticationMode.Windows)
            {
                string httpOrigin = Request.Params["HTTP_ORIGIN"];
                if (httpOrigin == null) httpOrigin = "*";
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", httpOrigin);
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,PUT,POST,Delete,Options");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, Accept,Authorization,dataType");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");

                if (Request.HttpMethod == "OPTIONS")
                {
                    HttpContext.Current.Response.StatusCode = 200;
                    var httpApplication = sender as HttpApplication;
                    httpApplication.CompleteRequest();
                }
            }
        }
    }
}
