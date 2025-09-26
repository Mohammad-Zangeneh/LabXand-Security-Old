using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class PermissionMapper : IEntityMapper<Permission, PermissionDto>
    {
       // IEntityMapper<PermissionCategory, PermissionCategoryDto> _permissionCategoryMapper;
        public PermissionMapper(/*IEntityMapper<PermissionCategory, PermissionCategoryDto> permissionCategoryMapper*/)
        {
            //_permissionCategoryMapper = permissionCategoryMapper;
        }
        public Permission CreateFrom(PermissionDto domainDto)
        {
            var domain = new Permission(domainDto.Id, domainDto.Title, domainDto.Code, domainDto.PermissionCategoryId,domainDto.PermissionType);
            return domain;
        }

        public PermissionDto MapTo(Permission domain)
        {
            var domainDto = new PermissionDto();
            domainDto.Id = domain.Id;
            //domainDto.ParentId = domain.ParentId;
            //domainDto.Parent = domain.Parent != null ? MapTo(domain.Parent) : null;
            domainDto.Title = domain.Title;
            domainDto.Code = domain.Code;
            domainDto.PermissionCategoryId = domain.PermissionCategoryId;
            domainDto.PermissionType = domain.PermissionType;
            return domainDto;
        }
    }
}
