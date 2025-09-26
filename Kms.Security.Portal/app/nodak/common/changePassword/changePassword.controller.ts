module nodak.common.controllers {
    export class ChangePasswordController extends nodak.core.CrudControllerBase<models.ChangePasswordModel, models.ChangePasswordModel>{

        static $inject = ["$injector", "$scope", "common.service.member", "common.service.member", "common.member.mapper"];
        constructor(injector, scope, service, private memberService: nodak.common.services.IMemberService, mapper) {
            super(injector, scope, service, mapper, enums.SubSystems.Common, "ChangePassword", "تغییر رمز ورود", false);
            this.currentEntity = new models.ChangePasswordModel();

        }
        AfterGetDetails() { }
        BeforSave() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;
        }
        AfterSave() {
        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;

        }
        ChangePassword() {
            if (this.currentEntity.ConfirmPassword == undefined || this.currentEntity.Password == undefined || this.currentEntity.CurrentPassword == undefined) {
                alert("تمام فیلد ها اجباری می باشد");
                return;
            }
            if (this.currentEntity.Password != this.currentEntity.ConfirmPassword) {
                alert("رمز ورود و تکرار آن برابر نیست");
                return;
            }
            this.memberService.ChangePassword(this.currentEntity).then((test) => {
                alert("رمز با موفقیت تغییر پیدا کرد، به صفحه لاگین منتقل می شوید");
                let url = this.GetParameterByName("redirectPath");
                if (url != null)
                    window.location.href = Base.Config.LoginPage + "?redirectPath=" + url;
                else
                    window.location.href = Base.Config.LoginPage;
            })
                .catch((error) => {
                    //console.log("change password", error);
                    alert("تغییر با خطا مواجه شد\n" + error.data.ExceptionMessage);
                });
        }
        BackUrl() {
            let url = this.GetParameterByName("redirectPath");
            if (url != null)
                window.location.href = url;
            else
                window.location.href = Base.Config.AppRoot;
        }

    }
}
angular.module("common.controllers").controller("common.changePassword.controller", nodak.common.controllers.ChangePasswordController);