module nodak.common.models {
    export class PermissionMapper extends nodak.models.EntityMapper<Permission, PermissionDto>{
        constructor() {
            super();
        }

        MapToDto(entity: Permission): PermissionDto {
            let permissionDto = new PermissionDto();
            permissionDto = ObjectAssign<PermissionDto>(new PermissionDto(), entity);

            return permissionDto;
        }

        MapToEntity(dto: PermissionDto): Permission {
            let permission = new Permission();
            permission = ObjectAssign<Permission>(new Permission(), dto);
            return permission;
        };
    }
}
angular.module('common.services').service('common.permission.mapper', nodak.common.models.PermissionMapper);