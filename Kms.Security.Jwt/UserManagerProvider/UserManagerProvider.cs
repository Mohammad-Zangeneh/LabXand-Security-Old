using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Kms.Security.Jwt
{
    public class UserProvider : IUserProvider
    {
        private readonly IApplicationUserManager _userManager;
        public UserProvider(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        public bool CheckInternalUser { get; protected set; } = false;
        public string RipiToken { get; set; } = null;


        public ApplicationUser GetInternalUser(string username, string password)
        {
            var user = UserManager.GetUserWithUsername(username);
            if (user != null && VerifyPassword(password, user.PasswordHash) == PasswordVerificationResult.Success)
                return user;

            return null;
        }

        public virtual ApplicationUser GetUser(string username, string password = null)
        {
            return UserManager.GetUserWithUsername(username);
        }

        public virtual PasswordVerificationResult VerifyPassword(string password, string hashedPassword)
        {
            if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null)
                return PasswordHashSHA.VerifyPassword(hashedPassword, password);
            else
                return _userManager.PasswordHasher.VerifyHashedPassword(hashedPassword, password);
        }

        public virtual string ActiveDirectoryAuthenticate(string username, string password, string domainName = null, string container = null)
        {
            PrincipalContext pc;

            if (domainName == null && container == null)
                pc = new PrincipalContext(ContextType.Domain);
            else if (container == null)
                pc = new PrincipalContext(ContextType.Domain, domainName);
            else
                pc = new PrincipalContext(ContextType.Domain, domainName, container, username, password);

            using (pc)
            {
                return pc.ValidateCredentials(username, password) ? username.Split('@').First() : null;
            }
        }

    }
}
