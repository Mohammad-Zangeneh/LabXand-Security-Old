module nodak.common.models {
    export class RoleSearchModel {

        private id: string;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }

        private name: string;
        get Name() {
            return this.name;
        }
        set Name(value) {
            this.name = value;
        }

        private title: string;
        get Title() {
            return this.title;
        }
        set Title(value) {
            this.title = value;
        }
    }
}