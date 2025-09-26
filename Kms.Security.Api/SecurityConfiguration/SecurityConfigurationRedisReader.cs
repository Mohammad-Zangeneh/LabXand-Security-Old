using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Util;
using LabXand.Infrastructure.Data.Redis;

namespace Kms.Security.Api
{
    public class SecurityConfigurationRedisReader : ISecurityConfigurationReader
    {
        private readonly ISecurityConfigurationService securityConfigurationService;
        private readonly IRedisCacheService redisCacheService;

        public SecurityConfigurationRedisReader(ISecurityConfigurationService securityConfigurationService, IRedisCacheService redisCacheService)
        {
            this.securityConfigurationService = securityConfigurationService;
            this.redisCacheService = redisCacheService;
        }

        public SecurityConfigurationDto GetSecurityConfiguration()
        {
            var dataFromRedis = redisCacheService.GetObject<SecurityConfigurationDto>(RedisKey.SecurityConfiguration, RedisDbType.SecurityConfiguration);

            if (dataFromRedis == null)
            {
                dataFromRedis = securityConfigurationService.GetSecurityConfiguration();
                redisCacheService.SetObject(RedisKey.SecurityConfiguration, dataFromRedis, RedisDbType.SecurityConfiguration);
            }

            return dataFromRedis;
        }
    }
}
