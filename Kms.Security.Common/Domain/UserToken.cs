using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class UserToken : IDomainBase<Guid>
    {

        public Guid OwnerUserId { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTime AccessTokenExpirationDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string Subject { get; set; }

        public DateTime RefreshTokenExpiresUtc { get; set; }

        public string RefreshToken { get; set; }

        public Guid Id { set; get; }

        public string SerialNumber { get; set; }//for check token is active or not
        public void SetNewId()
        {
            this.Id = Guid.NewGuid();
        }

        public bool IdIsEmpty()
        {
            return Id == Guid.Empty;
        }
    }
}
