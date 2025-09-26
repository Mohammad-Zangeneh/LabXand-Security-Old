module nodak.common.controllers {
    export class RoleController extends nodak.core.CrudControllerBase<models.Role, models.RoleDto>{
        PermissionTree: models.PermissionTree;
        gridEntity: models.RoleGrid;
        CompanyCombo: models.CompanyCombo;
        searchModel: models.RoleSearchModel;
        static $inject = ["$injector", "$scope", "common.service.role", "common.role.mapper", "common.service.permission", "common.service.company"];
        constructor(injector, scope, roleService, mapper, permissionService, companyService) {
            super(injector, scope, roleService, mapper, enums.SubSystems.Common, "Role", "مدیریت نقش ها", false);
            this.currentEntity = new models.Role();;
            this.PermissionTree = new models.PermissionTree(permissionService, true, true);
            this.gridEntity = new models.RoleGrid(injector, roleService);
            this.searchModel = new models.RoleSearchModel();
            this.gridEntity.Search(this.searchModel);
            this.actionPanel.AddButton("resetForm").HasCaption("پاک کردن / انصراف").SetOnClick(() => this.Reset()).HasClass(nodak.enums.NodakCss.CancelBtn);
            //this.actionPanel.AddButton("resetForm").HasCaption("ویرایش").SetOnClick(() => this.Edit()).HasClass(nodak.enums.NodakCss.Update);
            this.CompanyCombo = new models.CompanyCombo(companyService);
        }
        AfterGetDetails() { }
        BeforSave() {
            let permissionRoles = Array<models.PermissionRole>();
            if (this.PermissionTree.SelectedRows != null)
                this.PermissionTree.SelectedRows.forEach((item) => {
                    let temp = new models.PermissionRole();
                    temp.RoleId = this.currentEntity.Id;
                    temp.PermissionId = item.Id;
                    temp.Value = 1;
                    permissionRoles.push(temp);
                });

            this.currentEntity.Permissions = permissionRoles;
            if (this.CompanyCombo.SelectedItems!= undefined && this.CompanyCombo.SelectedItems != [] && this.CompanyCombo.SelectedItems.length > 0 && this.CompanyCombo.SelectedItems[0] != null)
                this.currentEntity.CompanyId = this.CompanyCombo.SelectedItems[0].Id;
            else
                this.currentEntity.CompanyId = null;
            let defer: ng.IDeferred<{}> = this.$q.defer();

            defer.resolve();
            return defer.promise;
        }
        AfterSave() {
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.Reset();
            this.gridEntity.Search(this.searchModel);

        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;

        }
        Reset() {
            this.currentEntity = new models.Role();
            this.PermissionTree.ClearSelected();
            this.gridEntity.ClearSelected();
            this.gridEntity.SelectedRow = null;
            this.CompanyCombo.Clear();
        }
        Edit() {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ویرایش انتخاب نشده است");
                return;
            }
            this.currentEntity = angular.copy(this.gridEntity.SelectedRow);
            let selectedPermission = new Array<models.Permission>();
            this.currentEntity.Permissions.forEach((item) => {
                let temp = new models.Permission();
                temp.Id = item.PermissionId;
                selectedPermission.push(temp);
            });
            this.PermissionTree.SelectedRows = selectedPermission;
            this.CompanyCombo.SelectedItems = [this.currentEntity.Company];
            let currentTemp = this.mapper.MapToDto(this.currentEntity);
            this.currentEntity = this.mapper.MapToEntity(currentTemp);
        }

    }
}
angular.module("common.controllers").controller("common.role.controller", nodak.common.controllers.RoleController);