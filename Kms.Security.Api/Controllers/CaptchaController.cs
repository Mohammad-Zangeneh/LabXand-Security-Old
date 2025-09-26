using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.WebApi.RateLimit;
using System.ComponentModel;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class CaptchaController : ApiController 
    {
        private readonly ICaptchaService _captchaService;

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        [Description("دریافت کپچا")]
        [HttpGet]
        [Route("api/Captcha/get", Name = "GetCaptcha")]
        public CaptchaDto Get() => _captchaService.Get();
    }
}
