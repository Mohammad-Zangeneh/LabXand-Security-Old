using Kms.Security.Common.Domain;

namespace Kms.Security.Common.DataContract
{
    public class SecurityConfigurationContext : ISecurityConfigurationContext
    {
        private readonly ISecurityConfigurationReader _securityConfigurationReader;
        private SecurityConfigurationDto securityConfigurationDto;
        public SecurityConfigurationContext(ISecurityConfigurationReader securityConfigurationReader)
        {
            _securityConfigurationReader = securityConfigurationReader;
        }

        public SecurityConfigurationDto Instance
        {
            get
            {
                if (securityConfigurationDto == null)
                    securityConfigurationDto = _securityConfigurationReader.GetSecurityConfiguration();
                return securityConfigurationDto;
            }
        }

        public void Set(SecurityConfigurationDto dto)
        {
            this.securityConfigurationDto = dto;
        }
    }
}
