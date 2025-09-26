using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class PermissionCategoryMapper : IEntityMapper<PermissionCategory, PermissionCategoryDto>
    {
        IEntityMapper<Company, CompanyDto> _companyMapper;
        IEntityMapper<Permission, PermissionDto> _permissionMapper;
        public PermissionCategoryMapper(IEntityMapper<Company,CompanyDto> companyMapper,IEntityMapper<Permission,PermissionDto> permissionMapper)
        {
            _companyMapper = companyMapper;
            _permissionMapper = permissionMapper;
        }
        public PermissionCategory CreateFrom(PermissionCategoryDto domainDto)
        {
            var PC = new PermissionCategory(domainDto.Id, domainDto.Name, domainDto.ParentId, domainDto.CompanyId,domainDto.PermissionType);
            return PC;
        }

        public PermissionCategoryDto MapTo(PermissionCategory source)
        {
            var domainDto = new PermissionCategoryDto();
            domainDto.Id = source.Id;
            domainDto.Name = source.Name;
            domainDto.ParentId = source.ParentId;
            domainDto.Parent = source.Parent != null ? MapTo(source.Parent) : null;
            domainDto.CompanyId = source.CompanyId;
            domainDto.Company = source.Company != null ? _companyMapper.MapTo(source.Company) : null;
            domainDto.Permissions = source.Permissions != null ? source.Permissions.Select(p => _permissionMapper.MapTo(p)).ToList():null;
            domainDto.PermissionType = source.PermissionType;
            return domainDto;
        }
    }
}
