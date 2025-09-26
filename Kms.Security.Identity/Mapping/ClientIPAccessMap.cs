using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    public class ClientIPAccessMap : EntityTypeConfiguration<ClientIPAccess>
    {
        public ClientIPAccessMap()
        {
            this.HasKey(t => t.Id);
        }
    }
}
