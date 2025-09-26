using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Kms.Security.OpenId
{
    public class OpenIdOptions : OpenIdConnectAuthenticationOptions
    {
        public static string PersistentAuthType => "keycloak_cookies";
        public OpenIdOptions()
        {
            AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active;
            Authority = "https://sso.karafariniomid.ir/auth/realms/master/";
            ClientId = "kms-client-id";
            ClientSecret = "wctgLSoVLvi2x8cXGTSxZlHBohYSJCIu";
            //SignInAsAuthenticationType = persistentAuthType;
            ResponseType = "code";
            SaveTokens = true;
            Scope = "openid";
            RedirectUri = WebConfigurationManager.AppSettings["SecurityRoot"] + "/api/sso/KeycloakSignIn";
            RedeemCode = true;
            Notifications = new OpenIdConnectAuthenticationNotifications()
            {

                RedirectToIdentityProvider = async (context) =>
                {
                    context.ProtocolMessage.Parameters["code_challenge"] = "0KpkdgYxlnrYb9pJWCHhXQqirurQTPfX7McwyZ7drQQ";
                    context.ProtocolMessage.Parameters["code_challenge_method"] = "plain";
                },
                AuthorizationCodeReceived = async (context) =>
                {
                    context.TokenEndpointRequest.Parameters["code_verifier"] = "0KpkdgYxlnrYb9pJWCHhXQqirurQTPfX7McwyZ7drQQ";

                },
                TokenResponseReceived = async (responseToken) =>
                {
                    responseToken.Request.Headers.Add("Authorization", new[] { responseToken.TokenEndpointResponse.AccessToken });
                    responseToken.Request.Headers.Add("RefreshToken", new[] { responseToken.TokenEndpointResponse.RefreshToken });
                    responseToken.SkipToNextMiddleware();
                },
                SecurityTokenReceived = async (param) =>
                {
                },
                MessageReceived = async (request) =>
                {
                },
                SecurityTokenValidated = async (n) =>
                {
                }

            };

        }
    }
}
