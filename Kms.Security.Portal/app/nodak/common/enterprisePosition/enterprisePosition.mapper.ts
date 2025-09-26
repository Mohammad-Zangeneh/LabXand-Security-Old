module nodak.common.models {
    export class EnterprisePositionMapper extends nodak.models.EntityMapper<EnterprisePosition, EnterprisePositionDto>{
        constructor() {
            super();
        }

        MapToDto(entity: EnterprisePosition): EnterprisePositionDto {
            let enterprisePositionDto = new EnterprisePositionDto();
            enterprisePositionDto = ObjectAssign<EnterprisePositionDto>(new EnterprisePositionDto(), entity);

            return enterprisePositionDto;
        }

        MapToEntity(dto: EnterprisePositionDto): EnterprisePosition {
            let enterprisePosition = new EnterprisePosition();
            enterprisePosition = ObjectAssign<EnterprisePosition>(new EnterprisePosition(), dto);
            return enterprisePosition;
        };
    }
}
angular.module('common.services').service('common.enterprisePosition.mapper', nodak.common.models.EnterprisePositionMapper);