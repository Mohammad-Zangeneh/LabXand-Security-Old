using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class ClientIPAccessDto
    {
        [DataMember]
        public Guid Id { get;  set; }
        [DataMember]
        public string IpAddress { get;  set; }
        [DataMember]
        public string Port { get; set; }
        [DataMember]
        public bool IsWhiteList { get;  set; }


    }
}
