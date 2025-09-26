module nodak.common.models {
    export class AttachmentDto {

        private id: any;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }

        private attachmentDescriptionId: string;
        get AttachmentDescriptionId() {
            return this.attachmentDescriptionId;
        }
        set AttachmentDescriptionId(value: string) {
            this.attachmentDescriptionId = value;
        }

        private filePath: string;
        get FilePath() {
            return this.filePath;
        }
        set FilePath(value: string) {
            this.filePath = value;
        }
        private mIME: string;
        get MIME() {
            return this.mIME;
        }
        set MIME(value: string) {
            this.mIME = value;
        }
        private fileSize: number;
        get FileSize() {
            return this.fileSize;
        }
        set FileSize(value: number) {
            this.fileSize = value;
        }

        private createDate: Date;
        get CreateDate() {
            return this.createDate;
        }
        set CreateDate(value: Date) {
            this.createDate = value;
        }
        private fileData: string;
        get FileData() {
            return this.fileData;
        }
        set FileData(value: string) {
            this.fileData = value;

        }

        private version: Date;
        get Version() {
            return this.version;
        }
        set Version(value: Date) {
            this.version = value;
        }


        private itemId: number;
        set ItemId(value) {
            this.itemId = value
        }
        get ItemId() {
            return this.itemId;
        }


        private attachmentURL: string;
        set AttachmentURL(value) {
            this.attachmentURL = value
        }
        get AttachmentURL() {
            return this.attachmentURL;
        }
    }

}