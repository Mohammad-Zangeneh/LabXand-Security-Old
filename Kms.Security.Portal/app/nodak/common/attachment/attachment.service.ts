module nodak.common.services {
    export class AttachmentService extends nodak.service.ServiceBase<string>
    {
        constructor($http) {
            super($http, Base.Config.ServiceRoot + '/api/attachment');
        }
    }
}
commonService.service('common.service.attachment', nodak.common.services.AttachmentService);