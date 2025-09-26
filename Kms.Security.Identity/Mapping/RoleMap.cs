using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping

{
    public class RoleMap:EntityTypeConfiguration<LabxandRole>
    {
        public RoleMap()
        {
            this.HasMany(p => p.Permissions).WithRequired(p => p.Role).HasForeignKey(p => p.RoleId).WillCascadeOnDelete();
            this.HasOptional(p => p.Company).WithMany().HasForeignKey(p => p.CompanyId);
        }
    }
}
