using Kms.Security.Common.Domain;
using LabXand.Infrastructure.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Api.RedisCache
{
    public static class CacheManager
    {
        private static readonly IRedisCacheService _redisCacheService;

        #region OrganizationDto
        public static List<OrganizationDto> GetOrganizationDtoListFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.GetObject<List<OrganizationDto>>(key, dbNumber);
        }

        public static bool SetOrganizationDtoListToRedis(this IRedisCacheService _redisCacheService, string key, List<OrganizationDto> value, int dbNumber)
        {
            return _redisCacheService.SetObject<List<OrganizationDto>>(key, value, dbNumber);
        }

        public static bool RemoveOrganizationDtoListFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.RemoveKey(key, dbNumber);
        }
        #endregion

        #region EnterprisePositionDto
        public static List<EnterprisePositionDto> GetEnterprisePositionDtoListFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.GetObject<List<EnterprisePositionDto>>(key, dbNumber);
        }

        public static bool SetEnterprisePositionDtoListToRedis(this IRedisCacheService _redisCacheService, string key, List<EnterprisePositionDto> value, int dbNumber)
        {
            return _redisCacheService.SetObject<List<EnterprisePositionDto>>(key, value, dbNumber);
        }

        public static bool RemoveEnterprisePositionDtoListFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.RemoveKey(key, dbNumber);
        }
        #endregion

        #region SecurityConfigurationDto
        public static SecurityConfigurationDto GetSecurityConfigurationFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.GetObject<SecurityConfigurationDto>(key, dbNumber);
        }

        public static bool SetSecurityConfigurationToRedis(this IRedisCacheService _redisCacheService, string key, SecurityConfigurationDto value, int dbNumber)
        {
            return _redisCacheService.SetObject(key, value, dbNumber);
        }

        public static bool RemoveSecurityConfigurationDtoFromRedis(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.RemoveKey(key, dbNumber);
        }
        #endregion
        public static bool Remove(this IRedisCacheService _redisCacheService, string key, int dbNumber)
        {
            return _redisCacheService.RemoveKey(key, dbNumber);
        }
    }
}
