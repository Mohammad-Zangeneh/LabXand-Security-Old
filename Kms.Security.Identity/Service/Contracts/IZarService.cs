using Kms.Security.Common.DataContract.Zar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Service.Contracts
{
    public interface IZarService
    {
        IList<ZarApiDto> GetZarRawData();
        void SyncData();
        void SyncOrganizations(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode);
        void SyncEnterprisePositions(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode);
        void SyncUsers(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode);
    }
}
