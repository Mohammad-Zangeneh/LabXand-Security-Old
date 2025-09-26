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
    public class RoleMapper : IEntityMapper<LabxandRole, RoleDto>
    {
        IEntityMapper<PermissionRole, PermissionRoleDto> _permissionRoleMapper;
        IEntityMapper<Company, CompanyDto> _companyMapper;
        public RoleMapper(IEntityMapper<PermissionRole, PermissionRoleDto> permissionRoleMapper, IEntityMapper<Company, CompanyDto> companyMapper)
        {
            _permissionRoleMapper = permissionRoleMapper;
            _companyMapper = companyMapper;
        }
        public LabxandRole CreateFrom(RoleDto domainDto)
        {

            var domain = new LabxandRole(domainDto.Id, domainDto.Name.ApplyCorrectYeKe(), domainDto.Title.ApplyCorrectYeKe(), domainDto.CompanyId);
            if (domainDto.Permissions != null)
                domain.SetPermissions(domainDto.Permissions.Select(p => _permissionRoleMapper.CreateFrom(p)).ToList());
            return domain;
        }

        public RoleDto MapTo(LabxandRole domain)
        {
            var domainDto = new RoleDto();
            domainDto.Id = domain.Id;
            domainDto.Name = domain.Name;
            domainDto.Title = domain.Title;
            domainDto.Permissions = domain.Permissions != null ? domain.Permissions.Select(p => _permissionRoleMapper.MapTo(p)).ToList() : null;
            domainDto.Company = domain.Company != null ? _companyMapper.MapTo(domain.Company) : null;
            domainDto.CreateDate = domain.CreateDate;
            domainDto.LastUpdateDate = domain.LastUpdateDate;
            return domainDto;
        }
    }
}
