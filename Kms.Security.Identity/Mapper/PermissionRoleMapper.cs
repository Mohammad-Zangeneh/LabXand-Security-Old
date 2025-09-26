using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class PermissionRoleMapper : IEntityMapper<PermissionRole, PermissionRoleDto>
    {
        public PermissionRole CreateFrom(PermissionRoleDto destination)
        {
            var domain = new PermissionRole(destination.RoleId, destination.PermissionId, destination.Value);
            return domain;
        }

        public PermissionRoleDto MapTo(PermissionRole source)
        {
            var domainDto = new PermissionRoleDto();
            domainDto.PermissionId = source.PermissionId;
            domainDto.RoleId = source.RoleId;
            domainDto.Value = source.Value;
            domainDto.LastEdit = source.LastEdit;
            return domainDto;
        }
    }
}
