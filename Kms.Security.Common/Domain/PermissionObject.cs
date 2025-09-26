using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class PermissionObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string PermissionCategoryId { get; set; }
        public object PermissionCategory { get; set; }
        public object Company { get; set; }
        public object CompanyId { get; set; }
        public int PermissionType { get; set; }
    }
}
