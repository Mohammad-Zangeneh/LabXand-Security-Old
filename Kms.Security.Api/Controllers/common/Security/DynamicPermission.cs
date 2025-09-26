using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.WebApi.RateLimit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api.Controllers.common.Security
{
    [RateLimit]
    public class DynamicPermissionController : ApiController
    {
        [Description("دریافت")]
        [HttpGet]
        [Route("api/DynamicPermission/Get", Name = "DynamicPermissionGet")]
        public List<DynamicPermission> Get( )
        {
            return LabXandApiControllers.GetLsitOfApiControllers();
        }

    }
}