using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class OrganizationDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { set; get; }

        [DataMember]
        public OrganizationDto Parent { set; get; }
        [DataMember]
        public Guid? ParentId { set; get; }


        [DataMember]
        public int SortingNumber { get; set; }
        [DataMember]
        public IList<EnterprisePositionDto> EnterprisePositions { get; set; }

    }
}
