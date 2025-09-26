module nodak.common.models {
    export class OrganizationMapper extends nodak.models.EntityMapper<Organization, OrganizationDto>{
        constructor() {
            super();
        }

        MapToDto(entity: Organization): OrganizationDto {
            let organizationDto = new OrganizationDto();
            organizationDto = ObjectAssign<OrganizationDto>(new OrganizationDto(), entity);

            return organizationDto;
        }

        MapToEntity(dto: OrganizationDto): Organization {
            let organization = new Organization();
            organization = ObjectAssign<Organization>(new Organization(), dto);
            return organization;
        };
    }
}
angular.module('common.services').service('common.organization.mapper', nodak.common.models.OrganizationMapper);