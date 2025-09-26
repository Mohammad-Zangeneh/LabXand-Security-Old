module nodak.common.models {
    export class PermissionLookup extends labxand.components.core.TreeLookupBase<common.models.Permission>
    {
        constructor(id: string) {
            super("l", "common.permission.selectorController", "/Template/common/permission.selector.html", id, "Permission", enums.SubSystems.Common)

            this.SetSelectedTextField(() => this.Model.Title);
            this.SetSelectedIdField(() => this.Model.Id)
            this.staticLoad = true;
        }
    }
}