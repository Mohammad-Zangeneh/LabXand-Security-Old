module nodak.common.controllers {
    export class EnterprisePositionSelectorController extends nodak.core.TreeSearchControllerBase<models.EnterprisePosition> {
        static $inject = ['$injector','common.service.enterprisePosition'];
        constructor(injector: ng.auto.IInjectorService, enterprisePositionService) {
            super(injector, "EnterprisePosition", enums.SubSystems.Common);
            this.treeEntity = new models.EnterprisePositionTree(enterprisePositionService);
        }
    }
}
angular.module("common.controllers").controller("common.enterprisePosition.selectorController", nodak.common.controllers.EnterprisePositionSelectorController);