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
   public class OrganizationMapper : IEntityMapper<Organization, OrganizationDto>
    {
        IEntityMapper<EnterprisePosition, EnterprisePositionDto> _enterprisePositionMapper;
        public OrganizationMapper(IEntityMapper<EnterprisePosition, EnterprisePositionDto> enterprisePositionMapper)
        {
            _enterprisePositionMapper = enterprisePositionMapper;
        }
        public Organization CreateFrom(OrganizationDto organizationDto)
        {
            var domain = new Organization(organizationDto.Id, organizationDto.Name.ApplyCorrectYeKe(), organizationDto.ParentId, organizationDto.SortingNumber
              /*, organizationDto.Code, organizationDto.Address
             , organizationDto.Website, organizationDto.Phone*/);
            //domain.AddParent(organizationDto.Parent != null ? this.CreateFrom(organizationDto.Parent) : null);
            return domain;
        }

        public OrganizationDto MapTo(Organization organization)
        {
            var domainDto = new OrganizationDto()
            {
                Id = organization.Id,
                Name = organization.Name,
                ParentId = organization.ParentId,
                Parent = organization.Parent != null ? this.MapTo(organization.Parent) : null,
                SortingNumber = organization.SortingNumber 
            };
            if (organization.EnterprisePositions != null)
                domainDto.EnterprisePositions = organization.EnterprisePositions.Select(r => _enterprisePositionMapper.MapTo(r)).ToList();
            return domainDto;
        }
    }
}
