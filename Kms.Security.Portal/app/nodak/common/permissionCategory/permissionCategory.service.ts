module nodak.common.services {
    export class PermissionCategoryService extends nodak.service.ServiceBase<models.PermissionCategoryDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/PermissionCategory");
        }
    }
}
angular.module("common.services").service("common.service.permissionCategory", nodak.common.services.PermissionCategoryService);