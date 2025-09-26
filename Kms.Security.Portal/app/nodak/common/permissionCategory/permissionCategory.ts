module nodak.common.models {
    export class PermissionCategory extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "PermissionCategory");
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

        private parentId: string;
        set ParentId(value) {
            this.parentId = value;
        }
        get ParentId() {
            return this.parentId;
        }

        private parent: PermissionCategory;
        set Parent(value) {
            this.parent = value;
        }
        get Parent() {
            return this.parent;
        }

        private companyId: string;
        get CompanyId() {
            return this.companyId;
        }
        set CompanyId(value) {
            this.companyId = value;
        }

        private company: Company;
        set Company(value) {
            this.company = value;
        }
        get Company() {
            return this.company;
        }

        private permissions: Array<Permission>;
        set Permissions(value) {
            this.permissions = value;
        }
        get Permissions() {
            return this.permissions;
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