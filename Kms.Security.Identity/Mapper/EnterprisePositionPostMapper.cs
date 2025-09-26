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
    public class EnterprisePositionPostMapper : IEntityMapper<EnterprisePositionPost, EnterprisePositionPostDto>
    {
        IEntityMapper<EnterprisePosition, EnterprisePositionDto> _enterprisePositionMapper;
        IEntityMapper<Permission, PermissionDto> _permissionMapper;
        IEntityMapper<Organization, OrganizationDto> _organizationMapper;
        public EnterprisePositionPostMapper(IEntityMapper<EnterprisePosition, EnterprisePositionDto> enterprisePositionMapper, IEntityMapper<Permission, PermissionDto> permissionMapper
            , IEntityMapper<Organization, OrganizationDto> organizationMapper
            )
        {
            _enterprisePositionMapper = enterprisePositionMapper;
            _permissionMapper = permissionMapper;
            _organizationMapper = organizationMapper;
        }
        public EnterprisePositionPost CreateFrom(EnterprisePositionPostDto destination)
        {
            var domain = new EnterprisePositionPost(destination.Id, destination.Title.ApplyCorrectYeKe(), destination.EnterprisePositionId, destination.Description.ApplyCorrectYeKe());
            var permissionList = destination.Permissions != null ? destination.Permissions.Select(p => _permissionMapper.CreateFrom(p)).ToList() : null;
            domain.SetPermissionList(permissionList);
            return domain;
        }

        public EnterprisePositionPostDto MapTo(EnterprisePositionPost source)
        {
            var domainDto = new EnterprisePositionPostDto();
            domainDto.Id = source.Id;
            domainDto.Title = source.Title;
            domainDto.EnterprisePositionId = source.EnterprisePositionId;
            domainDto.EnterprisePosition = source.EnterprisePosition != null ? _enterprisePositionMapper.MapTo(source.EnterprisePosition) : null;

            if (source.EnterprisePosition!= null && source.EnterprisePosition.Organization!= null)
            {
                source.EnterprisePosition.Organization.SetEnterprisePositions(null);
                domainDto.EnterprisePosition.Organization = _organizationMapper.MapTo(source.EnterprisePosition.Organization);
            }
            domainDto.Description = source.Description;
            domainDto.Permissions = source.Permissions != null ? source.Permissions.Select(p => _permissionMapper.MapTo(p)).ToList() : null;
            return domainDto;
        }
    }
}
