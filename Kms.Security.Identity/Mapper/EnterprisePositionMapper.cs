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
  public  class EnterprisePositionMapper : IEntityMapper<EnterprisePosition, EnterprisePositionDto>
    {
        public EnterprisePositionMapper()
        {
        }
        public EnterprisePosition CreateFrom(EnterprisePositionDto domainDto)
        {
            return new EnterprisePosition(domainDto.Id, domainDto.Name.ApplyCorrectYeKe(), domainDto.ParentId, domainDto.OrganizationId
                ,domainDto.SortingNumber);
        }

        public EnterprisePositionDto MapTo(EnterprisePosition domain)
        {
            EnterprisePositionDto domainDto = new EnterprisePositionDto
            {
                Name = domain.Name,

                Parent = domain.Parent != null ? this.MapTo(domain.Parent) : null,
                ParentId = domain.ParentId,
                OrganizationId = domain.OrganizationId,
                Organization = domain.Organization!= null ? new OrganizationDto() { Id = domain.Organization.Id, Name = domain.Organization.Name, ParentId = domain.Organization.ParentId, SortingNumber = domain.Organization.SortingNumber }  : null,
                Id = domain.Id,
                SortingNumber = domain.SortingNumber ?? 0
            };
            return domainDto;
        }
    }
}
