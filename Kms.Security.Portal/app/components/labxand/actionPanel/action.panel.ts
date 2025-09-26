module labxand.components {
    interface IActionPanelScope extends ng.IScope {
        operation: core.IActionPanel;
    }

    export class ActionPanelDirective implements ng.IDirective {
        $inject = ['$compile', '$rootScope'];
        constructor(public $compile, public $rootScope) {
        }
        replace = true;
        template = `
<div ng-show="operation.visible">
<h3 ng-bind="operation.panelTitle"></h3>
<div  id="" class="row" >
    <button  ng-repeat="btn in operation.buttons" ng-disabled="btn.disabled"  ng-show="btn.visible" title="{{btn.title}}" ng-click="btn.OnClick()" class="{{btn.className}}" style="margin:8px">{{btn.caption}} </button>
</div>
<div>
`;
        restrict = 'E';
        scope = {
            operation: '=',
            selectedText: '=',
            selectedId: '='
        };
        link = (scope: IActionPanelScope, element: ng.IAugmentedJQuery) => {
        }
    }

}
angular.module('nodak.components').directive('actionPanel', ($compile, $rootScope) => { return new labxand.components.ActionPanelDirective($compile, $rootScope); });
