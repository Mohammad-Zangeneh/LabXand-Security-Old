using Microsoft.Owin.Security.Cookies;
using System.Configuration;

namespace Kms.Security.OpenId
{
    public class KmsCookieOptions : CookieAuthenticationOptions
    {
        public const string KMS_COOKIE_NAME = "kms_cookie";
        public const string AUTHENTICATION_TYPE = "KmsLoginWithJWTAndCookie";
        public KmsCookieOptions()
        {
            AuthenticationType = AUTHENTICATION_TYPE;
            CookieHttpOnly = true;
            CookieName = KMS_COOKIE_NAME;
            CookieSameSite = Microsoft.Owin.SameSiteMode.Strict;
            if (ConfigurationManager.AppSettings["PortalRoot"].Contains("https"))
                CookieSecure = CookieSecureOption.Always;
            else
                CookieSecure = CookieSecureOption.Never;
            CookiePath = "/";
            //ExpireTimeSpan
            //SlidingExpiration
        }
    }
}
