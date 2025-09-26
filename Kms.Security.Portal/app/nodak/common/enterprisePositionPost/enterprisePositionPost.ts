module nodak.common.models {

    export class EnterprisePositionPost extends nodak.models.ModelBase {

        constructor() {
            super(enums.SubSystems.Common, "EnterprisePositionPost");
        }
        AddBussinessModelValidation() {

        }
        private id: any
        set Id(value)
        { this.id = value; }
        get Id()
        { return this.id; }

        private title: string;
        get Title() {
            return this.title;
        }
        set Title(value) {
            this.title = value;
        }

        private description: string;
        get Description() {
            return this.description;
        }
        set Description(value) {
            this.description = value;
        }

        private enterprisePositionId: string;
        get EnterprisePositionId() {
            return this.enterprisePositionId;
        }
        set EnterprisePositionId(value) {
            this.enterprisePositionId = value;
        }

        private enterprisePosition: EnterprisePosition;
        set EnterprisePosition(value) {
            this.enterprisePosition = value;
        }
        get EnterprisePosition() {
            return this.enterprisePosition;
        }

        private permissions: Array<models.Permission>;
        get Permissions() {
            return this.permissions;
        }
        set Permissions(value) {
            this.permissions = value;
        }
    }
}
