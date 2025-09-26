using Kms.Security.Common.DataContract;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;

namespace Kms.Security.Jwt
{
    public class AppOAuthOptions : OAuthAuthorizationServerOptions
    {
        public AppOAuthOptions(IAppJwtConfiguration configuration, IOAuthAuthorizationServerProvider _oAuthAuthorizationServerProvider,
            IAuthenticationTokenProvider _authenticationTokenProvider, ISecurityConfigurationContext securityConfigurationContext)
        {
            AllowInsecureHttp = true; // TODO: Buy an SSL certificate!
            TokenEndpointPath = new PathString(configuration.TokenPath);
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(securityConfigurationContext.Instance.ExpirationMinutes);
            AccessTokenFormat = new AppJwtWriterFormat(this, configuration);
            ApplicationCanDisplayErrors = false;
            Provider = _oAuthAuthorizationServerProvider;
            RefreshTokenProvider = _authenticationTokenProvider;
        }
    }
}
