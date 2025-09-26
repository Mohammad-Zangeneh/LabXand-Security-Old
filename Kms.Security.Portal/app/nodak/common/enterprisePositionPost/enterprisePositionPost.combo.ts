module nodak.common.models {
    export class EnterprisePositionPostCombo extends labxand.components.core.ComboBase<models.EnterprisePositionPost>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedText(() => this.ModelEntity.Title);
            this.IsMultiSelect = true;
            this.IsStatic = false;
            this.PlaceHolder = "سمت را انتخاب کنید";
            this.RTL = true;
            this.AllowClear = false;
            this.SelectOnClose = false; 
        }
    }
}