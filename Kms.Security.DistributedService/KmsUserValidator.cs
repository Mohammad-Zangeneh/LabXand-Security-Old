using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.ServiceModel;
using System.IdentityModel.Selectors;

namespace Kms.Security.DistributedService
{
    public class KmsUserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) && userName.Equals(password))
            {
                throw new SecurityException(string.Format("Invalid Credential.{0}Username is '{1}' and token is '{2}'", System.Environment.NewLine, userName, password));
            }
        }
    }
}
