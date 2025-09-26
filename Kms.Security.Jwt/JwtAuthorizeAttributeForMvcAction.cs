using Kms.Security.Identity;
using Kms.Security.Util;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Web.Mvc;

namespace Kms.Security.Jwt
{

    public class JwtAuthorizeAttributeForMvcAction : AuthorizeAttribute
    {
        public Func<ITokenStoreService> TokenStoreService { set; get; }
        public string Permission { get; set; }
        //IEntityMapper<UserToken, UserTokenDto> mapper;
        private ITokenStoreService TokenService
        {
            get
            {
                return DependencyResolver.Current.GetService<ITokenStoreService>();
            }
        }
        public System.Security.Claims.ClaimsPrincipal ValidateBearerToken(AuthorizationContext context)
        {
            AppJwtConfiguration config = ConfigurationManager.GetSection("appJwtConfiguration") as AppJwtConfiguration;
         
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] securityKey = Encoding.ASCII.GetBytes(config.JwtKey);
            //this should come from a config file

            SecurityToken securityToken;

            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = config.JwtAudience,
                IssuerSigningToken = new BinarySecretSecurityToken(securityKey),
                ValidIssuer = config.JwtIssuer
            };

            var auth = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrWhiteSpace(auth) && auth.Contains("Bearer"))
            {
                var token = auth.Split(' ')[1];

                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            return null;

        }
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            //if (skipAuthorization(filterContext))
            //{
            //    return;
            //}
            var Authorizationtemp = filterContext.HttpContext.Request.Headers["Authorization"];
            if (Authorizationtemp == null)
            {
                // null token
               // filterContext.HttpContext.Response.StatusCode = 600;
                this.HandleUnauthorizedRequest(filterContext);
                return;
            }


            var accessToken = Authorizationtemp.Substring(7);
            if (string.IsNullOrWhiteSpace(accessToken) ||
                accessToken.Equals("undefined", StringComparison.OrdinalIgnoreCase))
            {
                // null token
                //filterContext.HttpContext.Response.StatusCode = 600;
                this.HandleUnauthorizedRequest(filterContext);

                return;
            }

            System.Security.Claims.ClaimsPrincipal claimsIdentity;

            try
            {
                claimsIdentity = ValidateBearerToken(filterContext);
                if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
                {
                    // this is not our issued token
                    //filterContext.HttpContext.Response.StatusCode = 600;
                    this.HandleUnauthorizedRequest(filterContext);
                    
                    return;
                }
            }
            catch 
            {
                this.HandleUnauthorizedRequest(filterContext);
                return;
            }

            var userId = new Guid(claimsIdentity.FindFirst(LabxandClaimTypes.UserId).Value);

            if (TokenService == null)
            {
                throw new NullReferenceException($"{nameof(TokenStoreService)} is null. Make sure ioc.Policies.SetAllProperties is configured and also IFilterProvider is replaced with SmWebApiFilterProvider.");
            }

            //if (TokenStoreService == null)
            //{
            //    throw new NullReferenceException($"{nameof(TokenStoreService)} is null. Make sure ioc.Policies.SetAllProperties is configured and also IFilterProvider is replaced with SmWebApiFilterProvider.");
            //}
            //remove comment
            if (!TokenService.IsValidToken(accessToken, userId))
            {
                // this is not our issued token
                //filterContext.HttpContext.Response.StatusCode = 600;
                this.HandleUnauthorizedRequest(filterContext);
                return;
            }
            var principal = claimsIdentity;
            if (Permission != null && !principal.HasClaim(x => x.Type == LabxandClaimTypes.Permissions && x.Value == Permission))
            {
                this.HandleUnauthorizedRequest(filterContext);
                return;
            }

            base.OnAuthorization(filterContext);

        }
    }

    internal interface IEntityMapper<T1, T2>
    {
    }
}