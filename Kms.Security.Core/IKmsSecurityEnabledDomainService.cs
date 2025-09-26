using LabXand.DomainLayer.Core;

namespace Kms.Security.Core
{
    public interface IKmsSecurityEnabledDomainService<TDomain, TIdentifier> : ISecurityEnabledDomainService<TDomain, TIdentifier, KmsUserContext>, IDomainService<TDomain, TIdentifier>, IReadOnlyDomainService<TDomain, TIdentifier>, ISecurityEnabledService<KmsUserContext> where TDomain : DomainEntityBase<TIdentifier>
    {
    }
}