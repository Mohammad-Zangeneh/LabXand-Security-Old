module nodak.common.models {
    export class OrganizationLookup extends labxand.components.core.TreeLookupBase<Organization>{
        constructor(id: string) {
            super("c", "common.organization.selectorController", "/Template/common/organizationSelector.html", id, "Organization", enums.SubSystems.Common)
            this.SetSelectedTextField(() => this.Model.Name);
            this.SetSelectedIdField(() => this.Model.Id);
            this.staticLoad = true;
        }
    }
}
