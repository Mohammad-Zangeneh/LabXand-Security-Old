using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Jwt
{
    public static class GuardExtensions
    {
        public static void CheckArgumentNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }
    }
}