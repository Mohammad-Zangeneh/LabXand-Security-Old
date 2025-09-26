module nodak.common.models {
    export class RoleCombo extends labxand.components.core.ComboBase<models.Role>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedText(() => this.ModelEntity.Title);
            this.IsMultiSelect = true;
            this.IsStatic = false;
        }
    }
}