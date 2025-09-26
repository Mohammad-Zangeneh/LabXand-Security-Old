using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    [Description("دسترسی نقش")]
    public class PermissionRole
    {
        public PermissionRole()
        {

        }
        public PermissionRole(Guid roleId, Guid permissionId, Int16 value)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            Value = value;
            LastEdit = DateTime.Now;
        }
        public void SetRoleId(Guid roleId)
        {
            RoleId = roleId;
        }

        public void SetValue(Int16 value)
        {
            Value = value;
        }
        public Guid RoleId { get; protected set; }
        public LabxandRole Role { get; protected set; }
        public Permission Permission { get; protected set; }
        public Guid PermissionId { get; protected set; }
        public DateTime LastEdit { get; set; }
        public Int16 Value { get; protected set; }
        Permission innerPermission;
        public void SetPermission(Permission permission) => innerPermission = permission;

        public override string ToString()
        {
            return (innerPermission?.Title != null)?  $"مجوز : ' {innerPermission.Title} ' " : $"PermissionId : ' {innerPermission?.Id} ' " ;
        }

    }
}
