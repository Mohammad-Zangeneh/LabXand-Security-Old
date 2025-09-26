module nodak.common.models {
    export class PermissionDto{
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

        private parent: PermissionDto;
        get Parent() {
            return this.parent;
        }
        set Parent(value) {
            this.parent = value;
        }

        private companyId: string;
        get CompanyId() {
            return this.companyId;
        }
        set CompanyId(value) {
            this.companyId = value;
        }

        private company: CompanyDto;
        set Company(value) {
            this.company = value;
        }
        get Company() {
            return this.company;
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