module nodak.common.models {
    export class CompanyCombo extends labxand.components.core.ComboBase<models.Company>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedText(() => this.ModelEntity.Name);
            this.IsMultiSelect = false;
            this.IsStatic = false;
            this.AllowClear = true; 
        }
    }
}