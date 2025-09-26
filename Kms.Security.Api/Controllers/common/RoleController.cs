using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using LabXand.Core;
using LabXand.Logging.Core;
using LabXand.Infrastructure.Data.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Http;
using Kms.Security.Util;
using Kms.Security.WebApi.RateLimit;

namespace Kms.Security.Api.Controllers.common
{
    public class GuidClass
    {
        public Guid Id { get; set; }
    }
    [RateLimit]
    public class RoleController : ApiController
    {
        IRoleService _roleService;
        private readonly IRedisCacheService _redisCacheService;
        public RoleController(IRoleService roleService, IRedisCacheService redisCacheService)
        {
            _roleService = roleService;
            _redisCacheService = redisCacheService;
        }

        [Description("ذخیره")]
        [HttpPost]
        [Route("api/Role/Save", Name = "SaveRole")]
        [JwtAuthorize(Permission = "RoleManagement")]
        public RoleDto Save(RoleDto domainDto)
        {
            var result = _roleService.Save(domainDto);
            var role = _roleService.GetTrackEntity();
            _redisCacheService.RemoveDatabase(RedisDbType.Permission);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += $" ->  تغییرات  سطح دسترسی نقش : " + role;
            return result;
        }

        [Description("دریافت")]
        [HttpGet]
        [Route("api/Role/Get", Name = "GetRole")]
        [JwtAuthorize(Permission = "RoleManagement")]
        public IList<RoleDto> Get()
        {
            return _roleService.GetAll();
        }

        [Description("حذف نقش")]
        [HttpPost]
        [Route("api/Role/Delete")]
        [JwtAuthorize(Permission = "RoleManagement")]
        public void DeleteRole(GuidClass id)
        {
            _roleService.Delete(id.Id);
        }

        [Description("دریافت نقش برای گرید")]
        [HttpPost]
        [Route("api/role/GetForGrid", Name = "GetRoleForGrid")]
        [JwtAuthorizeAttribute(Permission = "RoleManagement")]
        public object GetRoleForGrid(SpecificationOfDataList<RoleDto> role)
        {
            var result = _roleService.GetOnePageOfList(role.GetCriteria(), role.PageIndex, role.PageSize, role.GetSortItem());
            return new
            {
                TotalCount = result.TotalCount,
                Results = result.Data
            };
        }
    }
}