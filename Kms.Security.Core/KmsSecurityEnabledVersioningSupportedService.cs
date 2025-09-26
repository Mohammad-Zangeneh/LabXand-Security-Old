using LabXand.DomainLayer;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace Kms.Security.Core
{

    public abstract class KmsSecurityEnabledVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository> : SecurityEnabledVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository, KmsUserContext> where TDomain : DomainEntityBase<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion> where TRepository : class, IRepository<TDomain, TIdentifier>
    {
        public KmsSecurityEnabledVersioningSupportedService(TRepository repository, IUserContextDetector<KmsUserContext> userContextInitializer)
            : base(repository, userContextInitializer)
        {
        }
    }
}