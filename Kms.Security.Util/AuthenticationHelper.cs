using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public class AuthenticationHelper
    {
        public const string KeycloakCookieName = "KMS.keycloak_cookie";
        public const string CasCookieName = "KMS.cas_cookie";
        public static string Authorization => GetAuthenticationMode() != AuthenticationMode.Windows ? "Authorization" : "WindowsAuthorization";
        public static AuthenticationMode GetAuthenticationMode()
        {
            var authConfig = ConfigurationManager.AppSettings["AuthenticationMode"];
            if (string.IsNullOrEmpty(authConfig) || string.IsNullOrWhiteSpace(authConfig))
                return AuthenticationMode.Kms;
            var authMode = Enum.GetNames(typeof(AuthenticationMode)).FirstOrDefault(x => string.Compare(x, authConfig, true) == 0);
            return (AuthenticationMode)Enum.Parse(typeof(AuthenticationMode), authMode);
        }
        public static JwtSecurityToken DecodeJWT(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken);
            return jsonToken as JwtSecurityToken;
        }

        public static TimeSpan CalculateTokenExpireDuration(DateTimeOffset? issueUtc, DateTimeOffset? expireUtc) =>
            ConvertUtcToLocalDate(issueUtc).Subtract(ConvertUtcToLocalDate(expireUtc));

        private static DateTime ConvertUtcToLocalDate(DateTimeOffset? dateUtc) => DateTime.Parse(dateUtc.ToString());

    }
}
