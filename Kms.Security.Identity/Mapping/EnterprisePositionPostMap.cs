using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    public class EnterprisePositionPostMap:EntityTypeConfiguration<EnterprisePositionPost>
    {
        public EnterprisePositionPostMap()
        {
            this.HasKey(p => p.Id);
            this.HasRequired(p => p.EnterprisePosition).WithMany().HasForeignKey(p => p.EnterprisePositionId);
            this.HasMany(p => p.Permissions).WithMany().Map(p => {
                p.MapLeftKey("PermisionId");
                p.MapRightKey("EnterprisePositionPostId");
                p.ToTable("EnterprisePositionPostPermission");
            });
        }
    }
}
