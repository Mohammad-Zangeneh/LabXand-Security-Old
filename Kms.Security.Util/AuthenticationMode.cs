using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public enum AuthenticationMode
    {
        Kms,
        Windows,
        Keycloak,
        Cas
    }
}
