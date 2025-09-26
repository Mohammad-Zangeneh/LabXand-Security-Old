using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public enum PermissionType
    {
        [Description("دسترسی های عادی")]
        Normal,
        [Description("دسترسی ها سوپر ادمین")]
        SuperAdmin = 1
    }

    public enum ForceLogoutType
    {
        BlockedIP,
        ChangePassword,
        ChangeRolePermissions,
        ChangeUserRole,
        BlockUser,
        ChangeSecurityConfiguration,
        TokenExpirationMinute
    }

}
