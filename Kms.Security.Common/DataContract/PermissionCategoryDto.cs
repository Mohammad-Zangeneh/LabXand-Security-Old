using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class PermissionCategoryDto
    {
        public string Name { get;  set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get;  set; }
        public PermissionCategoryDto Parent { get;  set; }

        public Guid? CompanyId { get;  set; }
        public CompanyDto Company { get;  set; }

        public IList<PermissionDto> Permissions { get;  set; }
        public PermissionType PermissionType { set; get; }
    }
}
