using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    public class PermissionCategoryMap :EntityTypeConfiguration<PermissionCategory>
    {
        public PermissionCategoryMap()
        {
            this.HasKey(p => p.Id);
            this.HasOptional(p => p.Parent).WithMany().HasForeignKey(p => p.ParentId);
            this.HasOptional(p => p.Company).WithMany().HasForeignKey(p => p.CompanyId);
            this.HasMany(p => p.Permissions).WithRequired(p => p.PermissionCategory).HasForeignKey(p => p.PermissionCategoryId);
        }
    }
}
