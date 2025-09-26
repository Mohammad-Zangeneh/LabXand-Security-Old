module nodak.common.services {
    export class EnterprisePositionService extends nodak.service.ServiceBase<models.OrganizationDto>{
        static $inject = ["$http"];
        constructor($http) {
            super($http, Base.Config.ServiceRoot + "/api/EnterprisePosition");
        }
    }
}
angular.module("common.services").service("common.service.enterprisePosition", nodak.common.services.EnterprisePositionService);