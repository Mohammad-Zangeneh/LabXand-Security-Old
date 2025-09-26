module nodak.common.controllers {
    export class PermissionController extends nodak.core.CrudControllerBase<models.Permission, models.PermissionDto>{
        CompanyCombo: models.CompanyCombo;
        PermissionTypeCombo: models.PermissionTypeCombo;
        permissionCategoryLookup: models.PermissionCategoryLookup;
        PermissionTree: models.PermissionTree;
        static $inject = ["$injector", "$scope", "common.service.permission", "common.permission.mapper", "common.service.company", "common.service.permissionType"];
        constructor(injector, scope, permissionService, mapper, companyService,permissionTypeService) {
            super(injector, scope, permissionService, mapper, enums.SubSystems.Common, "Permission", "ثبت دسترسی ها", false);
            this.currentEntity = new models.Permission();
            this.permissionCategoryLookup = new models.PermissionCategoryLookup("parentLookup");
            this.PermissionTree = new models.PermissionTree(permissionService);
            this.CompanyCombo = new models.CompanyCombo(companyService);
            this.PermissionTypeCombo = new models.PermissionTypeCombo(permissionTypeService);
        }
        AfterGetDetails() { }
        BeforSave() {

            this.currentEntity.PermissionCategoryId = this.permissionCategoryLookup.selectedId;
            this.currentEntity.CompanyId = this.CompanyCombo.SelectedItem;
            this.currentEntity.PermissionType = this.PermissionTypeCombo.SelectedItem;
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;
        }
        AfterSave() {
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.PermissionTree.DataBind();
            this.permissionCategoryLookup.treeController.treeEntity.DataBind();
            this.currentEntity = new models.Permission();
        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;

        }

    }
}
angular.module("common.controllers").controller("common.permission.controller", nodak.common.controllers.PermissionController);