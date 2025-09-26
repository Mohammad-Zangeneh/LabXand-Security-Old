module nodak.common.controllers {
    export class OrganizationController extends nodak.core.CrudControllerBase<common.models.Organization, common.models.OrganizationDto> {
        TreeEntity: models.OrganizationTree;
        titleAction = "ثبت سازمان جدید";
        organizationLookup: labxand.components.ITreeLookup<models.Organization>;
        gridActionPanel: labxand.components.core.IActionPanel;
        static $inject = ["$injector", "$scope", "common.service.organization", "common.organization.mapper",'$rootScope'];
        constructor(injector: ng.auto.IInjectorService, scope: nodak.core.IScopeBase, service: any, mapper, $rootScope) {
            super(injector, scope, service, mapper, nodak.enums.SubSystems.Test, "Organization", "مدیریت سازمان");
            this.currentEntity = new common.models.Organization();
            this.TreeEntity = new models.OrganizationTree(service);
            this.TreeEntity.build();
            this.organizationLookup = new nodak.common.models.OrganizationLookup("Organizationss");
            this.titleAction = "ثبت سازمان جدید";
            this.gridActionPanel = new labxand.components.core.ActionPanelBase();
            this.actionPanel.AddButton("resetForm").HasCaption("پاک کردن / انصراف").SetOnClick(() => this.Clear()).HasClass(nodak.enums.NodakCss.CancelBtn);
            this.gridActionPanel.AddButton("editOrganization").HasCaption("ویرایش").SetOnClick(() => this.Update()).HasClass(nodak.enums.NodakCss.Update);

        }
        BeforSave() {
            if (this.organizationLookup.treeController.treeEntity != undefined
                && this.organizationLookup.treeController.treeEntity.SelectedRow != undefined && this.organizationLookup.treeController.treeEntity.SelectedRow != null)
                this.currentEntity.ParentId = this.organizationLookup.treeController.treeEntity.SelectedRow.Id;
            else
                this.currentEntity.ParentId = null;
            let deferred: ng.IDeferred<{}> = this.$q.defer();
            if (this.currentEntity.Name == "" || this.currentEntity.Name == undefined) {
                this.messageBox.OkButton("ثبت اطلاعات", "ورود نام اجباری است");
                deferred.reject();
            }
            else
                deferred.resolve();
            return deferred.promise;
        }
        AfterSave() {
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.TreeEntity.DataBind();
            this.organizationLookup.treeController.treeEntity.DataBind();
            this.Clear();
        }
        AfterGetDetails() { }
        AddBussinessControllerValidation() {
            let deferred: ng.IDeferred<{}> = this.$q.defer();
            deferred.resolve();
            return deferred.promise;
        }
        Clear() {
            this.currentEntity = new models.Organization();
            this.organizationLookup.Clear();
            this.titleAction = "ثبت سازمان جدید";
        }
        Update() {
            if (this.TreeEntity.SelectedRow == null) {
                this.messageBox.OkButton("ویرایش اطلاعات", "موردی برای ویرایش انتخاب نشده است");
                return;
            }
            this.titleAction = "ویرایش اطلاعات";
            this.currentEntity = this.mapper.MapToEntity(angular.copy(this.TreeEntity.SelectedRow));
            this.organizationLookup.treeController.treeEntity.SelectedRow = this.currentEntity.Parent;
            if (this.currentEntity.ParentId != null)
                this.organizationLookup.OnSelected();

        }
    }
}
angular.module("common.controllers").controller("common.organization.controller", nodak.common.controllers.OrganizationController);