module nodak.common.models {
    export class EnterprisePositionTree extends labxand.components.core.TreeBase<EnterprisePosition>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedParent(() => this.ModelEntity.ParentId);
            this.HasItemSelectedText(() => this.ModelEntity.Name);
            this.HasItemSort(() => this.ModelEntity.SortingNumber); 
            this.MultiSelect = false;
            this.CallFromService = false;

        }
    }
}