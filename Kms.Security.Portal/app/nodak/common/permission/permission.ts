module nodak.common.models {
    export class Permission extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "Permission");
            //this.model = new kms.userManagement.models.MemberDto();
            //this.HasProperty(() => this.LastName, "نام خانوادگی").IsRequired();
            //this.HasProperty(() => this.FirstName, "نام ").IsRequired();
            //this.HasProperty(() => this.Password, "رمز عبور ").IsRequired();
            //this.HasProperty(() => this.UserName, "نام کاربری ").IsRequired();
            //this.HasProperty(() => this.Email, "ایمیل ").IsRequired();
        }
        AddBussinessModelValidation() { }

        private id: string;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }

        private code: string;
        get Code() {
            return this.code;
        }
        set Code(value) {
            this.code = value;
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

        private parent: Permission;
        get Parent() {
            return this.parent;
        }
        set Parent(value) {
            this.parent = value;
        }

        private permissionCategoryId: string;
        get PermissionCategoryId() {
            return this.permissionCategoryId;
        }
        set PermissionCategoryId(value) {
            this.permissionCategoryId = value;
        }

        private permissionCategory: PermissionCategory;
        set PermissionCategory(value) {
            this.permissionCategory = value;
        }
        get PermissionCategory() {
            return this.permissionCategory;
        }

        private companyId: string;
        get CompanyId() {
            return this.companyId;
        }
        set CompanyId(value) {
            this.companyId = value;
        }
        private permissionType: number;
        get PermissionType() {
            return this.permissionType;
        }
        set PermissionType(value) {
            this.permissionType = value;
        }

    }
}