module nodak.common.services {
    export class OrganizationService extends nodak.service.ServiceBase<models.OrganizationDto>{
        static $inject = ["$http"];
        constructor($http) {
            super($http, Base.Config.ServiceRoot + "/api/Organization");
        }
    }
}
angular.module("common.services").service("common.service.organization", nodak.common.services.OrganizationService);