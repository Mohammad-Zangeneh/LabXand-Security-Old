module nodak.common.models {
    export class EnterprisePositionLookup extends labxand.components.core.TreeLookupBase<EnterprisePosition>{
        constructor(id: string) {
            super("l", "common.enterprisePosition.selectorController", "/Template/common/enterprisePositionSelector.html", id, "EnterprisePosition", enums.SubSystems.Common)
            this.SetSelectedTextField(() => this.Model.Name);
            this.SetSelectedIdField(() => this.Model.Id);
            this.staticLoad = true;
        }
    }
}
