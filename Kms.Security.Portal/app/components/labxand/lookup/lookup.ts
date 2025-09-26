module labxand.components {
    interface ILookupScope<T extends nodak.models.ModelBase, U> extends ng.IScope {
        operation: core.ILookupBase<T, U>;
        selectedText: string;
        selectedId: string;
        changedBody: boolean;
    }

    export class LookupDirective implements ng.IDirective {
        $inject = ['$compile', '$rootScope'];
        constructor(public $compile, public $rootScope) {
        }

        template = `
<div class='input-group'>
    <input Id='{{operation.idInput}}' ng-model="selectedText" ng-disabled="operation.disableCaption" type='text'>
     <button id='{{operation.idModalButton}}' style='border-radius:0px;' class='btn-primary btn btn-sm' ng-click='operation.Show()'>{{btnCaption}}<i class ="fa fa-ellipsis-h"/></button>
</div>
<div class="modal labxand-fade in nodakModal" id="{{operation.id}}Parent" >
    <div class="modal-dialog modal-95">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" ng-click='operation.Close()'>&times;</button>
                <h4 class="modal-title" id="myModalLabel">{{caption}}</h4>
                <div class="">
                    <a ng-click="operation.OnSelected()" class="btn btn-sm btn-success">تایید</a>
                    <a ng-click="operation.Close()" class="btn btn-sm btn-danger">خروج</a>
                </div>
            </div>
            <div class="modal-body" id="{{operation.id}}">
            </div>
            <div class="modal-footer">
            </div>
        </div>
    </div>
</div>
`;
        restrict = 'E';
        scope = {
            operation: '=',
            selectedText: '=',
            selectedId: '='
        };
        link = (scope: ILookupScope<any, any>, element: ng.IAugmentedJQuery) => {
            let self = this;
            scope.operation.Close = () => {
                document.getElementById(scope.operation.id + 'Parent').style.display = "none";
                //nodak.DOMManipulating.EnableScreen();
                if (scope.changedBody)
                    angular.element("body").removeClass("modal-open");
                scope.operation.AfterClose();
            }

            scope.operation.SetSelectedTextAndId = () => {
                scope.selectedText = scope.operation.selectedText;
                scope.selectedId = scope.operation.selectedId;
                scope.$apply();
            };
            //morsa 
            if (scope.operation.staticLoad) {
                
                angular.element(document).ready(() => {
                    scope.operation.LoadContent();
                    this.$rootScope.$broadcast(scope.operation.eventName);
                });

                this.$rootScope.$on(scope.operation.eventName, function () {
                    if (!scope.operation.eventFired) {
                        window.setTimeout(() => {
                            scope.operation.eventFired = true;
                            scope.operation.SetController();
                            scope.$apply();
                        }, 1000);
                    }
                });
            }
            //end
            scope.operation.Show = () => {
                if (!angular.element("body").hasClass("modal-open")) {
                    scope.changedBody = true;
                    angular.element("body").addClass("modal-open");
                }

                this.$rootScope.$on(scope.operation.eventName, function () {
                    if (!scope.operation.eventFired) {
                        window.setTimeout(() => {
                            scope.operation.eventFired = true;
                            scope.operation.AfterCompile();
                            scope.$apply();
                        }, 1000);
                    }
                });

                if (!scope.operation.isLoaded) {
                    scope.operation.LoadContent();
                }
                else {
                    if (scope.operation.ControllerNotAssigned())
                        scope.operation.SetController();
                    scope.operation.AfterShow();
                    document.getElementById(scope.operation.id + 'Parent').style.display = "block";
                }

                //nodak.DOMManipulating.DisableScreen();
            }
            scope.operation.LoadContent = () => {
                if (scope.operation.isLoaded != true && scope.operation.id != null) {
                    scope.operation.isLoaded = true;
                    angular.element(document.getElementById(scope.operation.id)).append(self.$compile(
                        '<div partial-view="' + scope.operation.contenUrl + '" ></div> ')(scope));
                }
            }
        }

    }

}
angular.module('nodak.components').directive('labxandLookup', ($compile, $rootScope) => { return new labxand.components.LookupDirective($compile, $rootScope); });
