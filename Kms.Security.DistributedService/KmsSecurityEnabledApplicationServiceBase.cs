using Kms.Security.Core;
using LabXand.DistributedServices;
using LabXand.DistributedServices.Wcf;
using LabXand.DomainLayer.Core;
using LabXand.Infrastructure.Data;
using LabXand.Logging.Core;
using LabXand.Security.Core;

namespace Kms.Security.DistributedService
{
    public abstract class KmsSecurityEnabledApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier>
        : SecurityEnabledServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, KmsUserContext, ApiLogEntry>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
    {
        public KmsSecurityEnabledApplicationServiceBase(IUnitOfWork unitOfWork, TDomainService domainService, IEntityMapper<TDomainEntity, TDomainDto> mapper, IUserContextDetector<KmsUserContext> userContextDetector)
            : base(unitOfWork, domainService, mapper, userContextDetector, new LogContext<ApiLogEntry>())
        {

        }

    }
}
