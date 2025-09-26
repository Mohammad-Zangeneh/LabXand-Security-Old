using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class RoleDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public IList<PermissionRoleDto> Permissions { get; set; }

        [DataMember]
        public Guid? CompanyId { get;  set; }

        [DataMember]
        public CompanyDto Company { get;  set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public DateTime? LastUpdateDate { get; set; }

    }
}
