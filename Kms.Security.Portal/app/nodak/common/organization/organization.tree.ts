module nodak.common.models {
    export class OrganizationTree extends labxand.components.core.TreeBase<Organization>{
        constructor(service, CallFromService = true) {
            super(service);
            this.HasItemSelectedId(() =>this.ModelEntity.Id );
            this.HasItemSelectedParent(()=>this.ModelEntity.ParentId);
            this.HasItemSelectedText(() => this.ModelEntity.Name);
            this.MultiSelect = false;
            this.CallFromService = CallFromService; 
        }
    }
}