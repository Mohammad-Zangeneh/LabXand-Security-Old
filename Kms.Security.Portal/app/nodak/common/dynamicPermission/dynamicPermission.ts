module nodak.common.models {
    export class DynamicPermission extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "DynamicPermission");
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
        //=============
        private name: string;
        set Name(value) {
            this.name = value;
        }
        get Name() {
            return this.name;
        }

        private title: string;
        get Title() {
            return this.title;
        }
        set Title(value) {
            this.title = value;
        }

        private parentId: string;
        get ParentId() {
            return this.parentId;
        }
        set ParentId(value) {
            this.parentId = value;
        }

        private controllerName: string;
        set ControllerName(value) {
            this.controllerName = value;
        }
        get CoontrollerName() {
            return this.controllerName;
        }


    }
}
