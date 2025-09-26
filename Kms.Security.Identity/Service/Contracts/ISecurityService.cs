using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
   public interface ISecurityService
    {
        string GetSha256Hash(string input);

    }
}
