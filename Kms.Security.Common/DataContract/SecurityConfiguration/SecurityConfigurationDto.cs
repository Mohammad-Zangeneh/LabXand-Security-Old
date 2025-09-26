using System.Runtime.Serialization;
using System;

namespace Kms.Security.Common.Domain
{
    public class SecurityConfigurationDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int ExpirationMinutes { get; set; }

        [DataMember]
        public int RefreshTokenExpirationMinutes { get; set; }

        [DataMember]
        public string AllowedTypes { get; set; }

        [DataMember]
        public string SelectedTypes { get; set; }

        [DataMember]
        public long MaximumAttachmentSize { get; set; }

        [DataMember]
        public long ElasticLimitationStorage { get; set; }

        [DataMember]
        public int MaximumNumberOfFailedLogin { get; set; }

        [DataMember]
        public int AllowedTimeForReEntry { get; set; }

        [DataMember]
        public int MaximumLoginAccount { get; set; }

        [DataMember]
        public int PasswordChangeDaysPeriod { get; set; }
    }
}
