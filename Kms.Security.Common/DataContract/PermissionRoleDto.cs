using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
   public class PermissionRoleDto
    {
        [DataMember]
        public Guid RoleId { get;  set; }
        [DataMember]
        public RoleDto Role { get;  set; }
        [DataMember]
        public PermissionDto Permission { get;  set; }
        [DataMember]
        public Guid PermissionId { get;  set; }
        [DataMember]
        public DateTime LastEdit { get; set; }
        [DataMember]
        public Int16 Value { get;  set; }
    }
}
