using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using LabXand.Infrastructure.Data.Redis;
using StructureMap;
using StructureMap.Web;
using System;

namespace Kms.Security.Jwt
{
    public class JwtIocRegistery : Registry
    {
        public JwtIocRegistery()
        {
            For<IUserProvider>().Use<UserProvider>();
            For<IAppJwtConfiguration>().Singleton().Use(() => AppJwtConfiguration.Config);
            For<IOAuthAuthorizationServerProvider>().Singleton().Use<AppOAuthProvider>();
            For<IAuthenticationTokenProvider>().Singleton().Use<RefreshTokenProvider>();
            For<ITokenStoreService>().HybridHttpOrThreadLocalScoped().Use<TokenStoreService>();
            For<ISecurityService>().HybridHttpOrThreadLocalScoped().Use<SecurityService>();
            For<IRedisCacheService>().Use<RedisCacheService>();
            Policies.SetAllProperties(setterConvention =>
              {
                  setterConvention.OfType<Func<ITokenStoreService>>();
              });
        }
    }
}
