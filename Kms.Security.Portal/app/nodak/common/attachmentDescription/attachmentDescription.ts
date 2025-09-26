module nodak.common.models {
    export class AttachmentDescription extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "AttachmentDescription");

        }
        AddBussinessModelValidation() {
        }
        private id: any;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }

        private description: string;
        get Description() {
            return this.description;
        }
        set Description(value: string) {
            this.description = value;
        }
        private type: number;
        get Type() {
            return this.type;
        }
        set Type(value: number) {
            this.type = value;
        }

        private attachments: Array<models.Attachment>;
        get Attachments() {
            return this.attachments;
        }
        set Attachments(value) {
            this.attachments = value;
        }
    }

}