module nodak.common.models {
    export class PermissionCategoryMapper extends nodak.models.EntityMapper<PermissionCategory, PermissionCategoryDto>{
        constructor() {
            super();
        }

        MapToDto(entity: PermissionCategory): PermissionCategoryDto {
            let permissionCategoryDto = new PermissionCategoryDto();
            permissionCategoryDto = ObjectAssign<PermissionCategoryDto>(new PermissionCategoryDto(), entity);

            return permissionCategoryDto;
        }

        MapToEntity(dto: PermissionCategoryDto): PermissionCategory {
            let permissionCategory = new PermissionCategory();
            permissionCategory = ObjectAssign<PermissionCategory>(new PermissionCategory(), dto);
            return permissionCategory;
        };
    }
}
angular.module('common.services').service('common.permissionCategory.mapper', nodak.common.models.PermissionCategoryMapper);