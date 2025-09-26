module nodak.common.services {
    export class TagService extends nodak.service.ServiceBase<nodak.common.models.TagDto>
    {
        static $inject = ['$http'];

        constructor($http: ng.IHttpService) {
            super($http, Base.Config.ServiceRoot + '/api/tag');

        }
    }
    
}
appServicesModule.directive('tagService', nodak.common.services.TagService);