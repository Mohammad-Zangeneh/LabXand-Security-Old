using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Kms.Security.Identity.Mapping
{
    public class MemberMap: EntityTypeConfiguration<ApplicationUser>
    {
        public MemberMap()
        {
            this.HasMany(p => p.EnterprisePositionPosts).WithMany().Map(p => {
                p.MapLeftKey("UserId");
                p.MapRightKey("EnterprisePositionPost");
                p.ToTable("UserEnterprisePostionPost");
            });
            this.HasMany(p => p.LabxandRoles).WithMany().Map(p =>
            {
                p.MapLeftKey("UserId");
                p.MapRightKey("LabxandRoleId");
                p.ToTable("UserRoleTable");
            });
            this.HasRequired(p => p.Organization).WithMany().HasForeignKey(q => q.OrganizationId);
            this.HasOptional(p => p.EnterprisePosition).WithMany().HasForeignKey(p => p.EnterprisePositionId);
            this.Property(p => p.FirstName).IsRequired();
            this.Property(p => p.LastName).IsRequired();
            this.Property(p => p.PasswordHash).IsRequired();
            this.Property(p => p.UserName).IsRequired();
        }
    }
}
