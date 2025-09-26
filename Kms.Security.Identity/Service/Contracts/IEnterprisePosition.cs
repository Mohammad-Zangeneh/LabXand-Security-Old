using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IEnterprisePositionService:IServiceBase<EnterprisePosition,EnterprisePositionDto>
    {
        Dictionary<Guid, string> GetAllEnterprisePositionName();
    }
}
