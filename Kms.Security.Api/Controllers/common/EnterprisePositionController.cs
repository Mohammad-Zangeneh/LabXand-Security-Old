using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using LabXand.Security.Core;
using LabXand.Infrastructure.Data.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kms.Security.Util;
using Kms.Security.WebApi.RateLimit;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class EnterprisePositionController : ApiController
    {
        IEnterprisePositionService _enterprisePositionApplicationService;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        private readonly IRedisCacheService _cacheManager;

        public EnterprisePositionController(IEnterprisePositionService enterprisePositionApplicationService, IRedisCacheService cacheManager,
            IUserContextDetector<KmsUserContext> userContextDetector)
        {
            _enterprisePositionApplicationService = enterprisePositionApplicationService;
            _cacheManager = cacheManager;
            _userContextDetector = userContextDetector;
        }

        [Description("ذخیره")]
        [HttpPost]
        [Route("api/enterprisePosition/Save", Name = "saveEnterprisePosition")]
        [JwtAuthorizeAttribute(Permission = "EnterprisePositionManagement")]
        public EnterprisePositionDto Save(EnterprisePositionDto domainDto)
        {
            _cacheManager.RemoveDatabase(RedisDbType.Organization);
            _cacheManager.RemoveDatabase(RedisDbType.EnterprisePosition);

            return _enterprisePositionApplicationService.Save(domainDto);
        }

        [Description("دریافت")]
        [HttpGet]
        [Route("api/enterprisePosition/Get", Name = "GetAllEnterprisePosition")]
        public IList<EnterprisePositionDto> Get()
        {
            string cacheKey = RedisKey.EnterprisePositionDtoList + _userContextDetector.UserContext.OrganizationId.ToString();

            var dataFromRedis = _cacheManager.GetObject<List<EnterprisePositionDto>>(cacheKey, RedisDbType.EnterprisePosition);

            if (dataFromRedis == null)
            {
                dataFromRedis = (List<EnterprisePositionDto>)_enterprisePositionApplicationService.GetAll();
                _cacheManager.SetObject(cacheKey, dataFromRedis, RedisDbType.EnterprisePosition);
            }

            return dataFromRedis;
        }
    }
}