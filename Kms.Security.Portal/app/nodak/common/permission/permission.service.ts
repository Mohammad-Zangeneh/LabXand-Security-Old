module nodak.common.services {
    export class PermissionService extends nodak.service.ServiceBase<models.PermissionDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/Permission");
        }
    }
}
angular.module("common.services").service("common.service.permission", nodak.common.services.PermissionService);