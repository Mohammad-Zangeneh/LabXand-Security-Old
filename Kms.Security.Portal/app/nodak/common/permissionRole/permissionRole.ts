module nodak.common.models {
    export class PermissionRole extends nodak.models.ModelBase {
        constructor() {
            super(enums.SubSystems.Common, "PermissionRole");
        }
        AddBussinessModelValidation() { }
        private role: Role;
        get Role() {
            return this.role;
        }
        set Role(value) {
            this.role = value;
        }

        private roleId: string;
        get RoleId() {
            return this.roleId;
        }
        set RoleId(value) {
            this.roleId = value;
        }

        private permission: Permission;
        set Permission(value) {
            this.permission = value;
        }
        get Permission() {
            return this.permission;
        }

        private permissionId: string;
        get PermissionId() {
            return this.permissionId;
        }
        set PermissionId(value) {
            this.permissionId = value;
        }

        private value: number;
        get Value() {
            return this.value;
        }
        set Value(v) {
            this.value = v;
        }
        
      
    }
}