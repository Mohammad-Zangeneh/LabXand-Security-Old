module nodak.common.services {
    export class DynamicPermissionService extends nodak.service.ServiceBase<models.DynamicPermissionDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/DynamicPermission");
        }
    }
}
angular.module("common.services").service("common.service.dynamicPermission", nodak.common.services.DynamicPermissionService);