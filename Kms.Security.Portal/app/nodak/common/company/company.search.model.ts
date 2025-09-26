module nodak.common.models {
    export class CompanySearchModel {

       
        private name: string;
        set Name(value) {
            this.name = value;
        }
        get Name() {
            return this.name;
        }

    }
}