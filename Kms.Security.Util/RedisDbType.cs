using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public static class RedisDbType
    {
        public const int User = 0;
        public const int Permission = 1;
        public const int Notification = 2;
        public const int EnterprisePosition = 3;
        public const int Organization = 4;
        public const int SecurityConfiguration = 5;
        public const int KnowledgeField = 6;
        public const int FormDefinition = 7;
        public const int KnowledgeMaxCode = 8;
        public const int RateLimit = 9;
        public const int LoginAttempts = 10;
        public const int News = 11;
        public const int PostView = 12;
        public const int Message = 13;
        public const int RecoverPassword = 14;
    }
}
