module nodak.common.models {
    export class Login extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "Login"); 
        }
        AddBussinessModelValidation() { }

        //=====================
        private userName: string;
        set UserName(value) {
            this.userName = value;
        }
        get UserName() {
            return this.userName;
        }
     
        //==========================
        private password: string;
        set Password(value) {
            this.password = value;
        }
        get Password() {
            return this.password;
        }
        //==========================

    }
}