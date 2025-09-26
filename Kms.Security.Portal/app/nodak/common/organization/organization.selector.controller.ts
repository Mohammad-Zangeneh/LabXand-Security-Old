module nodak.common.controllers {
    export class OrganizationSelectorController extends nodak.core.TreeSearchControllerBase<models.Organization> {
        static $inject = ['$injector', "common.service.organization"];
        constructor(injector: ng.auto.IInjectorService,service) {
            super(injector, "Organization", enums.SubSystems.Common);
            this.treeEntity = new common.models.OrganizationTree(service);
        }
    }
}
angular.module('common.controllers').controller('common.organization.selectorController', nodak.common.controllers.OrganizationSelectorController); 