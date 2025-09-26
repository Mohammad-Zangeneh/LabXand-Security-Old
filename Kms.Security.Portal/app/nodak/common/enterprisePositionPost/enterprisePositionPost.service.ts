module nodak.common.services {
    export class EnterprisePositionPostService extends nodak.service.ServiceBase<models.OrganizationDto>{
        static $inject = ["$http"];
        constructor($http) {
            super($http, Base.Config.ServiceRoot + "/api/EnterprisePositionPost");
        }
    }
}
angular.module("common.services").service("common.service.enterprisePositionPost", nodak.common.services.EnterprisePositionPostService);