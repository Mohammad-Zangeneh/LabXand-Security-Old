using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Kms.Security.Util;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.Redis;
using Microsoft.Owin.Security.Provider;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Kms.Security.Jwt
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public Func<ITokenStoreService> TokenStoreService { set; get; }
        public string Permission { get; set; }
        private static AuthenticationMode AuthenticationMode => AuthenticationHelper.GetAuthenticationMode();
        private static IRedisCacheService redisCacheService;
        private UserClaimDto claims;
        public JwtAuthorizeAttribute()
        {
            redisCacheService = RedisFactory.CreateInstance();
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
                return;

            claims = GetClaimsFromContext(actionContext);
            if (claims == null || !IsTokenExist(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var username = claims.Username ?? throw new UnauthorizedAccessException();

            if (Permission != null)
            {
                Permission permission = GetSpecificUserPermissionWithUsername(username, Permission) ?? throw new UnauthorizedAccessException();

                bool hadSuperAdminPermission = (permission.PermissionType == PermissionType.SuperAdmin);

                if (hadSuperAdminPermission && !claims.IsSuperAdmin)
                    throw new UnauthorizedAccessException();
            }

            base.OnAuthorization(actionContext);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        public Permission GetSpecificUserPermission(Guid userId, string permission)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var userHasPermission = dbContext.Set<ApplicationUser>().Where(t => t.Id == userId && (t.LabxandRoles.Any(p => p.Permissions.Any(l => l.Permission.Code == permission)) ||
                t.EnterprisePositionPosts.Any(g => g.Permissions.Any(l => l.Code == permission)))
                ).Select(t => t.Id).FirstOrDefault();
                if (userHasPermission == Guid.Empty)
                    return null;
                return dbContext.Permissions.FirstOrDefault(p => p.Code == permission);
            }
        }
        private Permission GetSpecificUserPermissionWithUsername(string username, string permission)
            => UserManager.GetUserPermissions(UserManager.GetOnlyUserByUsername(username).Id).FirstOrDefault(x => x.Code == permission);
        private bool IsTokenExist(HttpActionContext actionContext)
        {
            var claimsIdentity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            if (IsKmsAuth())
            {
                var accessToken = claimsIdentity.Claims.FirstOrDefault(x => x.Type == LabxandClaimTypes.AccessToken)?.Value ?? actionContext.Request.Headers.Authorization?.Parameter;
                if (accessToken == null)
                    return false;

                if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrEmpty(accessToken) ||
                    !IsTokenValid(claimsIdentity) || accessToken.Equals("undefined", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            else if (IsKeycloakAuth())
            {
                var token = claimsIdentity.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.Authorization);
                if (token == null || string.IsNullOrEmpty(token.Value) || !IsTokenValid(claimsIdentity))
                    return false;
            }
            else if (IsCasAuth())
            {
                var ticket = claimsIdentity.Claims.FirstOrDefault(x => x.Type == CasClaimTypes.CasTicket);
                if (ticket == null || string.IsNullOrEmpty(ticket.Value) || !IsTicketValid(claimsIdentity))
                    return false;
            }
            else if (IsWindowsAuth())
                return true;
            else
                return false;

            return true;
        }


        private bool IsTokenValid(ClaimsIdentity claimsIdentity)
        {
            if (IsKmsAuth())
            {
                if (claims == null)
                    return false;

                var tokenFromRedis = redisCacheService.GetObject<string>(RedisKey.Token + claims.UserId, RedisDbType.User);
                var tokenFromClient = claims.AccessToken;

                if (tokenFromRedis == null)
                    return false;
                return string.Compare(tokenFromRedis, tokenFromClient, true) == 0;
            }
            else if (IsKeycloakAuth())
            {
                var accessTokenValidTo = claimsIdentity.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.AccessTokenExpirationTime).Value;
                var refreshTokenValidTo = claimsIdentity.Claims.FirstOrDefault(x => x.Type == KeycloakClaimTypes.RefreshTokenExpirationTime).Value;
                if (DateTime.Now > DateTime.Parse(accessTokenValidTo).AddHours(3).AddMinutes(10) &&
                    DateTime.Now > DateTime.Parse(refreshTokenValidTo).AddHours(3).AddMinutes(10))
                    return false;
            }

            return true;
        }

        private bool IsTicketValid(ClaimsIdentity claimsIdentity)
        {
            var ticketExpirationTime = claimsIdentity.Claims.FirstOrDefault(x => x.Type == CasClaimTypes.ExpirationTime);
            if (ticketExpirationTime == null || ticketExpirationTime.Value.IsNullOrEmpty())
                return false;

            if (DateTime.Now > Convert.ToDateTime(ticketExpirationTime.Value))
                return false;

            return true;
        }
        private string GetUsername(ClaimsIdentity claimsIdentity)
        {
            string username;

            if (!IsWindowsAuth())
                username = claimsIdentity.Claims.FirstOrDefault(u => u.Type == LabxandClaimTypes.UserName)?.Value;
            else
            {
                username = HttpContext.Current.User.Identity.Name;
                if (username.Contains("\\"))
                    username = username.Split('\\')[1];
            }
            return username;
        }

        private bool HasClaim(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
                return false;
            return true;
        }
        private UserClaimDto GetClaimsFromContext(HttpActionContext actionContext)
        {
            var claimsIdentity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            if (!HasClaim(claimsIdentity))
                return null;
            var username = GetUsername(claimsIdentity);
            if (IsWindowsAuth())
                return new UserClaimDto()
                {
                    UserId = null,
                    Username = username,
                    AccessToken = null,
                    IsSuperAdmin = UserManager.IsSuperAdmin(username)
                };
            return new UserClaimDto()
            {
                UserId = claimsIdentity.Claims.FirstOrDefault(u => u.Type == LabxandClaimTypes.UserId)?.Value,
                Username = username,
                AccessToken = claimsIdentity.Claims.FirstOrDefault(x => x.Type == LabxandClaimTypes.AccessToken)?.Value ?? actionContext.Request.Headers.Authorization?.Parameter,
                IsSuperAdmin = Convert.ToBoolean(claimsIdentity.Claims.FirstOrDefault(u => u.Type == LabxandClaimTypes.IsSuperAdmin)?.Value) 
            };
        }

        private static bool IsKeycloakAuth() => AuthenticationMode == AuthenticationMode.Keycloak;
        private static bool IsWindowsAuth() => AuthenticationMode == AuthenticationMode.Windows;
        private static bool IsKmsAuth() => AuthenticationMode == AuthenticationMode.Kms;
        private static bool IsCasAuth() => AuthenticationMode == AuthenticationMode.Cas;


    }

}
