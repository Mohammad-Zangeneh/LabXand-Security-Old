using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class EnterprisePositionPostDto 
    {
        [DataMember]
        public Guid Id {  set; get; }
        [DataMember]
        public string Title { get;  set; }
        [DataMember]
        public EnterprisePositionDto EnterprisePosition { get;  set; }
        [DataMember]
        public Guid EnterprisePositionId { get;  set; }
        [DataMember]
        public string Description { get;  set; }

        [DataMember]
        public IList<PermissionDto> Permissions{ get; set; }

    }
}
