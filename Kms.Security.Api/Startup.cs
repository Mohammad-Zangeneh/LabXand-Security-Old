using Microsoft.Owin;
using Owin;
using Kms.Security.Api.App_Start.Ioc;
using Microsoft.Owin.Cors;
using Kms.Security.Jwt;
using Microsoft.Owin.Security;
using Kms.Security.Util;
using Kms.Security.OpenId;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Configuration;
using Microsoft.Owin.Security.Cookies;
using System;

[assembly: OwinStartup(typeof(Kms.Security.Api.Startup))]

namespace Kms.Security.Api
{
    public class Startup
    {
        public static AppOAuthOptions OAuthServerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(GetCorsOption());

            if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Kms)
            {
                app.UseCookieAuthentication(ObjectFactory.Current.GetInstance<KmsCookieOptions>());
                OAuthServerOptions = ObjectFactory.Current.GetInstance<AppOAuthOptions>();
                app.UseOAuthAuthorizationServer(OAuthServerOptions);
                app.UseJwtBearerAuthentication(ObjectFactory.Current.GetInstance<AppJwtOptions>());
            }

            else if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Keycloak)
            {
                app.SetDefaultSignInAsAuthenticationType(OpenIdOptions.PersistentAuthType);
                app.UseCookieAuthentication(ObjectFactory.Current.GetInstance<OpenId.CookieOptions>());
                app.UseOpenIdConnectAuthentication(ObjectFactory.Current.GetInstance<OpenIdOptions>());
            }
            else if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Cas)
            {
                app.SetDefaultSignInAsAuthenticationType("cas_cookie");
                app.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    AuthenticationType = "cas_cookie",
                    CookieHttpOnly = false,
                    CookieName = "KMS.cas_cookie",
                    CookieSameSite = SameSiteMode.None,
                    CookieSecure = CookieSecureOption.Always,
                    CookiePath = "/",
                    ExpireTimeSpan = TimeSpan.FromDays(2)
                });

            }
            else if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Windows)
            {
                OAuthServerOptions = ObjectFactory.Current.GetInstance<AppOAuthOptions>();
                app.UseOAuthAuthorizationServer(OAuthServerOptions);
            }


        }
        private CorsOptions GetCorsOption()
        {
            return new CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider()
                {
                    PolicyResolver = ctx =>
                    {
                        var policy = new CorsPolicy();
                        policy.Origins.Add(ConfigurationManager.AppSettings["ServiceRoot"]);
                        policy.Origins.Add(ConfigurationManager.AppSettings["PortalRoot"]);
                        policy.Origins.Add(ConfigurationManager.AppSettings["ReportRoot"]);
                        policy.Origins.Add(ConfigurationManager.AppSettings["WorkFlowApiUrl"]);
                        policy.AllowAnyHeader = true;
                        policy.SupportsCredentials = true;
                        policy.Methods.Add("GET");
                        policy.Methods.Add("POST");
                        policy.Methods.Add("OPTIONS");
                        return Task.FromResult(policy);
                    }
                }
            };
        }
    }
}
