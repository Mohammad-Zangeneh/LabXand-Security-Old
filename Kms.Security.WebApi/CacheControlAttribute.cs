using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace Kms.Security.WebApi
{
    public class CacheControlAttribute : ActionFilterAttribute
    {
        public int MaxAgeSeconds { get; set; }
        public bool IsPublic { get; set; }

        public CacheControlAttribute(int maxAgeSeconds, bool isPublic)
        {
            MaxAgeSeconds = maxAgeSeconds;
            IsPublic = isPublic;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    Public = IsPublic,
                    Private = !IsPublic,
                    MaxAge = TimeSpan.FromSeconds(MaxAgeSeconds)
                };
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
