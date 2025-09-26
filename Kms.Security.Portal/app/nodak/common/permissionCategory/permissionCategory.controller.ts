module nodak.common.controllers {
    export class PermissionCategoryController extends nodak.core.CrudControllerBase<models.PermissionCategory, models.PermissionCategoryDto>{
        CompanyCombo: models.CompanyCombo;
        permissionCategoryLookup: models.PermissionCategoryLookup;
        PermissionCategoryTree: models.PermissionCategoryTree;
        PermissionTypeCombo: models.PermissionTypeCombo;

        static $inject = ["$injector", "$scope", "common.service.permissionCategory", "common.permissionCategory.mapper", "common.service.company", "common.service.permissionType"];
        constructor(injector, scope, permissionCategoryService, mapper, companyService, permissionTypeService) {
            super(injector, scope, permissionCategoryService, mapper, enums.SubSystems.Common, "PermissionCategory", "ثبت دسته بندی دسته بندی دسترسی ها", false);
            this.currentEntity = new models.PermissionCategory();
            this.actionPanel.AddButton("resetPermissionCategoryForm").HasCaption("پاک کردن فرم").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("پاک کردن فرم")
                .SetOnClick(() => { this.ResetForm(); });
            this.PermissionCategoryTree = new models.PermissionCategoryTree(permissionCategoryService);
            this.CompanyCombo = new models.CompanyCombo(companyService);
            this.permissionCategoryLookup = new models.PermissionCategoryLookup("permissionCategorySe");
            this.PermissionTypeCombo = new models.PermissionTypeCombo(permissionTypeService);
        }
        AfterGetDetails() { }
        BeforSave() {
            let defer: ng.IDeferred<{}> = this.$q.defer();

            this.currentEntity.CompanyId = this.CompanyCombo.SelectedItem;
            this.currentEntity.ParentId = this.permissionCategoryLookup.selectedId;
            this.currentEntity.PermissionType = this.PermissionTypeCombo.SelectedItem;

            if (this.permissionCategoryLookup.selectedId != null || this.permissionCategoryLookup.selectedId != undefined)
                if (this.currentEntity.CompanyId != this.permissionCategoryLookup.treeController.treeEntity.SelectedRow.CompanyId) {
                    defer.reject();
                    this.messageBox.OkButton("خطا", "سازمان انتخاب شده با سازمان سرشاخه باید برابر باشد");
                    return defer.promise;
                }
            

            defer.resolve();
            return defer.promise;
        }
        AfterSave() {
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.PermissionCategoryTree.DataBind();
            this.ResetForm();
        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;
        }

        Edit() {

        }
        ResetForm() {
            this.currentEntity = new models.PermissionCategory();

        }

    }
}
angular.module("common.controllers").controller("common.permissionCategory.controller", nodak.common.controllers.PermissionCategoryController);