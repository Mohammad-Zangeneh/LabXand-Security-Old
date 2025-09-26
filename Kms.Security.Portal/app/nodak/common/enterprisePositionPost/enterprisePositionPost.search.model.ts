module nodak.common.models {
    export class EnterprisePositionPostSearchModel {

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
    }
}