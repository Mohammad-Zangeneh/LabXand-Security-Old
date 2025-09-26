module nodak.common.services {
    export class MainService extends nodak.service.ServiceBase<any>{
        static $inject = ["$http"];
        constructor(http) {
            super(http, Base.Config.ServiceRoot + "/api/Main");
        }
    }

}
angular.module("common.services").service("common.service.main", nodak.common.services.MainService);