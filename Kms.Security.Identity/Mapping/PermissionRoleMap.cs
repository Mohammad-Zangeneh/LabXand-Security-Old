using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    class PermissionRoleMap : EntityTypeConfiguration<PermissionRole>
    {
        public PermissionRoleMap()
        {
            this.HasKey(p => new { p.PermissionId, p.RoleId });
            this.HasRequired(p => p.Role).WithMany().HasForeignKey(p => p.RoleId);
            this.HasRequired(p => p.Permission).WithMany().HasForeignKey(p => p.PermissionId);
        }
    }
}
