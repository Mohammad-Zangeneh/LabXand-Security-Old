module labxand.components {
    interface IPartialViewScope extends ng.IScope {
        operation: core.IPartialViewBase;
    }

    export class PartialViewDirective implements ng.IDirective {
        $inject = ['$compile', '$timeout', '$rootScope'];
        constructor(public $compile, public $timeout, public $rootScope) {
        }

        template = `
<div id="{{operation.id}}"></div>
`;
        restrict = 'E';
        scope = {
            operation: "="
        };
        link = (scope: IPartialViewScope, element) => {
            let self = this;

            // angular.element(document).ready(() => { scope.operation.AfterCompile(); console.log(new Date(), 'redayyy'); });
            scope.operation.LoadContent = () => {
                if (scope.operation.isLoaded != true && scope.operation.id != null) {
                    scope.operation.isLoaded = true;
                    angular.element(document.getElementById(scope.operation.id)).append(self.$compile(
                        '<div partial-view="' + scope.operation.contenUrl + '"></div>')(scope));
                }

                this.$rootScope.$on(scope.operation.eventName, function () {
                    if (!scope.operation.eventFired) {
                        window.setTimeout(() => {
                            scope.operation.AfterCompile();
                            scope.operation.eventFired = true;
                            scope.$apply();
                        }, 1500);
                    }
                });

                scope.$apply();
            }
            this.$timeout(() => {
                if (scope.operation.loadAutomatically)
                    scope.operation.LoadContent();
            }, 1000);
        }

    }

}
appDirectivesModule.directive('labxandPartialview', ($compile, $timeout, $rootScope) => { return new labxand.components.PartialViewDirective($compile, $timeout, $rootScope); });

