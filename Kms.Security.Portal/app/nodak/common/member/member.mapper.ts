module nodak.common.models {
    export class MemberMapper extends nodak.models.EntityMapper<Member, MemberDto>{
        constructor() {
            super();
        }

        MapToDto(entity: Member): MemberDto {
            let memberDto = new MemberDto();
            memberDto = ObjectAssign<MemberDto>(new MemberDto(), entity);

            return memberDto;
        }

        MapToEntity(dto: MemberDto): Member {
            let member = new Member();
            member = ObjectAssign<Member>(new Member(), dto);
            return member;
        };
    }
}
angular.module('common.services').service('common.member.mapper', nodak.common.models.MemberMapper);