module nodak.common.models {
    export class PermissionTypeDto {
        private id: number;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }
        private value: string;
        get Value() {
            return this.value;
        }
        set Value(v) {
            this.value = v;
        }
    }
}