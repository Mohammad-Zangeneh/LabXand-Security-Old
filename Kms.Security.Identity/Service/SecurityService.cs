using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Service
{
    public class SecurityService : ISecurityService
    {
        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}