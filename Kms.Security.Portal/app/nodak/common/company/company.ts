module nodak.common.models {
    export class Company extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "Company");
            //this.model = new kms.userManagement.models.MemberDto();
            //this.HasProperty(() => this.LastName, "نام خانوادگی").IsRequired();
            //this.HasProperty(() => this.FirstName, "نام ").IsRequired();
            //this.HasProperty(() => this.Password, "رمز عبور ").IsRequired();
            //this.HasProperty(() => this.UserName, "نام کاربری ").IsRequired();
            //this.HasProperty(() => this.Email, "ایمیل ").IsRequired();
        }
        AddBussinessModelValidation() { }
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