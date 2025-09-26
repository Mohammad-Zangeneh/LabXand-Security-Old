module nodak.common.controllers {
    export class LoginController extends nodak.core.CrudControllerBase<models.Member, models.MemberDto>{

        static $inject = ["$injector", "$scope", "common.service.member", "common.member.mapper"];
        constructor(injector, scope, memberService, mapper) {
            super(injector, scope, memberService, mapper, enums.SubSystems.Common, "Login", "ورود کاربران", false);
            this.currentEntity = new models.Member();
            window.localStorage.clear();

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
        Login() {

            if (this.currentEntity.Password != "" && this.currentEntity.UserName != "")
                this.service.PostLogin(this.currentEntity);
            else
                alert("نام کاربری و رمز ورود اجباری است");
        }
        Logout() {
            alert("ok");
        }

        Register() {
            let url = Base.Config.AppRoot + "/RegisterRequest";
            let redirectPath = this.GetParameterByName("redirectPath");
            if (redirectPath != null)
                url += "?redirectPath=" + redirectPath;
            window.location.href = url;
        }
        LoginPage() {
            let url = Base.Config.LoginPage;
            let redirectPath = this.GetParameterByName("redirectPath");
            if (redirectPath != null)
                url += "?redirectPath=" + redirectPath;
            window.location.href = url;
        }
    }
}
angular.module("common.controllers").controller("common.login.controller", nodak.common.controllers.LoginController);