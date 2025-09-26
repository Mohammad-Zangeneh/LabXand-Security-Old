using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.WebApi.RateLimit
{
    public class RequestIpHistoryModel
    {
        public string IpAddress { get; set; }
        public DateTime FirstApprovedRequest { get; set; } = DateTime.MinValue;
        public int NumberOfApprovedRequest { get; set; } = 0;
        public DateTime ReleaseTime { get; set; } = DateTime.MinValue;

        public void Lock(int secondFromNow)
        {
            this.ReleaseTime = DateTime.Now.AddSeconds(secondFromNow);
        }

        public void Unlock()
        {
            this.FirstApprovedRequest = DateTime.UtcNow;
            this.NumberOfApprovedRequest = 1;
            this.ReleaseTime = DateTime.MinValue;
        }
    }
}
