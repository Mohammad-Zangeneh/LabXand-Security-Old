using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabXand.DistributedServices;


namespace Kms.Security.Identity
{ 
    public class CompanyService : ServiceBase<Guid, Company, CompanyDto>, ICompanyService
    {
        public CompanyService(IEntityMapper<Company, CompanyDto> mapper) : base(mapper)
        {
        }
    }
}
