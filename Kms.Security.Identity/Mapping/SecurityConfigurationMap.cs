using Kms.Security.Common.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Kms.Security.Identity.Mapping
{
    public class SecurityConfigurationMap : EntityTypeConfiguration<SecurityConfiguration>
    {
        public SecurityConfigurationMap()
        {
            HasKey(x => x.Id);
        }
    }
}
