module nodak.common.services {
    export class RoleService extends nodak.service.ServiceBase<models.RoleDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/Role");
        }
    }
}
angular.module("common.services").service("common.service.role", nodak.common.services.RoleService);