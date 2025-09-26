using Kms.Security.Common.Domain;

namespace Kms.Security.Common.DataContract
{
    public interface ISecurityConfigurationContext
    {
        SecurityConfigurationDto Instance { get; }
        void Set(SecurityConfigurationDto securityConfiguration);
    }
}
