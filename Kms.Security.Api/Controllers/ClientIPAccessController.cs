using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using Kms.Security.WebApi.RateLimit;
using LabXand.Core;
using LabXand.Logging.Core;
using LabXand.Security.Core;
using System.ComponentModel;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class ClientIPAccessController : ApiController
    {
        IClientIPAccessService _clientIPAccessService;
        private readonly ILogger _logger;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        public ClientIPAccessController(IClientIPAccessService clientIPAccessService,
            ILogger logger,
            IUserContextDetector<KmsUserContext> userContextDetector)
        {
            _clientIPAccessService = clientIPAccessService;
            _logger = logger;
            _userContextDetector = userContextDetector;
        }

        [Description("ذخیره آی‌پی")]
        [HttpPost]
        [Route("api/ClientIPAccess/Save")]
        [JwtAuthorizeAttribute(Permission = "RoleManagement")]
        public ClientIPAccessDto Save(ClientIPAccessDto domainDto)
        {
            var result = _clientIPAccessService.Save(domainDto);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += $" -> ای پی آدرس با مقدار [ {result.IpAddress} ] توسط کاربر : « {_userContextDetector.UserContext.UserName} » در لیست محدودیت قرار گرفت "; 
            return result;
        }

        [Description("دریافت برای گرید")]
        [HttpPost]
        [Route("api/ClientIPAccess/GetForGrid")]
        [JwtAuthorizeAttribute(Permission = "RoleManagement")]
        public object GetForGrid(SpecificationOfDataList<ClientIPAccessDto> specification)
        {
            var res = _clientIPAccessService.GetAllForGrid(specification.GetCriteria(), specification.PageIndex, specification.PageSize, specification.GetSortItem());
            return new
            {
                res.TotalCount,
                Results = res.Data
            };
        }

        [Description("حذف ای پی آدرس")]
        [HttpPost]
        [Route("api/ClientIPAccess/Delete")]
        [JwtAuthorizeAttribute(Permission = "RoleManagement")]
        public ClientIPAccessDto Delete(ClientIPAccessDto domainDto)
        {
            var result = _clientIPAccessService.Delete(domainDto);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += $" ->  ای پی آدرس با مقدار [ {result.IpAddress} ] توسط کاربر : « {_userContextDetector.UserContext.UserName} » از لیست محدودیت حذف شد  ";
            return result;
        }

    }
}