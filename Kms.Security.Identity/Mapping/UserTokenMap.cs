using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    class UserTokenMap : EntityTypeConfiguration<UserToken>
    {
        public UserTokenMap()
        {
            this.HasRequired(p => p.OwnerUser).WithMany().HasForeignKey(p => p.OwnerUserId);
        }
    }
}
