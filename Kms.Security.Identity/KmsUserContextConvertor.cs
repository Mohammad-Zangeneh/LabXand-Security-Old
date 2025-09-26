using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using Kms.Security.Core;

namespace Kms.Security.Identity
{
    public class KmsUserContextConvertor
    {

        private static string GetValueFromClaimsByType(IEnumerable<Claim> claims, string type)
        {
            Claim claim = claims.FirstOrDefault((Claim p) => p.Type == type);
            return (claim != null) ? claim.Value : string.Empty;
        }

        private static List<string> GetValuesFromClaimsByType(IEnumerable<Claim> claims, string type)
        {
            return (from p in claims
                    where p.Type == type
                    select p.Value)?.ToList();
        }

        public static KmsUserContext Convert(IEnumerable<Claim> claims)
        {
            KmsUserContext kmsUserContext = new KmsUserContext();

            string userId = string.Empty;
            if (ConfigurationManager.AppSettings["AuthenticationMode"] == "Windows")
            {
                string userName = GetValueFromClaimsByType(claims, ClaimTypes.Name);
                var user = KmsSecurityHelper.GetUserByUsername(userName);
                kmsUserContext.MemberId = user.Id;
                kmsUserContext.FirstName = user.FirstName;
                kmsUserContext.LastName = user.LastName;
                kmsUserContext.UserName = user.UserName;
                kmsUserContext.Posts = user.EnterprisePositionPosts.Select(x => x.Id.ToString()).ToList();// GetValuesFromClaimsByType(claims, "EnterprisePositionTile");
                kmsUserContext.OrganizationId = user.OrganizationId ?? Guid.Empty;// new Guid(GetValueFromClaimsByType(claims, "OrganizationId"));
                kmsUserContext.OrganizationTitle = user.Organization.Name;// GetValueFromClaimsByType(claims, "OrganizationId");
                kmsUserContext.AuthorizedOperations = KmsSecurityHelper.GetUserPermissions(user.Id).Select(x => x.Code).ToList();// GetValuesFromClaimsByType(claims, "Permissions");
                if (user.EnterprisePositionId.HasValue && user.EnterprisePositionId.Value.ToString() != "")
                    kmsUserContext.EnterprisePositionId = user.EnterprisePositionId.Value;

                kmsUserContext.IsSuperAdmin = user.IsSuperAdmin;// bool.Parse(GetValueFromClaimsByType(claims, "IsSuperAdmin"));
            }

            else
            {
                userId = GetValueFromClaimsByType(claims, "UserId");
                if (!string.IsNullOrEmpty(userId))
                {
                    kmsUserContext.MemberId = new Guid(userId);
                    kmsUserContext.FirstName = GetValueFromClaimsByType(claims, "FirstName");
                    kmsUserContext.LastName = GetValueFromClaimsByType(claims, "LastName");
                    kmsUserContext.UserName = (GetValueFromClaimsByType(claims, "UserName"));
                    kmsUserContext.Posts = GetValuesFromClaimsByType(claims, "EnterprisePositionTile");
                    kmsUserContext.OrganizationId = new Guid(GetValueFromClaimsByType(claims, "OrganizationId"));
                    kmsUserContext.OrganizationTitle = GetValueFromClaimsByType(claims, "OrganizationId");
                    kmsUserContext.AuthorizedOperations = GetValuesFromClaimsByType(claims, "Permissions");
                    string valueFromClaimsByType2 = GetValueFromClaimsByType(claims, "EnterprisePositionId");
                    if (valueFromClaimsByType2 != "")
                    {
                        kmsUserContext.EnterprisePositionId = new Guid(valueFromClaimsByType2);
                    }
                    kmsUserContext.IsSuperAdmin = bool.Parse(GetValueFromClaimsByType(claims, "IsSuperAdmin"));
                    return kmsUserContext;
                }
            }

            return kmsUserContext;
        }
        public static KmsUserContext Convert(string username)
        {
            KmsUserContext kmsUserContext = new KmsUserContext();

            string userId = string.Empty;
            var user = KmsSecurityHelper.GetUserByUsername(username);
            kmsUserContext.MemberId = user.Id;
            kmsUserContext.FirstName = user.FirstName;
            kmsUserContext.LastName = user.LastName;
            kmsUserContext.UserName = user.UserName;
            kmsUserContext.Posts = user.EnterprisePositionPosts.Select(x => x.Id.ToString()).ToList();
            kmsUserContext.OrganizationId = user.OrganizationId ?? Guid.Empty;
            kmsUserContext.OrganizationTitle = user.Organization.Name;
            kmsUserContext.AuthorizedOperations = KmsSecurityHelper.GetUserPermissions(user.Id).Select(x => x.Code).ToList();
            if (user.EnterprisePositionId.HasValue && user.EnterprisePositionId.Value.ToString() != "")
                kmsUserContext.EnterprisePositionId = user.EnterprisePositionId.Value;

            kmsUserContext.IsSuperAdmin = user.IsSuperAdmin;

            return kmsUserContext;
        }
    }
}