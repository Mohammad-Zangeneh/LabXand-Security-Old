var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var nodak;
(function (nodak) {
    var common;
    (function (common) {
        var services;
        (function (services) {
            var AttachmentService = (function (_super) {
                __extends(AttachmentService, _super);
                function AttachmentService($http) {
                    _super.call(this, $http, Base.Config.ServiceRoot + '/api/attachment');
                }
                return AttachmentService;
            }(nodak.service.ServiceBase));
            services.AttachmentService = AttachmentService;
        })(services = common.services || (common.services = {}));
    })(common = nodak.common || (nodak.common = {}));
})(nodak || (nodak = {}));
commonService.service('common.service.attachment', nodak.common.services.AttachmentService);
//# sourceMappingURL=attachment.service.js.map