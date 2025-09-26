using Kms.Security.Common.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Jwt
{
    public interface IUserProvider
    {
        bool CheckInternalUser { get; }
        string RipiToken { get; set; }

        PasswordVerificationResult VerifyPassword(string password, string hashedPassword = null);
        ApplicationUser GetUser(string username,string password = null);
        ApplicationUser GetInternalUser(string username, string password);
    }
}
