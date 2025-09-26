using Kms.Security.Common.Domain;
using Kms.Security.Util;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IJwtUtil
    {
        bool HasRestriction(string permissionCode);
    }
    public class JwtUtil : IJwtUtil
    {
        IAuthenticationManager _authenticationManager;
        public JwtUtil(IAuthenticationManager authenticationManager)
        {
           
            _authenticationManager = authenticationManager;
        }

        public Permission GetSpecificUserPermission(Guid userId, string permission)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var userHasPermission = dbContext.Set<ApplicationUser>().Where(t =>t.Id==userId &&( t.LabxandRoles.Any(p => p.Permissions.Any(l => l.Permission.Code == permission)) ||
                t.EnterprisePositionPosts.Any(g => g.Permissions.Any(l => l.Code == permission)))
                ).Select(t => t.Id).FirstOrDefault();
                if (userHasPermission == Guid.Empty)
                {
                    return null;
                }
                return dbContext.Permissions.FirstOrDefault(p => p.Code == permission);

            }
        }
        public bool HasRestriction(string permissionCode)
        {
            var isSuperAdmin = Convert.ToBoolean(_authenticationManager.User.Claims.FirstOrDefault(r => r.Type == LabxandClaimTypes.IsSuperAdmin).Value);
            var userid =new Guid( _authenticationManager.User.Claims.FirstOrDefault(r => r.Type == LabxandClaimTypes.UserId).Value);

            var permission = GetSpecificUserPermission(userid, permissionCode);

            if (permission == null)
                return false;
            if ((permission.PermissionType == PermissionType.SuperAdmin && isSuperAdmin != true))
                return false;

            return true;
        }
    }
}
