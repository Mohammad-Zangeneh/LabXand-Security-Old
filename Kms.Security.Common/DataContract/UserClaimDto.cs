using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.DataContract
{
    public class UserClaimDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
