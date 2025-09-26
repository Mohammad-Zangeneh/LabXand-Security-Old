using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain

{
    public class DynamicPermission
    {
        public string ControllerName { get; set; }
        public string Id
        {
            get
            {
                return ControllerName + "_" + Name;
            }
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public string Title { get; set; }

        public string ParentId { get; set; }
    }
}
