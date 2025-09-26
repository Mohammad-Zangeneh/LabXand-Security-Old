

// Kms.Security.DistributedService.KmsTraceDataEntityMapper<TIdentitifier,TSource,Tdestination>
using Kms.Security.Core;
using LabXand.DistributedServices;
using LabXand.DomainLayer.Core;

namespace Kms.Security.DistributedService
{
    public abstract class KmsTraceDataEntityMapper<TIdentitifier, TSource, Tdestination> : IEntityMapper<TSource, Tdestination> where TSource : KmsTraceableEntity<TIdentitifier> where Tdestination : KmsTraceDataDto
    {
        public TSource CreateFrom(Tdestination destination)
        {
            TSource val = CreateFromEntity(destination);
            val.SetTraceData(new KmsTraceData(destination.CreateDate, destination.LastUpdateDate, destination.OwnerOrganizationId, destination.WriterOrganizationId, destination.WriterMemberId, destination.OwnerMemberId, destination.WriterUserName, destination.LastUpdateMemberId, destination.OwnerEnterprisePositionId, destination.WriterEnterprisePositionId));
            return val;
        }

        public Tdestination MapTo(TSource source)
        {
            Tdestination val = MapToEntity(source);
            if (source.TraceData == null)
            {
                return val;
            }
            val.CreateDate = source.TraceData.CreateDate;
            val.LastUpdateDate = source.TraceData.LastUpdateDate;
            val.OwnerOrganizationId = source.TraceData.OwnerOrganizationId;
            val.WriterOrganizationId = source.TraceData.WriterOrganizationId;
            val.WriterMemberId = source.TraceData.WriterMemberId;
            val.OwnerMemberId = source.TraceData.OwnerMemberId;
            val.WriterUserName = source.TraceData.WriterUserName;
            val.LastUpdateMemberId = source.TraceData.LastUpdateMemberId;
            val.OwnerEnterprisePositionId = source.TraceData.OwnerEnterprisePositionId;
            val.WriterEnterprisePositionId = source.TraceData.WriterEnterprisePositionId;
            return val;
        }

        public abstract TSource CreateFromEntity(Tdestination domainDto);

        public abstract Tdestination MapToEntity(TSource domain);
    }
}