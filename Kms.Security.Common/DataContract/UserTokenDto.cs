using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class UserTokenDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid OwnerUserId { get; set; }
        [DataMember]
        public string AccessTokenHash { get; set; }
        [DataMember]
        public DateTime AccessTokenExpirationDateTime { get; set; }
        [DataMember]
        public string RefreshTokenIdHash { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public DateTime RefreshTokenExpiresUtc { get; set; }
        [DataMember]
        public string RefreshToken { get; set; }
        [DataMember]

        public string SerialNumber { get; set; }

    }
}
