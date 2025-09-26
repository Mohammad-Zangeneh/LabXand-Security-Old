using LabXand.DomainLayer;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace Kms.Security.Core
{

	public abstract class KmsSecurityEnabledDomainServiceBase<TDomain, TRepository, TIdentifier> : SecurityEnabledDomainServiceBase<TDomain, TRepository, TIdentifier, KmsUserContext> where TDomain : DomainEntityBase<TIdentifier> where TRepository : class, IRepository<TDomain, TIdentifier>
	{
		public KmsSecurityEnabledDomainServiceBase(TRepository repository, IUserContextDetector<KmsUserContext> userContextDetector)
			: base(repository, userContextDetector)
		{
		}
	}
}