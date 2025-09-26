module nodak.common.models {
    export class AttachmentMapper extends nodak.models.EntityMapper<Attachment, AttachmentDto>
    {
        constructor() {
            super();
        }
        MapToDto(entity: Attachment) {
            let attachmentDto = new AttachmentDto();
            attachmentDto = ObjectAssign<AttachmentDto>(new AttachmentDto(), entity);

            return attachmentDto;
        }
        MapToEntity(dto: AttachmentDto) {
            let attachment = new Attachment();
            attachment = ObjectAssign<Attachment>(new Attachment(), dto);
            return attachment;
        }
    }
}
commonService.service('common.attachment.mapper', nodak.common.models.AttachmentMapper);