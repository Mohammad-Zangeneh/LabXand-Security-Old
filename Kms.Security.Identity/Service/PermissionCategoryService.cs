using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabXand.DistributedServices;

namespace Kms.Security.Identity
{
    public class PermissionCategoryService : ServiceBase<Guid, PermissionCategory, PermissionCategoryDto>, IPermissionCategoryService
    {
        IJwtUtil _jwtUtil;
        public PermissionCategoryService(IEntityMapper<PermissionCategory, PermissionCategoryDto> mapper, IJwtUtil jwtUtil) : base(mapper)
        {
            _jwtUtil = jwtUtil;
            if (!jwtUtil.HasRestriction("SuperAdminPermisionManagement"))
                this.HasRestriction(p => p.PermissionType == Util.PermissionType.Normal);
        }
    }
}
