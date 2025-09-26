module nodak.common.models {
    export class PermissionCategoryLookup extends labxand.components.core.TreeLookupBase<common.models.PermissionCategory>
    {
        constructor(id: string) {
            super("l", "common.permissionCategory.selectorController", "/Template/common/permissionCategoryselector.html", id, "PermissionCategory", enums.SubSystems.Common)

            this.SetSelectedTextField(() => this.Model.Name);
            this.SetSelectedIdField(() => this.Model.Id)
            this.staticLoad = true;
        }
    }
}