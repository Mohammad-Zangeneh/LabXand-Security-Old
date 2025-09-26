using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kms.Security.Common.Domain;
using Kms.Security.Util;
using LabXand.DistributedServices;
using Microsoft.Owin.Security;

namespace Kms.Security.Identity
{
    public class PermissionService : ServiceBase<Guid, Permission, PermissionDto>, IPermissionService
    {
        IJwtUtil _jwtUtil;
     
        public PermissionService(IEntityMapper<Permission, PermissionDto> mapper, IJwtUtil jwtUtil) : base(mapper)
        {
            _jwtUtil = jwtUtil;
            if (!jwtUtil.HasRestriction("SuperAdminPermisionManagement"))
                this.HasRestriction(p => p.PermissionType == Util.PermissionType.Normal);
        }

        public PermissionDto GetByCode(string code)
        {
            this.ClearRestriction();
            using(var dbContext = new ApplicationDbContext())
            {
                var domain = dbContext.Permissions.FirstOrDefault(p=>p.Code == code);
                if (domain == null)
                    return null;
                return _mapper.MapTo(domain);
            }
        }

        public IList<StatusDescription> GetPermissionTypeForCombo()
        {
            return Util.EnumHelper.Get<PermissionType>();
        }

        public Permission GetSpecificUserPermission(Guid userId , string permission)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var userHasPermission = dbContext.Set<ApplicationUser>().Where(t =>t.Id==userId &&( t.LabxandRoles.Any(p => p.Permissions.Any(l => l.Permission.Code == permission)) ||
                t.EnterprisePositionPosts.Any(g => g.Permissions.Any(l => l.Code == permission)))
                ).Select(t => t.Id).FirstOrDefault();
                if (userHasPermission == Guid.Empty)
                    return null;
                return dbContext.Permissions.FirstOrDefault(p => p.Code == permission);
              
            }
        }
    }
}
