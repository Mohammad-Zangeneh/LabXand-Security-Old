using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.OpenId
{
    public class CookieOptions : CookieAuthenticationOptions
    {
        public CookieOptions()
        {
            AuthenticationType = OpenIdOptions.PersistentAuthType;
            CookieHttpOnly = false;
            CookieName = "KMS.keycloak_cookie";
            CookieSameSite = Microsoft.Owin.SameSiteMode.None;
            CookieSecure = CookieSecureOption.Always;
            //CookieDomain = "localhost";
            CookiePath = "/";
        }
    }
}
