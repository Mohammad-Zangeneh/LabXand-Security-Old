module nodak.common.controllers {
    export class CompanyController extends nodak.core.CrudControllerBase<models.Company, models.CompanyDto>{
        gridEntity: models.CompanyGrid;
        searchModel: models.CompanySearchModel;
        gridActionPanel: labxand.components.core.IActionPanel;
        static $inject = ["$injector", "$scope", "common.service.company", "common.company.mapper"
        ];
        constructor(injector, scope, companyService, mapper) {
            super(injector, scope, companyService, mapper, enums.SubSystems.Common, "Company", "مدیریت شرکت");
            this.currentEntity = new models.Company();
            this.gridEntity = new models.CompanyGrid(injector, companyService);
            this.searchModel = new models.CompanySearchModel();
            this.gridEntity.Search(this.searchModel);
            this.gridActionPanel = new labxand.components.core.ActionPanelBase();
            this.gridActionPanel.AddButton("resetForm").HasCaption("پاک کردن / انصراف").SetOnClick(() => this.ResetForm()).HasClass(nodak.enums.NodakCss.CancelBtn);
            this.gridActionPanel.AddButton("resetForm").HasCaption("ویرایش").SetOnClick(() => this.Edit()).HasClass(nodak.enums.NodakCss.Update);

        }
        AfterGetDetails() { }
        BeforSave() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            if (this.currentEntity.Name == null || this.currentEntity.Name == "") {
                this.messageBox.OkButton("خطا","نام سازمان اجباری است");
                defer.reject();
            }
            else
                defer.resolve();
            return defer.promise;
        }
        AfterSave() {
            this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
            this.gridEntity.Search(this.searchModel);
            this.ResetForm();
        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;

        }

        ResetForm() {
            this.currentEntity = new models.Company();
            this.gridEntity.SelectedRow = null;
            this.gridEntity.ClearSelected();
        }
        Edit() {
            if (this.gridEntity.SelectedRow == null)
            {
                this.messageBox.OkButton("خطا", "سطری برای ویرایش انتخاب نشده است");
                return;
            }

            this.currentEntity = this.mapper.MapToEntity(this.mapper.MapToDto(this.gridEntity.SelectedRow));
        }
    }
}
angular.module("common.controllers").controller("common.company.controller", nodak.common.controllers.CompanyController);