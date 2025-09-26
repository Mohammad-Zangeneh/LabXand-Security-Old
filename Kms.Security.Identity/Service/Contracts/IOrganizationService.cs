using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
   public interface IOrganizationService :IServiceBase<Organization,OrganizationDto>
    {
        IList<Organization> GetAllChild(Guid parentId);
        OrganizationDto GetRoot();
        IList<OrganizationDto> GetAllWithoutNavigation();
    }
}
