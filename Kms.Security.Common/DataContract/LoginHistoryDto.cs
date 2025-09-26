using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class LoginHistoryDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public Boolean IsSuccess { get; set; }
        [DataMember]
        public string FingerPrint { get; set; }
        [DataMember]
        public DateTime? ExpireDateToken { get;  set; }

        [DataMember]
        public DateTime? LogOutDate { get; set; }

        

    }
}
