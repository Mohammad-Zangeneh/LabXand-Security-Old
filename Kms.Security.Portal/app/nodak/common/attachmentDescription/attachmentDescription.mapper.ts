module nodak.common.models {
    export class AttachmentDescriptionMapper extends nodak.models.EntityMapper<AttachmentDescription, AttachmentDescriptionDto>
    {
        constructor() {
            super();
        }
        MapToDto(entity: AttachmentDescription) {
            let attachmentDescriptionDto = new AttachmentDescriptionDto();
            attachmentDescriptionDto = ObjectAssign<AttachmentDescriptionDto>(new AttachmentDescriptionDto(), entity);

            return attachmentDescriptionDto;
        }
        MapToEntity(dto: AttachmentDescriptionDto) {
            let attachmentDescription = new AttachmentDescription();
            attachmentDescription = ObjectAssign<AttachmentDescription>(new AttachmentDescription(), dto);
            return attachmentDescription;
        }
    }
}
commonService.service('common.attachmentDescription.mapper', nodak.common.models.AttachmentDescriptionMapper);