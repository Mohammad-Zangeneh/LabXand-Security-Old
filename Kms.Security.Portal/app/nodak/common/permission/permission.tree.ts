module nodak.common.models {
    export class PermissionTree extends labxand.components.core.TreeBase<Permission>{
        constructor(service, callFromServer = true, multiSelect = false) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedParent(() => this.ModelEntity.PermissionCategoryId); 

            this.HasItemSelectedText(() => this.ModelEntity.Title);
            this.MultiSelect = multiSelect;
            this.CallFromService = callFromServer;

        }
    }
}