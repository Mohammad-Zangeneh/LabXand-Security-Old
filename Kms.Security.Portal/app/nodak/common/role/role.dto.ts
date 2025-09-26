module nodak.common.models {
    export class RoleDto {
        private id: string;
        get Id() {
            return this.id;
        }
        set Id(value) {
            this.id = value;
        }

        private name: string;
        get Name() {
            return this.name;
        }
        set Name(value) {
            this.name = value;
        }

        private title: string;
        get Title() {
            return this.title;
        }
        set Title(value) {
            this.title = value;
        }

        private permissions: Array<PermissionRoleDto>;
        set Permissions(value) {
            this.permissions = value;
        }
        get Permissions() {
            return this.permissions;
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

        private createDate;
        set CreateDate(value) {
            this.createDate = value;
        }
        get CreateDate() {
            return this.createDate;
        }

        private lastUpdateDate;
        set LastUpdateDate(value) {
            this.lastUpdateDate = value;
        }
        get LastUpdateDate() {
            return this.lastUpdateDate;
        }


    }
}