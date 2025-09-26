module nodak.common.models {
    export class DynamicPermissionTree extends labxand.components.core.TreeBase<DynamicPermission>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedParent(() => this.ModelEntity.ParentId);
            this.HasItemSelectedText(() => this.ModelEntity.Title);
            this.MultiSelect = true;
            this.CallFromService = true;

        }
    }
}