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
    [RateLimit]
    public class EnterprisePositionPostController : ApiController
    {
        IEnterprisePositionPostService _service;
        public EnterprisePositionPostController(IEnterprisePositionPostService service)
        {
            _service = service;
        }

        [Description("دریافت")]
        [HttpGet]
       [JwtAuthorize(Permission = "EnterprisePositionPostManagement")]
        [Route("api/EnterprisePositionPost/Get", Name = "EnterprisePositionPostGet")]
        public IList<EnterprisePositionPostDto> Get( )
        {
            return _service.GetAll();
        }

        [Description("ذخیره")]
        [HttpPost]
        [JwtAuthorize(Permission = "EnterprisePositionPostManagement")]
        [Route("api/EnterprisePositionPost/Save", Name = "EnterprisePositionPostSave")]
        public EnterprisePositionPostDto Save(EnterprisePositionPostDto domainDto)
        {
            return _service.Save(domainDto);
        }

        [Description("دریافت برای گرید")]
        [JwtAuthorize(Permission = "EnterprisePositionPostManagement")]
        [HttpPost]
        [Route("api/EnterprisePositionPost/GetForGrid", Name = "EnterprisePositionPostGetForGrid")]
        public object GetForGrid(SpecificationOfDataList<EnterprisePositionPostDto> specification)
        {
            var result = _service.GetOnePageOfList(specification.GetCriteria(), specification.PageIndex, specification.PageSize, specification.GetSortItem());
            return new
            {
                TotalCount = result.TotalCount,
                Results = result.Data
            };
        }


    }
}