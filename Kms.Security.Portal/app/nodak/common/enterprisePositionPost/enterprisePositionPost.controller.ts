module nodak.common.controllers {
    export class EnterprisePositionPostController extends nodak.core.CrudControllerBase<common.models.EnterprisePositionPost, common.models.EnterprisePositionPostDto> {
        gridEntity: models.EnterprisePositionPostGrid;
        searchModel: models.EnterprisePositionPostSearchModel;
        organizationLookup: labxand.components.ITreeLookup<models.Organization>;
        enterprisePositionLookup: labxand.components.ITreeLookup<models.EnterprisePosition>;
        PermissionTree: models.PermissionTree;

        //test: models.testt;

        static $inject = ["common.service.enterprisePositionPost", "$injector", "$scope", "common.enterprisePositionPost.mapper", "common.service.permission"];
        constructor(service, injector: ng.auto.IInjectorService, scope: nodak.core.IScopeBase, /*organizationService: any,*/ mapper, permissionService) {
            super(injector, scope, service, mapper, nodak.enums.SubSystems.Common, "EnterprisePositionPost", "عملیات جایگاه سازمانی", false);
            this.currentEntity = new common.models.EnterprisePositionPost();
            this.gridEntity = new models.EnterprisePositionPostGrid(injector, service);
            this.searchModel = new models.EnterprisePositionPostSearchModel();
            this.gridEntity.Search(this.searchModel);
            this.organizationLookup = new nodak.common.models.OrganizationLookup("Organizationss");
            this.enterprisePositionLookup = new nodak.common.models.EnterprisePositionLookup("EnterprisePositionLookup");
            this.organizationLookup.AfterSelect = (item) => {
                this.enterprisePositionLookup.treeController.treeEntity.Data = item.EnterprisePositions;
                this.enterprisePositionLookup.treeController.treeEntity.Refresh();
            }
            this.PermissionTree = new models.PermissionTree(permissionService, true, true);
            //this.test = new models.testt(service);

        }

        Edit() {

            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ویرایش انتخاب نشده است");
                return;
            }
            debugger;
            this.currentEntity = angular.copy(this.gridEntity.SelectedRow);
            let selectedPermission = new Array<models.Permission>();
            this.currentEntity.Permissions.forEach((item) => {
                let temp = new models.Permission();
                temp.Id = item.Id;
                selectedPermission.push(temp);
            });
            this.PermissionTree.SelectedRows = selectedPermission;
            let currentTemp = this.mapper.MapToDto(this.currentEntity);
            let temp = angular.copy(this.currentEntity.EnterprisePosition.Organization);
            this.organizationLookup.treeController.treeEntity.SelectedRow = temp;
            this.organizationLookup.OnSelected();
            this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = angular.copy(this.currentEntity.EnterprisePosition);
            this.enterprisePositionLookup.OnSelected();
            this.currentEntity = this.mapper.MapToEntity(currentTemp);
        }
        BeforSave() {
            let deferred: ng.IDeferred<{}> = this.$q.defer();

            let v = this.enterprisePositionLookup.treeController.treeEntity.SelectedRow;
            if (v == undefined || v == null) {
                this.messageBox.OkButton("خطا", "چارت سازمانی اجباری است");
                deferred.reject();
            }
            this.currentEntity.Permissions = this.PermissionTree.SelectedRows;
            this.currentEntity.EnterprisePositionId = v.Id;
            deferred.resolve();


           
            return deferred.promise;
        }
        AfterSave() {
            this.currentEntity = new models.EnterprisePositionPost();
            this.gridEntity.Search(this.searchModel);
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.enterprisePositionLookup.Clear();
            this.organizationLookup.Clear();
            this.PermissionTree.ClearSelected();
            this.gridEntity.ClearSelected();
        }
        AfterGetDetails() { }
        AddBussinessControllerValidation() {
            let deferred: ng.IDeferred<{}> = this.$q.defer();
            deferred.resolve();
            return deferred.promise;
        }
       
    }
}
angular.module("common.controllers").controller("common.enterprisePositionPost.controller", nodak.common.controllers.EnterprisePositionPostController);