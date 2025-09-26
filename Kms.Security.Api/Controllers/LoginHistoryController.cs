using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using Kms.Security.WebApi.RateLimit;
using LabXand.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    public class GuidClass
    {
        public Guid Id { get; set; }
    }
    [RateLimit]
    public class LoginHistoryController : ApiController
    {
        ILoginHistoryService _loginHistoryService;
        public LoginHistoryController(ILoginHistoryService loginHistoryService)
        {

            _loginHistoryService = loginHistoryService;
        }

        [Description("دریافت تمامی تاریخچه")]
        [HttpPost]
        [Route("api/LoginHistory/GetAllHistory")]
        [JwtAuthorizeAttribute]
        public Object GetAllHistory(SpecificationOfDataList<LoginHistoryDto> specification)
        {
            var res = _loginHistoryService.GetAllHistoryForGrid(specification.GetCriteria(), specification.PageIndex, specification.PageSize, specification.GetSortItem());
            return new
            {
                TotalCount = res.TotalCount,
                Results = res.Data
            };
        }
    }
}