using Kms.Security.Common.Domain;

namespace Kms.Security.Common.DataContract
{
    public interface ISecurityConfigurationReader
    {
        SecurityConfigurationDto GetSecurityConfiguration();
    }
}
