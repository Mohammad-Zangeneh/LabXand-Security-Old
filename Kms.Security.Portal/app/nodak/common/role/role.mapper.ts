module nodak.common.models {
    export class RoleMapper extends nodak.models.EntityMapper<Role, RoleDto>{
        constructor() {
            super();
        }

        MapToDto(entity: Role): RoleDto {
            let roleDto = new RoleDto();
            roleDto = ObjectAssign<RoleDto>(new RoleDto(), entity);

            return roleDto;
        }

        MapToEntity(dto: RoleDto): Role {
            let role = new Role();
            role = ObjectAssign<Role>(new Role(), dto);
            return role;
        };
    }
}
angular.module('common.services').service('common.role.mapper', nodak.common.models.RoleMapper);