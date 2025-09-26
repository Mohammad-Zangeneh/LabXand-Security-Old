using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using Newtonsoft.Json;
using LabXand.Infrastructure.Data.Redis;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Web.Http;
using Kms.Security.Util;
using LabXand.Logging.Core;
using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.WebApi.RateLimit;
using System.Collections.Generic;
using System.Web;
using Kms.Common.Util;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class SecurityConfigurationController : ApiController
    {
        private readonly ISecurityConfigurationService securityConfigurationService;
        private readonly IRedisCacheService redisCacheService;
        private readonly ISecurityConfigurationContext securityConfigurationContext;

        public SecurityConfigurationController(ISecurityConfigurationService securityConfigurationService, IRedisCacheService redisCacheService, ISecurityConfigurationContext securityConfigurationContext)
        {
            this.securityConfigurationService = securityConfigurationService;
            this.redisCacheService = redisCacheService;
            this.securityConfigurationContext = securityConfigurationContext;
        }

        [Description("دریافت تنظیمات امنیتی")]
        [HttpGet]
        [MachineAuthorize]
        [Route("api/securityConfiguration/GetForBack", Name = "GetSecurityConfigurationForBack")]
        public SecurityConfigurationDto GetSecurityConfigurationForBack()
        {
            return securityConfigurationService.GetSecurityConfiguration();
        }
        
        [Description("دریافت تنظیمات امنیتی")]
        [HttpGet]
        [JwtAuthorize(Permission = "SecurityConfiguration")]
        [Route("api/securityConfiguration", Name = "GetSecurityConfiguration")]
        public SecurityConfigurationDto GetSecurityConfiguration()
        {
            return securityConfigurationService.GetSecurityConfiguration();
        }

        [Description("ویرایش تنظیمات امنیتی")]
        [HttpPost]
        [JwtAuthorize(Permission = "SecurityConfiguration")]
        [Route("api/securityConfiguration", Name = "EditSecurityConfiguration")]
        public SecurityConfigurationDto EditSecurityConfiguration(SecurityConfigurationDto dto)
        {
            var result = securityConfigurationService.EditSecurityConfiguration(dto);
            Startup.OAuthServerOptions.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(dto.ExpirationMinutes);
            securityConfigurationContext.Set(dto);
            //redisCacheService.RemoveKey(RedisKey.SecurityConfiguration, RedisDbType.SecurityConfiguration);
            redisCacheService.SetObject(RedisKey.SecurityConfiguration,dto ,RedisDbType.SecurityConfiguration);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += $" ->   " + securityConfigurationService.GetTrackEntity();
            try
            {
                var headers = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(AuthenticationHelper.Authorization, WebConfigValues.SecretKey) };
                HttpUtil.PerformHttpPost(ConfigurationManager.AppSettings["ServiceRoot"] + "/api/SecurityConfiguration", JsonConvert.SerializeObject(result), "Application/Json", headers);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
