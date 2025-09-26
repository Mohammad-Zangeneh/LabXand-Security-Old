module nodak.common.services {
    export interface IMemberService extends service.IServiceBase<models.MemberDto> {
        ResetPassword(model: models.MemberDto): ng.IPromise<Array<models.MemberDto>>;
        RegisterRequest(model: models.MemberDto): ng.IPromise<Array<models.MemberDto>>;
        ChangeStatus(model: models.MemberDto): ng.IPromise<models.MemberDto>;
        ChangePassword(model: models.ChangePasswordModel): ng.IPromise<any>;
        ChangeToSuperAdmin(model: models.MemberDto): ng.IPromise<any>;
        RemoveFromSuperAdmin(model: models.MemberDto): ng.IPromise<any>;
        DeleteRegisterRequest(model: models.MemberDto): ng.IPromise<any>;
    }


    export class MemberService extends nodak.service.ServiceBase<models.MemberDto> implements IMemberService {
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/Member");
        }
        ResetPassword(model: models.MemberDto) {
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/ResetPasswordByAdmin");
        }
        RegisterRequest(model: models.MemberDto) {
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/RegisterRequest");
        }
		ChangeStatus(model: models.MemberDto) { 
            return this.Post(model, enums.ServiceTypeEnum.Save);
        }

        ChangePassword(model: models.ChangePasswordModel) {
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/ChangePassword");
        }

        ChangeToSuperAdmin(model: models.MemberDto) {
            model.IsSuperAdmin = true;
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/ChangeSupperAdmin");
        }
        RemoveFromSuperAdmin(model: models.MemberDto) {
            model.IsSuperAdmin = false;
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/ChangeSupperAdmin");
        }
        DeleteRegisterRequest(model: models.MemberDto) {
            return this.GeneralPost(model, Base.Config.ServiceRoot + "/api/Member/DeleteRegisterRequest");
        }
    }
}
angular.module("common.services").service("common.service.member", nodak.common.services.MemberService);