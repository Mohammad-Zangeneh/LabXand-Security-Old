using Kms.Security.Jwt;
using Kms.Security.WebApi.RateLimit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class MainController : ApiController
    {

        [Description("دریافت")]
        [HttpGet]
        [Route("api/Main/Get", Name = "MainGet")]
        [AllowAnonymous]
        [JwtAuthorizeAttribute]
        public IHttpActionResult MainGet( )//this is for check that user is login or not
        {
            var username = HttpContext.Current.Request.LogonUserIdentity.Name;
            var isAuth = HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated;

            return Ok(username);
        }

    }
}