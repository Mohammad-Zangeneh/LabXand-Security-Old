using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Jwt
{
    public class LastLoginAttempt
    {
        public LastLoginAttempt(string username, DateTime lastAttemptDate)
        {
            Username = username;
            LastAttemptDate = lastAttemptDate;
        }
        public DateTime LastAttemptDate { get; }
        public string Username { get; set; }
    }
}
