using Kms.Security.Common.Domain;
using LabXand.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IClientIPAccessService
    {
        ClientIPAccessDto Save(ClientIPAccessDto domainDto);
        Paginated<ClientIPAccess> GetAllForGrid(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems);
        IQueryable<ClientIPAccess> GetAll();
        ClientIPAccess GetWithIP(string ipAddress);
        ClientIPAccessDto Delete(ClientIPAccessDto clientIPAccessDto);
    }
}
