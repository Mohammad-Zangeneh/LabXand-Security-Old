using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Kms.Security.Jwt;
using Kms.Security.Util;
using LabXand.DistributedServices;
using LabXand.Logging.Core;
using LabXand.Security.Core;
using Microsoft.Owin.Security;
using LabXand.Infrastructure.Data.Redis;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.Http;
using System.Security.Claims;
using System.Linq;
using LabXand.Extensions;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using System.Net.Http;
using Microsoft.Owin.Security.Cookies;
using System.Threading;
using Kms.Security.WebApi.RateLimit;
using System.IdentityModel.Selectors;
using System.Web.Http.Results;
using System.Data.Entity.Core;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class SsoController : ApiController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IKmsApplicationUserManager _userManager;
        private readonly IEntityMapper<ApplicationUser, MemberDto> _memberMapper;
        ITokenStoreService _tokenService;
        ILoginHistoryService _loginHistoryService;
        private readonly ILogger _logger;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        private readonly IRedisCacheService _redisCacheService;
        public SsoController(IKmsApplicationUserManager userManager,
                                 IApplicationSignInManager signInManager,
                                 IAuthenticationManager authenticationManager, IEntityMapper<ApplicationUser, MemberDto> memberMapper,
                                 ITokenStoreService tokenService,
                                  ILoginHistoryService loginHistoryService,
                                  ILogger logger,
                                 IUserContextDetector<KmsUserContext> userContextDetector,
                                 IRedisCacheService redisCacheService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _memberMapper = memberMapper;
            _tokenService = tokenService;
            _loginHistoryService = loginHistoryService;
            _logger = logger;
            _userContextDetector = userContextDetector;
            _redisCacheService = redisCacheService;
        }
        protected string GetSecretKey() => WebConfigurationManager.AppSettings["SecretKey"];


        [Description("فرآیند ورود کاربر از طریق Keycloak Sso")]
        [HttpPost]
        [Route("api/sso/KeycloakSignIn", Name = "KeycloakCallback")]
        public IHttpActionResult KeycloakSignInCallback()
        {
            var accessToken = HttpContext.Current.Request.Headers["Authorization"];
            var refreshToken = HttpContext.Current.Request.Headers["RefreshToken"];
            var keycloakUser = ExtractUserFromToken(accessToken);
            var claims = _userManager.KeycloakUserProccess(keycloakUser, GetSecretKey());
            claims = claims.Prepend(new Claim(KeycloakClaimTypes.RefreshToken, string.Format("{0}", refreshToken))).ToArray();
            claims = claims.Prepend(new Claim(KeycloakClaimTypes.Authorization, string.Format("Bearer {0}", accessToken))).ToArray();

            var decodedAccessToken = AuthenticationHelper.DecodeJWT(accessToken.Replace("Bearer ", ""));
            var decodedRefreshToken = AuthenticationHelper.DecodeJWT(refreshToken);
            claims = claims.Append(new Claim(KeycloakClaimTypes.AccessTokenExpirationTime, string.Format("{0}", decodedAccessToken.ValidTo))).ToArray();
            claims = claims.Append(new Claim(KeycloakClaimTypes.RefreshTokenExpirationTime, string.Format("{0}", decodedRefreshToken.ValidTo))).ToArray();


            var identity = new ClaimsIdentity(claims, "keycloak_cookies");
            _authenticationManager.SignIn(identity);

            return Redirect(WebConfigurationManager.AppSettings["PortalRoot"]);
        }

        [Description("دریافت اطلاعات کاربر هنگام ورود از طریق Keycloak")]
        [HttpGet]
        [Route("api/sso/SignIn", Name = "KeylcoakSignInInfo")]
        [JwtAuthorize]
        public object SignInInfo()
        {
            var claims = (HttpContext.Current.Request.RequestContext.HttpContext.User.Identity as ClaimsIdentity).Claims;
            var accessToken = claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.Authorization).Value;
            var refreshToken = claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.RefreshToken).Value;
            var accessTokenExp = claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.AccessTokenExpirationTime).Value;
            var refreshTokenExp = claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.RefreshTokenExpirationTime).Value;
            var userId = claims.FirstOrDefault(x => x.Type == LabxandClaimTypes.UserId).Value;
            return new
            {
                UserInfo = _userManager.GetMember(userId.ToGuid()),
                Permissions = UserManager.GetUserPermissions(userId.ToGuid()),
                TokenInfo = new
                {
                    AccessToken = accessToken.Replace("Bearer ", ""),
                    RefreshToken = refreshToken,
                    AccessValidTo = accessTokenExp,
                    RefreshValidTo = refreshTokenExp
                },
            };
        }

        [Description("فرآیند خروج کاربر از  Keycloak Sso")]
        [HttpGet]
        [Route("api/sso/KeycloakSignOut", Name = "SignOut")]
        //[JwtAuthorize]
        public IHttpActionResult KeycloakSignOut()
        {
            //var claims = (HttpContext.Current.Request.RequestContext.HttpContext.User.Identity as ClaimsIdentity).Claims;
            //var accessToken = claims?.FirstOrDefault(x => x.Type == KeycloakClaimTypes.Authorization)?.Value;
            //var refreshToken = claims?.FirstOrDefault(x => x.Type == KeycloakClaimTypes.RefreshToken)?.Value;
            //if (claims == null || accessToken == null || refreshToken == null)
            //    throw new UnauthorizedAccessException();


            _authenticationManager.SignOut("keycloak_cookies");
            return Redirect("https://sso.karafariniomid.ir/auth/realms/master/protocol/openid-connect/logout");
        }


        private MemberDto ExtractUserFromToken(string jwtToken)
        {
            var decodedToken = AuthenticationHelper.DecodeJWT(jwtToken);
            return new MemberDto
            {
                Id = (Guid)(decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.UserId)?.Value.ToGuid()),
                UserName = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.Username)?.Value,
                FullName = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.FullName)?.Value,
                FirstName = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.FirstName)?.Value,
                LastName = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.LastName)?.Value,
                Email = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.Email)?.Value,
                //EmailVerified = decodedToken.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.EmailVerified)?.Value,
            };
        }

        [Description("فرآیند ورود کاربر از طریق Cas")]
        [HttpGet]
        [Route("api/sso/CasSignIn", Name = "CasCallBack")]
        public IHttpActionResult CasSignIn(string ticket)
        {
            if (ticket == null)
                return BadRequest("خطا،سرور تیکت ارسال نکرد.");
            string url = WebConfigurationManager.AppSettings["CasUrl"] + "&ticket=" + ticket;
            try
            {
                var stringResult = HttpUtil.ParsResponseBody(HttpUtil.PerformHttpGet(url));
                //var stringResult = "<cas:serviceResponse xmlns:cas='http://www.yale.edu/tp/cas'>\r\n\t<cas:authenticationSuccess>\r\n\t\t<cas:user>NodakWebAdmin</cas:user>\r\n\r\n\t\t<cas:attributes>\r\n\t\t\t<cas:credentialType>UsernamePasswordCredential</cas:credentialType>\r\n\t\t\t<cas:clientIpAddress>46.100.53.222</cas:clientIpAddress>\r\n\t\t\t<cas:isFromNewLogin>true</cas:isFromNewLogin>\r\n\t\t\t<cas:authenticationDate>2023-10-02T13:00:41.466237699Z</cas:authenticationDate>\r\n\t\t\t<cas:authenticationMethod>QueryDatabaseAuthenticationHandler</cas:authenticationMethod>\r\n\t\t\t<cas:successfulAuthenticationHandlers>QueryDatabaseAuthenticationHandler\r\n\t\t\t</cas:successfulAuthenticationHandlers>\r\n\t\t\t<cas:serverIpAddress>213.233.184.80</cas:serverIpAddress>\r\n\t\t\t<cas:userAgent>Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)\r\n\t\t\t\tChrome/117.0.0.0 Safari/537.36</cas:userAgent>\r\n\t\t\t<cas:longTermAuthenticationRequestTokenUsed>false</cas:longTermAuthenticationRequestTokenUsed>\r\n\t\t</cas:attributes>\r\n\t</cas:authenticationSuccess>\r\n</cas:serviceResponse>";
                stringResult = stringResult.Replace("cas:", "");
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(stringResult);

                string usernamePath = "serviceResponse/authenticationSuccess/user";
                var usernameNode = xmlDoc.SelectNodes(usernamePath);
                var username = usernameNode[0].InnerText;

                //get claims
                var user = _userManager.GetMemberWithRole(username);
                if (user == null)
                {
                    new FileLogger().Log("C:\\temp", "CasSignIn-" + Guid.NewGuid(), "txt", "the username '" + username + "' was not found in Kms App.\n" + stringResult);

                    return BadRequest("کاربر مورد نظر یافت نشد.");
                }

                var claims = _userManager.CasUserProccess(username);
                claims = claims.Prepend(new Claim(CasClaimTypes.CasTicket, string.Format("{0}", ticket))).ToArray();
                claims = claims.Prepend(new Claim(CasClaimTypes.ExpirationTime, string.Format("{0}", DateTime.Now.AddDays(2).ToString()))).ToArray();

                //creat cookie
                var identity = new ClaimsIdentity(claims, "cas_cookie");
                _authenticationManager.SignIn(
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                    }, identity);
            }
            catch (Exception ex)
            {
                new FileLogger().Log("C:\\temp", "CasSignIn-" + Guid.NewGuid(), "txt", HttpUtil.ParsResponseBody(HttpUtil.PerformHttpGet(url)));
                return BadRequest("خطا: " + ex.Message);
            }

            return Redirect(WebConfigurationManager.AppSettings["PortalRoot"]);
        }

        [Description("دریافت اطلاعات کاربر در حالت Cas")]
        [HttpGet]
        [Route("api/sso/GetCasSignInInfo", Name = "CasSignIn")]
        [JwtAuthorize]
        public object GetCasSignInInfo()
        {
            var claims = (HttpContext.Current.Request.RequestContext.HttpContext.User.Identity as ClaimsIdentity).Claims;
            var ticket = claims.FirstOrDefault(x => x.Type == CasClaimTypes.CasTicket).Value;
            var expirationTime = claims.FirstOrDefault(x => x.Type == CasClaimTypes.ExpirationTime).Value;
            var userId = claims.FirstOrDefault(x => x.Type == LabxandClaimTypes.UserId).Value;

            return new
            {
                UserInfo = _userManager.GetMember(userId.ToGuid()),
                Permissions = UserManager.GetUserPermissions(userId.ToGuid()),
                TokenInfo = new
                {
                    Ticket = ticket,
                    ExpirationTime = expirationTime,
                },
            };
        }

    }
}
