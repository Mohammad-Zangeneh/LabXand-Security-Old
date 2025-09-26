module nodak.models {
    export class FileUpload {
        Scan: nodak.common.models.AttachmentDescription
        constructor() {
            this.Scan = new nodak.common.models.AttachmentDescription();
            this.Scan.Attachments = [];
            for (var i = 0; i <= 5; i++) {
                var attachmentTest = new nodak.common.models.Attachment();
                attachmentTest.FilePath = "index" + i + ".jpg";
                attachmentTest.Id = i;

                this.Scan.Attachments.push(attachmentTest);

            }
        }
    }
}