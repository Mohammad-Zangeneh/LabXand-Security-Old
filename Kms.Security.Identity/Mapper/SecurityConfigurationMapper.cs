using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using LabXand.DistributedServices;

namespace Kms.Security.Identity
{
    public class SecurityConfigurationMapper : IEntityMapper<SecurityConfiguration, SecurityConfigurationDto>
    {
        public SecurityConfiguration CreateFrom(SecurityConfigurationDto dto)
        {
            return new SecurityConfiguration(
               dto.Id,
               dto.ExpirationMinutes,
               dto.RefreshTokenExpirationMinutes,
               dto.AllowedTypes,
               dto.SelectedTypes,
               dto.MaximumAttachmentSize,
               dto.ElasticLimitationStorage,
               dto.MaximumNumberOfFailedLogin,
               dto.AllowedTimeForReEntry,
               dto.MaximumLoginAccount,
               dto.PasswordChangeDaysPeriod);
        }

        public SecurityConfigurationDto MapTo(SecurityConfiguration securityConfiguration)
        {
            return new SecurityConfigurationDto
            {
                Id = securityConfiguration.Id,
                ExpirationMinutes = securityConfiguration.ExpirationMinutes,
                RefreshTokenExpirationMinutes = securityConfiguration.RefreshTokenExpirationMinutes,
                AllowedTypes = securityConfiguration.AllowedTypes,
                SelectedTypes = securityConfiguration.SelectedTypes,
                MaximumAttachmentSize = securityConfiguration.MaximumAttachmentSize,
                ElasticLimitationStorage = securityConfiguration.ElasticLimitationStorage,
                MaximumNumberOfFailedLogin = securityConfiguration.MaximumNumberOfFailedLogin,
                AllowedTimeForReEntry = securityConfiguration.AllowedTimeForReEntry,
                MaximumLoginAccount = securityConfiguration.MaximumLoginAccount,
                PasswordChangeDaysPeriod = securityConfiguration.PasswordChangeDaysPeriod
            };
        }
    }
}
