module nodak.common.models {
    export class CompanyDto {
        private id: string;
        set Id(value) {
            this.id = value;
        }
        get Id() {
            return this.id;
        }

        private name: string;
        get Name() {
            return this.name;
        }
        set Name(value) {
            this.name = value;
        }

    }
}