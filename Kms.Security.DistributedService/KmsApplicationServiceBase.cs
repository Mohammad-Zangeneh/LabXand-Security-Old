using LabXand.DistributedServices;
using LabXand.DistributedServices.Wcf;
using LabXand.DomainLayer;
using LabXand.DomainLayer.Core;
using LabXand.Infrastructure.Data;
using LabXand.Logging.Core;


namespace Kms.Security.DistributedService
{
    public abstract class KmsApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier> : ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, ApiLogEntry>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
    {
        public KmsApplicationServiceBase(IUnitOfWork unitOfWork, TDomainService domainService, IEntityMapper<TDomainEntity, TDomainDto> mapper)
            : base(unitOfWork, domainService, mapper, new LogContext<ApiLogEntry>())
        {

        }
    }
}
