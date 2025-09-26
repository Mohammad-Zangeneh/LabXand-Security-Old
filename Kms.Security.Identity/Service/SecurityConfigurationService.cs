using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using LabXand.Infrastructure.Data.EF;
using System;
using System.Linq;

namespace Kms.Security.Identity.Service
{
    public class SecurityConfigurationService : ServiceBase<Guid, SecurityConfiguration, SecurityConfigurationDto>, ISecurityConfigurationService
    {
        public SecurityConfigurationService(IEntityMapper<SecurityConfiguration, SecurityConfigurationDto> mapper) : base(mapper)
        {
        }

        public SecurityConfigurationDto GetSecurityConfiguration()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var securityConfiguration= dbContext.Set<SecurityConfiguration>().FirstOrDefault();
                return _mapper.MapTo(securityConfiguration);
            }
        }

        private string _trackEntity;
        public SecurityConfigurationDto EditSecurityConfiguration(SecurityConfigurationDto dto)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var oldSecurityConfiguration = dbContext.Set<SecurityConfiguration>().FirstOrDefault();
                oldSecurityConfiguration.SetExpirationMinutes(dto.ExpirationMinutes);
                oldSecurityConfiguration.SetRefreshTokenExpirationMinutes(dto.RefreshTokenExpirationMinutes);
                oldSecurityConfiguration.SetAllowedTypes(dto.AllowedTypes);
                oldSecurityConfiguration.SetSelectedTypes(dto.SelectedTypes);
                oldSecurityConfiguration.SetMaximumAttachmentSize(dto.MaximumAttachmentSize);
                oldSecurityConfiguration.SetElasticLimitationStorage(dto.ElasticLimitationStorage);
                oldSecurityConfiguration.SetMaximumNumberOfFailedLogin(dto.MaximumNumberOfFailedLogin);
                oldSecurityConfiguration.SetAllowedTimeForReEntry(dto.AllowedTimeForReEntry);
                oldSecurityConfiguration.SetMaximumLoginAccount(dto.MaximumLoginAccount);
                oldSecurityConfiguration.SetPasswordChangeDaysPeriod(dto.PasswordChangeDaysPeriod);
                _trackEntity = dbContext.StringifyDbContextChanges();
                dbContext.SaveAllChanges();
                return _mapper.MapTo(oldSecurityConfiguration);
            }
        }
        public string GetTrackEntity()
        {
            return _trackEntity ?? null;
        }
    }
}
