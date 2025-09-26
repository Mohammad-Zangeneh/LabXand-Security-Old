using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class PermissionDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Code { get; set; }
        //[DataMember]
        //public PermissionDto Parent { get;  set; }
        //[DataMember]
        //public Guid? ParentId { get;  set; }
        [DataMember]
        public Guid? PermissionCategoryId { get; set; }

        [DataMember]
        public PermissionCategoryDto PermissionCategory { get; set; }
        public PermissionType PermissionType { get; set; }

    }
}
