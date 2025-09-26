module labxand.components {
    interface IModalScope extends ng.IScope {
        operation: core.IModalBase;
        mytext: string;
        $apply(): any;
        $apply(exp: string): any;
        $apply(exp: (scope: ng.IScope) => any): any;

        $applyAsync(): any;
        $applyAsync(exp: string): any;
        $applyAsync(exp: (scope: ng.IScope) => any): any;

    }

    export class ModalDirective implements ng.IDirective {
        $inject = ['$compile', '$rootScope'];
        constructor(public $compile, public $rootScope) {
        }

        template = `
<div>
    <span id='{{operation.idModalButton}}' class='input-group-addon btnSpan' ng-click='operation.Show()'>{{btnCaption}}</span>
</div>
<div class="nodakModal" id="{{operation.id}}Parent" >
   
        <div class="nodakModal-content">
            <div class="nodakModal-header panel-heading  text-right">
                <span ng-click='operation.Close()' class="nodakClose">&times;</span>
                <action-panel operation="operation.actionPanel"></action-panel>
            </div>
            <div class="nodakModal-body text-right" id="{{operation.id}}">
            </div>
            <div class="nodakModal-footer text-left">
            </div>
        </div>
   
</div>
`;
        restrict = 'E';
        scope = {
            operation: '=',
            selectedText: '=',
            selectedId: '=',
            mytext:'='
        };

        link = (scope: IModalScope, element: ng.IAugmentedJQuery) => {
           
            let self = this;
          

            scope.operation.LoadContent = () => {
            
                if (scope.operation.isLoaded != true && scope.operation.id != null) {
                    scope.operation.isLoaded = true;
                    
                    angular.element(document.getElementById(scope.operation.id)).append(self.$compile(
                        '<div partial-view="' + scope.operation.contentUrl + '" ></div> ')(scope));
                }
            }

            scope.operation.Close = () => {

                document.getElementById(scope.operation.id + 'Parent').style.display = "none";
                nodak.DOMManipulating.EnableScreen();
                scope.operation.AfterClose();

               
            }

            scope.operation.Show = () => {
                if (!scope.operation.isLoaded) {
                    scope.operation.LoadContent();
                    if (scope.operation.withController) {
                        //alert("modalBase " + scope.operation.eventName);
                       
                        this.$rootScope.$on(scope.operation.eventName, function () {
                            if (!scope.operation.eventFired) {
                                window.setTimeout(() => {
                                    scope.operation.eventFired = true;
                                    scope.operation.AfterCompile();
                                    scope.$apply();
                                }, 100);
                            }
                        });
                    }
                    else {
                        window.setTimeout(() => {
                            scope.operation.AfterCompile();
                        }, 100);
                    }

                    //scope.operation.Visible();
                }
                else
                {
                    
                    if (!scope.operation.withController)
                        if (scope.operation.withController)
                            scope.operation.SetController();

                    document.getElementById(scope.operation.id + 'Parent').style.display = "block";
                    
                    scope.operation.AfterShow();
                }
            }
 

        }

    }
}

appDirectivesModule.directive('labxandModal', ($compile, $rootScope) => { return new labxand.components.ModalDirective($compile, $rootScope); });
