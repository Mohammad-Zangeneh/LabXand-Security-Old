module nodak.common.models {
    export class DynamicPermissionDto{
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
