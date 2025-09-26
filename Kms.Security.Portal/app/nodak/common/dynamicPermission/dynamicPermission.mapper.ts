module nodak.common.models {
    export class DynamicPermissionMapper extends nodak.models.EntityMapper<DynamicPermission, DynamicPermissionDto>{
        constructor() {
            super();
        }

        MapToDto(entity: DynamicPermission): DynamicPermissionDto {
            let dynamicPermissionDto = new DynamicPermissionDto();
            dynamicPermissionDto = ObjectAssign<DynamicPermissionDto>(new DynamicPermissionDto(), entity);

            return dynamicPermissionDto;
        }

        MapToEntity(dto: DynamicPermissionDto): DynamicPermission {
            let dynamicPermission = new DynamicPermission();
            dynamicPermission = ObjectAssign<DynamicPermission>(new DynamicPermission(), dto);
            return dynamicPermission;
        };
    }
}
angular.module('common.services').service('common.dynamicPermission.mapper', nodak.common.models.DynamicPermissionMapper);