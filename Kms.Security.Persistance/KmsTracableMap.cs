using Kms.Security.Core;
using LabXand.Infrastructure.Data.EF;

namespace Kms.Security.Persistance
{
    public class KmsTracableMap<TTrable, TIdentifier> : DomainEntityMapBase<TTrable, TIdentifier>
        where TTrable : KmsTraceableEntity<TIdentifier>
    {
        protected KmsTracableMap() : base()
        {
            this.Property(t => t.TraceData.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.TraceData.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.TraceData.OwnerOrganizationId).HasColumnName("OwnerOrganizationId");
            this.Property(t => t.TraceData.OwnerMemberId).HasColumnName("OwnerMemberId");
            this.Property(t => t.TraceData.LastUpdateMemberId).HasColumnName("LastUpdateMemberId");
        }
    }
}
