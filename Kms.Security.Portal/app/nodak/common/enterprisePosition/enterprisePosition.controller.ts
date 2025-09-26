module nodak.common.controllers {
    export class EnterprisePositionController extends nodak.core.CrudControllerBase<common.models.EnterprisePosition, common.models.EnterprisePositionDto> {
        organizationTreeEntity: models.OrganizationTree;
        enterprisePositionTreeEntity: models.EnterprisePositionTree;
        modalEnterprisePosition: labxand.components.core.ModalSectionOperation;

        organizationLookup: labxand.components.ITreeLookup<models.Organization>;
        enterprisePositionLookup: labxand.components.ITreeLookup<models.EnterprisePosition>;
        static $inject = ["common.service.enterprisePosition", "$injector", "$scope", "common.service.organization", "common.organization.mapper"];
        constructor(service, injector: ng.auto.IInjectorService, scope: nodak.core.IScopeBase, organizationService: any, mapper) {
            super(injector, scope, service, mapper, nodak.enums.SubSystems.Common, "EnterprisePosition", "عملیات چارت سازمانی", false);
            this.currentEntity = new common.models.EnterprisePosition();
            this.organizationTreeEntity = new models.OrganizationTree(organizationService);
            this.enterprisePositionTreeEntity = new models.EnterprisePositionTree(service);
            this.modalEnterprisePosition = new labxand.components.core.ModalSectionOperation("چارت سازمانی جدید");
            this.organizationTreeEntity.AfterSelect = (item: models.Organization) => { this.bindEnterprisePositionTree(item); }
            this.organizationLookup = new nodak.common.models.OrganizationLookup("Organizationss");
            this.enterprisePositionLookup = new nodak.common.models.EnterprisePositionLookup("EnterprisePositionLookup");

            this.modalEnterprisePosition.ConfirmButtonClick = () => { this.Save(); }
            this.actionPanel.buttons = [];
            this.actionPanel.AddButton("newEnterprisePosition").HasCaption("چارت سازمانی جدید").SetOnClick(
                () => { this.ShowNewEnterprisePositionModal(); }).HasClass(nodak.enums.NodakCss.OKBtn);
            this.actionPanel.AddButton("editEnterprisePosition").HasCaption("ویرایش").SetOnClick(
                () => { this.edit(); }).HasClass(nodak.enums.NodakCss.Update);

            this.organizationLookup.AfterSelect = (item) => {
                this.enterprisePositionLookup.Clear();
                this.enterprisePositionLookup.treeController.treeEntity.ClearSelected();
                this.enterprisePositionLookup.treeController.treeEntity.Data = item.EnterprisePositions;
                this.enterprisePositionLookup.treeController.treeEntity.Refresh();
                
            }
        }

        private bindEnterprisePositionTree(item: models.Organization) {
            if (item != null)
                this.enterprisePositionTreeEntity.Data = angular.copy(item.EnterprisePositions);
            else
                this.enterprisePositionTreeEntity.Data = null;
            this.enterprisePositionTreeEntity.SelectedRow = null;
            this.enterprisePositionTreeEntity.Refresh();
        }
        BeforSave() {
            let deferred: ng.IDeferred<{}> = this.$q.defer();
            if (this.currentEntity.Name == "" || this.currentEntity.Name == undefined) {
                this.messageBox.OkButton("ثبت اطلاعات", "ورود نام اجباری است");
                deferred.reject();
            }
            else if (this.organizationLookup.treeController.treeEntity.SelectedRow == null) {
                this.messageBox.OkButton("ثبت اطلاعات", "انتخاب سازمان اجباری است");
                deferred.reject();
            }
            else {
                this.currentEntity.OrganizationId = this.organizationLookup.treeController.treeEntity.SelectedRow.Id;
                this.currentEntity.ParentId = this.enterprisePositionLookup.treeController.treeEntity.SelectedRow != null ?
                    this.enterprisePositionLookup.treeController.treeEntity.SelectedRow.Id : null;
                deferred.resolve();

            }
            return deferred.promise;
        }
        AfterSave() {
            debugger;
            this.currentEntity = new models.EnterprisePosition();
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.modalEnterprisePosition.CloseModal();

            let selectItem = this.enterprisePositionTreeEntity.SelectedRow;

            this.organizationTreeEntity.DataBind();
            this.organizationLookup.treeController.treeEntity.DataBind();
            this.enterprisePositionTreeEntity.SelectedRow = selectItem;
            setTimeout(() => {
                this.enterprisePositionTreeEntity.SetSelectedItem(selectItem);
            }, 500);
            this.ResetForm();
        }
        AfterGetDetails() { }
        AddBussinessControllerValidation() {
            let deferred: ng.IDeferred<{}> = this.$q.defer();
            deferred.resolve();
            return deferred.promise;
        }
        ResetForm() {
            //this.enterprisePositionLookup.Clear();
            //this.enterprisePositionLookup.treeController.treeEntity.ClearSelected();
            //this.enterprisePositionLookup.treeController.treeEntity.Data = [];
          //  this.organizationLookup.Clear();
          //  this.organizationLookup.treeController.treeEntity.ClearSelected();
        }
        private edit() {
            if (this.enterprisePositionTreeEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "گزینه ای برای ویرایش انتخاب نشده است");
                return;
            }

            this.currentEntity = this.mapper.MapToEntity(angular.copy(this.enterprisePositionTreeEntity.SelectedRow));
            let temp = angular.copy(this.organizationTreeEntity.SelectedRow);
            this.organizationLookup.treeController.treeEntity.SelectedRow = temp;
            this.organizationLookup.OnSelected();
            this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = angular.copy(this.enterprisePositionTreeEntity.SelectedRow.Parent);
            this.enterprisePositionLookup.OnSelected();
            this.modalEnterprisePosition.ShowModal();
        }
        ShowNewEnterprisePositionModal() {
            this.currentEntity = new models.EnterprisePosition();
            if (this.organizationTreeEntity.SelectedRow != null) {
                let temp = angular.copy(this.organizationTreeEntity.SelectedRow);
                this.organizationLookup.treeController.treeEntity.SelectedRow = temp;
                this.organizationLookup.OnSelected();
            }
            if (this.enterprisePositionTreeEntity.SelectedRow != null) {
                this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = angular.copy(this.enterprisePositionTreeEntity.SelectedRow);
                this.enterprisePositionLookup.OnSelected();
            }
            this.modalEnterprisePosition.ShowModal();
        }

    }
}
angular.module("common.controllers").controller("common.enterprisePosition.controller", nodak.common.controllers.EnterprisePositionController);