using Kms.Security.Common.Domain;

namespace Kms.Security.Identity
{
    public interface ISecurityConfigurationService
    {
        SecurityConfigurationDto GetSecurityConfiguration();
        SecurityConfigurationDto EditSecurityConfiguration(SecurityConfigurationDto dto);
        string GetTrackEntity();
    }
}
