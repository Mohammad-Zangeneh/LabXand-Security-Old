module nodak.common.services {
    export class CompanyService extends nodak.service.ServiceBase<models.CompanyDto>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/Company");
        }
    }
}
angular.module("common.services").service("common.service.company", nodak.common.services.CompanyService);