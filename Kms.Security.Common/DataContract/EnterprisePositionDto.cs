using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
   public class EnterprisePositionDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public EnterprisePositionDto Parent { get; set; }

        [DataMember]
        public Guid? ParentId { get; set; }

        [DataMember]
        public Guid OrganizationId { get; set; }

        [DataMember]
        public OrganizationDto Organization { get; set; }
        [DataMember]
        public int? SortingNumber { get; set; }
    }
}
