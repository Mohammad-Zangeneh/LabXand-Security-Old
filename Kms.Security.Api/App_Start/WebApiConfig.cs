using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Jwt;
using LabXand.DistributedServices.WebApi;
using LabXand.Logging.Core;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace Kms.Security.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);


            var container = ObjectFactory.Current;
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator), new SmWebApiControllerActivator(container));

            config.Services.Replace(typeof(IFilterProvider), new SmWebApiFilterProvider(container));
            config.Services.Replace(typeof(IExceptionHandler), new LabXandApiExceptionHandler(ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>()));
        }
    }
}
