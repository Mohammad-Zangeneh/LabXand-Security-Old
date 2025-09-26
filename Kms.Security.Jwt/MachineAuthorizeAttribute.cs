using Kms.Common.Util;
using Kms.Security.Common.Domain;
using Kms.Security.Identity.Service;
using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using AuthenticationMode = Kms.Security.Util.AuthenticationMode;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
namespace Kms.Security.Jwt
{
    public class MachineAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }
        private static AuthenticationMode AuthenticationMode => AuthenticationHelper.GetAuthenticationMode();
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string secretKey = GetSecretKey(actionContext);

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrWhiteSpace(secretKey))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            if (!IsSecretKeyValid(secretKey))
                throw new UnauthorizedAccessException();

            if (Permission != null)
            {
                var username = GetUsername(actionContext) ?? throw new UnauthorizedAccessException();
                Permission permission = GetSpecificUserPermissionWithUsername(username, Permission) ?? throw new UnauthorizedAccessException();
                bool hadSuperAdminPermission = (permission.PermissionType == PermissionType.SuperAdmin);

                if (hadSuperAdminPermission && !IsSuperAdmin(username))
                    throw new UnauthorizedAccessException();
            }
        }

        private static string GetSecretKey(HttpActionContext actionContext)
        {
            var secretKey = actionContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(secretKey))
                secretKey = actionContext.Request.Headers.GetValues(AuthenticationHelper.Authorization).FirstOrDefault();

            return secretKey;
        }

        private bool IsSecretKeyValid(string secretKey) => string.Compare(secretKey, WebConfigValues.SecretKey, true) == 0;
        private Permission GetSpecificUserPermissionWithUsername(string username, string permission)
            => UserManager.GetUserPermissions(UserManager.GetOnlyUserByUsername(username).Id).FirstOrDefault(x => x.Code == permission);
        private bool IsSuperAdmin(string username) => UserManager.IsSuperAdmin(username);
        private string GetUsername(HttpActionContext actionContext)
        {
            string username;

            IEnumerable<string> headerList;
            actionContext.Request.Headers.TryGetValues("Username", out headerList);
            username = headerList.FirstOrDefault();

            if (AuthenticationMode == AuthenticationMode.Windows)
                if (username.Contains("\\"))
                    username = username.Split('\\')[1];

            return username;
        }

    }
}
