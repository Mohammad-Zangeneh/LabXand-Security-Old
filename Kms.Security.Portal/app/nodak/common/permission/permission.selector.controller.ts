module nodak.common.controllers {
    export class PermissionSelectorController extends nodak.core.TreeSearchControllerBase<models.Permission> {
        static $inject = ['$injector', 'common.service.permission'];
        constructor(injector: ng.auto.IInjectorService, permissionService) {
            super(injector, "Permission", enums.SubSystems.Common);
            this.treeEntity = new models.PermissionTree(permissionService);
        }
    }
}
angular.module("common.controllers").controller("common.permission.selectorController", nodak.common.controllers.PermissionSelectorController);