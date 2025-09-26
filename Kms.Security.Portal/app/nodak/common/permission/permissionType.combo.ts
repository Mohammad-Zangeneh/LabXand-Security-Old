module nodak.common.models {
    export class PermissionTypeCombo extends labxand.components.core.ComboBase<models.PermissionTypeDto>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedText(() => this.ModelEntity.Value);
            this.IsMultiSelect = false;
            this.IsStatic = false;
            this.AllowClear = true; 
        }
    }
}