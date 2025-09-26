
using LabXand.DomainLayer;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace Kms.Security.Core
{
    public abstract class KmsSecurityEnabledVersioningSupportedServiceForTrableEntity<TDomain, TIdentifier, TVersion, TRepository> : KmsSecurityEnabledVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository> where TDomain : KmsTraceableEntity<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion> where TRepository : class, IRepository<TDomain, TIdentifier>
    {
        public KmsSecurityEnabledVersioningSupportedServiceForTrableEntity(TRepository repository, IUserContextDetector<KmsUserContext> userContextInitializer)
            : base(repository, userContextInitializer)
        {
        }

        protected override void CustomOperation(TDomain domain, TDomain latestVersion)
        {
            if (latestVersion != null)
            {
                ((TracableDomainEntityBase<TIdentifier, KmsTraceData, KmsUserContext>)(object)domain).TraceData.SetFullTraceData(((TracableDomainEntityBase<TIdentifier, KmsTraceData, KmsUserContext>)(object)latestVersion).TraceData);
                ((TracableDomainEntityBase<TIdentifier, KmsTraceData, KmsUserContext>)(object)domain).TraceData.SetLastModificationData(UserContext);
            }
        }
    }
}