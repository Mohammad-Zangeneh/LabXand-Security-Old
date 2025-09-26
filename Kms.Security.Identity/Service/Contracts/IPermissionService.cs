using Kms.Security.Common.Domain;
using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IPermissionService
    {
        IList<PermissionDto> GetAll();

        PermissionDto Save(PermissionDto domain);

        IList<StatusDescription> GetPermissionTypeForCombo();
        PermissionDto GetByCode(string code);
        Permission GetSpecificUserPermission(Guid userId, string permission);
    }
}
