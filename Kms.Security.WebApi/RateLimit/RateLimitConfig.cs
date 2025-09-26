using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.WebApi.RateLimit
{
    public static class RateLimitConfig
    {
        public const int PeriodTime = 10;
        
        public const int MaxRequestPerPeriod = 5;

        public const int BlockTime = 60;
    }
}
