module nodak.common.controllers {
    export class PermissionCategorySelectorController extends nodak.core.TreeSearchControllerBase<models.PermissionCategory> {
        static $inject = ['$injector', 'common.service.permissionCategory'];
        constructor(injector: ng.auto.IInjectorService, permissionCategoryService) {
            super(injector, "PermissionCategory", enums.SubSystems.Common);
            this.treeEntity = new models.PermissionCategoryTree(permissionCategoryService);
        }
    }
}
angular.module("common.controllers").controller("common.permissionCategory.selectorController", nodak.common.controllers.PermissionCategorySelectorController);