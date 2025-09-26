using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using LabXand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class CompanyMapper : IEntityMapper<Company, CompanyDto>
    {
        public Company CreateFrom(CompanyDto destination)
        {
            var domain = new Company(destination.Name.ApplyCorrectYeKe(), destination.Id);
            return domain;
        }

        public CompanyDto MapTo(Company source)
        {
            var domainDto = new CompanyDto();
            domainDto.Id = source.Id;
            domainDto.Name = source.Name;
            return domainDto;
        }
    }
}
