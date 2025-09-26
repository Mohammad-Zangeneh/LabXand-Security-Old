using Kms.Security.Identity.Service.Contracts;
using Kms.Security.Jwt;
using Kms.Security.WebApi.RateLimit;
using System.ComponentModel;
using System.Web.Http;

namespace Kms.Security.Api.Controllers.Zar
{
    [RateLimit]
    public class ZarController : ApiController
    {
        public ZarController(IZarService zarService)
        {
            this.zarService = zarService;
        }

        private readonly IZarService zarService;

        [Description("دریافت")]
        [HttpGet]
        [Route("api/Zar/SyncData", Name = "SyncData")]
        [MachineAuthorize]
        public string SyncData()
        {
            zarService.SyncData();

            return "بروزرسانی جایگاه های سازمانی و کاربران با موفقیت انجام شد.";
        }
    }
}
