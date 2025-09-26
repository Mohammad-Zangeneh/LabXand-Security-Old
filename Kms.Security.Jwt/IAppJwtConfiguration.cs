using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Jwt
{
    public interface IAppJwtConfiguration
    {
        int ExpirationMinutes { get; set; }
        string JwtAudience { get; set; }
        string JwtIssuer { get; set; }
        string JwtKey { get; set; }
        int RefreshTokenExpirationMinutes { get; set; }
        string TokenPath { get; set; }
    }
}
