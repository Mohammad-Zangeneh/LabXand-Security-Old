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
using Kms.Security.Api.App_Start.Ioc;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class OrganizationController : ApiController
    {
        IOrganizationService _organizaitonApplicationSrvice;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        private readonly IRedisCacheService _cacheManager;


        public OrganizationController(IOrganizationService organizaitonApplicationSrvice, IUserContextDetector<KmsUserContext> userContextDetector,
            IRedisCacheService cacheManager)
        {
            _organizaitonApplicationSrvice = organizaitonApplicationSrvice;
            _userContextDetector = userContextDetector;
            _cacheManager = cacheManager;
        }

        [Description("دریافت")]
        [HttpGet]
        [Route("api/organization/Get", Name = "GetOrganization")]
        public IList<OrganizationDto> Get()
        {
            string cacheKey = RedisKey.OrganizationDtoList + _userContextDetector.UserContext.OrganizationId.ToString();
            var dataFromRedis = _cacheManager.GetObject<List<OrganizationDto>>(cacheKey, RedisDbType.Organization);

            if (dataFromRedis == null)
            {
                dataFromRedis = _organizaitonApplicationSrvice.GetAll().OrderBy(o => o.SortingNumber).ToList();
                _cacheManager.SetObject(cacheKey, dataFromRedis, RedisDbType.Organization);
            }

            return dataFromRedis;
        }

        [Description("ذخیره")]
        [JwtAuthorize(Permission = "OrganizationManagement")]
        [HttpPost]
        [Route("api/organization/Save", Name = "organizationSave")]
        public Object Save(OrganizationDto organization)
        {
            _cacheManager.RemoveDatabase(RedisDbType.Organization);

            return _organizaitonApplicationSrvice.Save(organization);
        }

    }
}