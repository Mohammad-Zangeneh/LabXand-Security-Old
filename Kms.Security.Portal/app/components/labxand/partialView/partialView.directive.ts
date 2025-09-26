module labxand.components {
    export function PartialView(): ng.IDirective {
        return {
            templateUrl: function (elm, attrs) { return Base.Config.AppRoot + attrs.partialView },
            
            restrict: 'A',
            link(scope: ng.IScope, element: ng.IAugmentedJQuery) {
            }
        }
    }
}

appDirectivesModule.directive('partialView', () => { return  labxand.components.PartialView() });
