module nodak.common.models {
    export class ChangePasswordModel extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "ChangePassword");
        }
        AddBussinessModelValidation() { }

        //=====================
        private currentPassword: string;
        set CurrentPassword(value) {
            this.currentPassword = value;
        }
        get CurrentPassword() {
            return this.currentPassword;
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
        private confirmPassword: string;
        set ConfirmPassword(value) {
            this.confirmPassword = value;
        }
        get ConfirmPassword() {
            return this.confirmPassword;
        }

    }
}