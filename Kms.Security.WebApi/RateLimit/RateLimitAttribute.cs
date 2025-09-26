using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web;
using LabXand.Infrastructure.Data.Redis;
using System.Web.Http.Filters;
using Kms.Security.Util;
using System.Globalization;
using LabXand.Logging.Core;
using Kms.Security.Identity;
using System.Web.Http.Dependencies;

namespace Kms.Security.WebApi.RateLimit
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private IRedisCacheService Cache
        {
            get
            {
                return RedisFactory.CreateInstance();
            }
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string cacheKey = GetKey(filterContext);
            if (!string.IsNullOrEmpty(cacheKey))
            {
                var result = Checking(cacheKey);
                switch (result)
                {
                    case HttpStatusCode.Continue:
                        break;
                    case HttpStatusCode.Accepted:
                        break;
                    case HttpStatusCode.RequestTimeout:
                        var response = new HttpResponseMessage
                        {
                            StatusCode = (HttpStatusCode)429,
                            ReasonPhrase = "Too Many Requests",
                            Content = new StringContent(string.Format(CultureInfo.InvariantCulture, "Rate limit reached. Reset in {0} seconds.", (RateLimitConfig.PeriodTime + RateLimitConfig.BlockTime)))
                        };

                        response.Headers.Add("Retry-After", (RateLimitConfig.PeriodTime + RateLimitConfig.BlockTime).ToString(CultureInfo.InvariantCulture));
                        filterContext.Response = response;
                        break;
                    default:
                        break;
                }
                return;
            }
        }

        HttpStatusCode Checking(string cacheKey)
        {
            RequestIpHistoryModel ipHistoryModel = Cache.GetObject<RequestIpHistoryModel>(cacheKey, RedisDbType.RateLimit);

            if (ipHistoryModel is null)
            {
                ipHistoryModel = new RequestIpHistoryModel()
                {
                    FirstApprovedRequest = DateTime.UtcNow,
                    NumberOfApprovedRequest = 1,
                    ReleaseTime = DateTime.MinValue,
                    IpAddress = HttpContext.Current.Request.UserHostAddress
                };
                var setResult = Cache.SetObject(cacheKey, ipHistoryModel, RedisDbType.RateLimit, TimeSpan.FromSeconds(RateLimitConfig.PeriodTime));
                return setResult ? HttpStatusCode.Continue : HttpStatusCode.Accepted;
            }
            else
            {
                if (DateTime.UtcNow < ipHistoryModel.ReleaseTime)
                {
                    return HttpStatusCode.RequestTimeout;
                }
                else
                {
                    if ((DateTime.UtcNow - ipHistoryModel.FirstApprovedRequest).TotalSeconds <= RateLimitConfig.PeriodTime)
                    {
                        if (ipHistoryModel.NumberOfApprovedRequest < RateLimitConfig.MaxRequestPerPeriod)
                        {
                            ipHistoryModel.NumberOfApprovedRequest++;
                            Cache.RemoveKey(cacheKey, RedisDbType.RateLimit);
                            var setResult = Cache.SetObject(cacheKey, ipHistoryModel, RedisDbType.RateLimit, TimeSpan.FromSeconds(RateLimitConfig.PeriodTime));
                            return setResult ? HttpStatusCode.Continue : HttpStatusCode.Accepted;
                        }
                        else
                        {
                            ipHistoryModel.Lock(RateLimitConfig.BlockTime);
                            Cache.RemoveKey(cacheKey, RedisDbType.RateLimit);
                            var setResult = Cache.SetObject(cacheKey, ipHistoryModel, RedisDbType.RateLimit, TimeSpan.FromSeconds(RateLimitConfig.PeriodTime + RateLimitConfig.BlockTime));
                            return setResult ? HttpStatusCode.RequestTimeout : HttpStatusCode.Accepted;
                        }
                    }
                    else
                    {
                        ipHistoryModel.FirstApprovedRequest = DateTime.UtcNow;
                        ipHistoryModel.NumberOfApprovedRequest = 1;
                        ipHistoryModel.ReleaseTime = DateTime.MinValue;
                        Cache.RemoveKey(cacheKey, RedisDbType.RateLimit);
                        var setResult = Cache.SetObject(cacheKey, ipHistoryModel, RedisDbType.RateLimit, TimeSpan.FromSeconds(RateLimitConfig.PeriodTime));
                        return setResult ? HttpStatusCode.Continue : HttpStatusCode.Accepted;
                    }
                }
            }
        }

        string GetKey(HttpActionContext filterContext)
        {
            var key = string.Format("request_id_{0}-{1}-{2}",
               filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
               filterContext.ActionDescriptor.ActionName,
               HttpContext.Current.Request.UserHostAddress
               );

            return key;
        }
    }
}
