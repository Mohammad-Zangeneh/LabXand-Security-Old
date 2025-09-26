using Kms.Security.Common.DataContract;
using Kms.Security.Identity.Service.Contracts;
using Kms.Security.Util;
using Kms.Security.WebApi.RateLimit;
using System.ComponentModel;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Description("تولید کلید")]
        [Route("api/auth/key", Name = "GenerateKey")]
        [HttpGet]
        public KeyDto GetGeneratedKeys()
        {
            var keys = _authService.GenerateKeys();
            return keys;
        }




    }
}
