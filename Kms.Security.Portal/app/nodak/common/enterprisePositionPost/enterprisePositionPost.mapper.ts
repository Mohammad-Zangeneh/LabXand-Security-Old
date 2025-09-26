module nodak.common.models {
    export class EnterprisePositionPostMapper extends nodak.models.EntityMapper<EnterprisePositionPost, EnterprisePositionPostDto>{
        constructor() {
            super();
        }

        MapToDto(entity: EnterprisePositionPost): EnterprisePositionPostDto {
            let enterprisePositionPostDto = new EnterprisePositionPostDto();
            enterprisePositionPostDto = ObjectAssign<EnterprisePositionPostDto>(new EnterprisePositionPostDto(), entity);

            return enterprisePositionPostDto;
        }

        MapToEntity(dto: EnterprisePositionPostDto): EnterprisePositionPost {
            let enterprisePositionPost = new EnterprisePositionPost();
            enterprisePositionPost = ObjectAssign<EnterprisePositionPost>(new EnterprisePositionPost(), dto);
            return enterprisePositionPost;
        };
    }
}
angular.module('common.services').service('common.enterprisePositionPost.mapper', nodak.common.models.EnterprisePositionPostMapper);