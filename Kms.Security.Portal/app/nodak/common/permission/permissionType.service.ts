module nodak.common.services {
    export class PermissionTypeService extends nodak.service.ServiceBase<models.PermissionTypeDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/PermissionType");
        }
    }
}
angular.module("common.services").service("common.service.permissionType", nodak.common.services.PermissionTypeService);