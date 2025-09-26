using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public static class KeycloakClaimTypes
    {
        public const string UserId = "sub";
        public const string Username = "preferred_username";
        public const string FullName = "name";
        public const string FirstName = "given_name";
        public const string LastName = "family_name";
        public const string Email = "email";
        public const string EmailVerified = "email_verified";
        public const string Authorization = "Authorization";
        public const string RefreshToken = "RefreshToken";
        public const string AccessTokenExpirationTime = "AccesssTokenExpirationTime";
        public const string RefreshTokenExpirationTime = "RefreshTokenExpirationTime";
    }
    
    public static class CasClaimTypes
    {
        public const string CasTicket = "CasTicket";
        public const string ExpirationTime = "ExpirationTime";
    }
}
