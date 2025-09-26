using System;

namespace Kms.Security.Common.Domain
{
    public class SecurityConfiguration : IDomainBase<Guid>
    {
        protected SecurityConfiguration() { }

        public SecurityConfiguration(Guid id, int expirationMinutes, int refreshTokenExpirationMinutes, string allowedTypes, string selectedTypes, long maximumAttachmentSize, long elasticLimitationStorage, int maximumNumberOfFailedLogin, int allowedTimeForReEntry, int maximumLoginAccount, int passwordChangeDaysPeriod)
        {
            Id = id;
            ExpirationMinutes = expirationMinutes;
            RefreshTokenExpirationMinutes = refreshTokenExpirationMinutes;
            AllowedTypes = allowedTypes;
            SelectedTypes = selectedTypes;
            MaximumAttachmentSize = maximumAttachmentSize;
            ElasticLimitationStorage = elasticLimitationStorage;
            MaximumNumberOfFailedLogin = maximumNumberOfFailedLogin;
            AllowedTimeForReEntry = allowedTimeForReEntry;
            MaximumLoginAccount = maximumLoginAccount;
            PasswordChangeDaysPeriod = passwordChangeDaysPeriod;
        }

        public Guid Id { get; protected set; }
        public int ExpirationMinutes { get; protected set; }
        public int RefreshTokenExpirationMinutes { get; protected set; }
        public string AllowedTypes { get; protected set; }
        public string SelectedTypes { get; protected set; }
        public long MaximumAttachmentSize { get; protected set; }
        public long ElasticLimitationStorage { get; protected set; }
        public int MaximumNumberOfFailedLogin { get; protected set; }
        public int AllowedTimeForReEntry { get; protected set; }
        public int MaximumLoginAccount { get; protected set; }
        public int PasswordChangeDaysPeriod { get; protected set; }

        public void SetNewId()
        {
            this.Id = Guid.NewGuid();
        }

        public bool IdIsEmpty()
        {
            return this.Id == Guid.Empty;
        }

        public void SetExpirationMinutes(int expirationMinutes)
        {
            ExpirationMinutes = expirationMinutes;
        }

        public void SetRefreshTokenExpirationMinutes(int refreshTokenExpirationMinutes)
        {
            RefreshTokenExpirationMinutes = refreshTokenExpirationMinutes;
        }

        public void SetAllowedTypes(string allowedTypes)
        {
            AllowedTypes = allowedTypes;
        }

        public void SetSelectedTypes(string selectedTypes)
        {
            SelectedTypes = selectedTypes;
        }

        public void SetMaximumAttachmentSize(long maximumAttachmentSize)
        {
            MaximumAttachmentSize = maximumAttachmentSize;
        }

        public void SetElasticLimitationStorage(long elasticLimitationStorage)
        {
            ElasticLimitationStorage = elasticLimitationStorage;
        }

        public void SetMaximumNumberOfFailedLogin(int maximumNumberOfFailedLogin)
        {
            MaximumNumberOfFailedLogin = maximumNumberOfFailedLogin;
        }

        public void SetAllowedTimeForReEntry(int allowedTimeForReEntry)
        {
            AllowedTimeForReEntry = allowedTimeForReEntry;
        }

        public void SetMaximumLoginAccount(int maximumLoginAccount)
        {
            MaximumLoginAccount = maximumLoginAccount;
        }

        public void SetPasswordChangeDaysPeriod(int days)
        {
            PasswordChangeDaysPeriod = days;
        }
    }
}
