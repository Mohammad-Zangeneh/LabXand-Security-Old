module nodak.models {

    export class TraceData {

        private recordDate: Date;
        set RecordDate(value) {
            this.recordDate = value;
        }
        get RecordDate() {
            return this.recordDate;
        }

        private userId: string;
        set UserId(value) {
            this.userId = value;
        }
        get UserId() {
            return this.userId;
        }

        private organizationId: string;
        set OrganizationId(value) {
            this.organizationId = value;
        }
        get OrganizationId() {
            return this.organizationId;
        }

        private unitId: string;
        set UnitId(value) {
            this.unitId = value;
        }
        get UnitId() {
            return this.unitId;
        }

        private delegatedToUserId: string;
        set DelegatedToUserId(value) {
            this.delegatedToUserId = value;
        }
        get DelegatedToUserId() {
            return this.delegatedToUserId;
        }

        private organizationCode: string;
        set OrganizationCode(value) {
            this.organizationCode = value;
        }
        get OrganizationCode() {
            return this.organizationCode;
        }
    }

}