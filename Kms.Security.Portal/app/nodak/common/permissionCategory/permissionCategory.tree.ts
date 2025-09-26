module nodak.common.models {
    export class PermissionCategoryTree extends labxand.components.core.TreeBase<PermissionCategory>{
        constructor(service, callFromServer = true, multiSelect = false) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedParent(() => this.ModelEntity.ParentId);
            this.HasItemSelectedText(() => this.ModelEntity.Name);
            this.MultiSelect = multiSelect;
            this.CallFromService = callFromServer;

        }
    }
}