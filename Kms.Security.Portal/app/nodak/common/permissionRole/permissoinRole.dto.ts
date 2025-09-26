module nodak.common.models {
    export class PermissionRoleDto{
        private role: RoleDto;
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

        private permission: PermissionDto;
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