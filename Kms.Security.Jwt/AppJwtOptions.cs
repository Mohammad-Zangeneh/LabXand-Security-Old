using Microsoft.Owin.Security.Jwt;
using System;
using System.Text;

namespace Kms.Security.Jwt
{

    public class AppJwtOptions : JwtBearerAuthenticationOptions
    {
        
        public AppJwtOptions(IAppJwtConfiguration config)
        {
            AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active;
            
            AllowedAudiences = new[] { config.JwtAudience };
            IssuerSecurityKeyProviders = new[]
            {
                new SymmetricKeyIssuerSecurityKeyProvider(
                    issuer: config.JwtIssuer,
                    base64Key: Convert.ToBase64String(Encoding.UTF8.GetBytes(config.JwtKey)))
            };
        }
        
    }
}
