module app.directives {
    export interface IModalForm extends ng.IScope {
        thisId: string;
        placeholder: string;
        btnCaption: string;
        contentUrl: string;
        maxWidth: string;
        display: boolean;
        //vesselData: any;
        modalData: any;
        showModal(): void;
        closeModal(): void;
        OKBTN(data): void;
        LoadContent(): void;
        http: ng.IHttpService;
        inputData: any;
        isLoaded: boolean;
        disable: string;
        justOne: boolean,
        justPicture: boolean
    }

    export function ModalForm($http, $compile): ng.IDirective {
        return {
                    template: ` <div id='{{handler.textboxId}}' class='' style='width:100%;'>

                         <div  type='text' class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" layout="row" flex >
                   
          <div class="col-sm-12 col-lg-6 md-block" flex-gt-sm="">
                        <label>{{placeholder}}</label>
                        <input type="text" name="{{handler.textId}}" ng-model="inputValue" id={{handler.textId}} readOnly="{{handler.readOnly}}">
                       <button class="btn btn-primary" ng-click='showModal()' data-toggle="dropdown" aria-label=""></button>
              
                    </div>
                         </div>
                      </div>     




           <!-- The Modal -->
            <div id="{{thisId}}" class="nodakModal col-lg-12">

                <!-- Modal content -->
                <div class="nodakModal-content">
                    <div class="modal-header panel-heading   ">
                        <span ng-click='closeModal()' class="close">&times;</span>
                        <h4 class="modal-title">{{title}} </h4>
                    </div>
                    <div class="nodakModal-body" id='{{thisId}}Content'>
                
                    </div>  
                </div>

            </div>`
           ,

            restrict: "E",
            scope: {
                thisId: '@thisId',
                placeholder: '@placeholder',
                btnCaption: '@btnCaption',
                contentUrl: '@',
                modalData: "=",
                maxWidth: "@maxWidth",
                inputValue: "=",
                readOnly: '@',
                inputData: '=',
                disable: "@",
                justOne: "=",
                justPicture:"="
            },
            link: function (scope: IModalForm, ele, attrs) {
                if (scope.justPicture == undefined)
                    scope.justPicture = false;
                if (scope.justOne == undefined)
                    scope.justOne = false;
                //console.log(scope.justPicture);
                scope.contentUrl = Base.Config.AppRoot + scope.contentUrl;
                scope.$watch('readOnly', function () {
                    if (JSON.parse(attrs.readOnly)) {
                        angular.element(document.getElementsByClassName(scope.thisId)).attr("readonly", "readonly");
                    }
                });
                scope.$watch('inputData', function () {
                    if (scope.inputData != undefined && !scope.isLoaded)
                        scope.LoadContent();
                });

                if (scope.maxWidth)
                    angular.element(document.getElementsByClassName('fade')).css('max-width', scope.maxWidth);

                scope.showModal = function () {
                    if (scope.disable != "true") {
                        if (!scope.isLoaded)

                            scope.LoadContent();

                        if (!JSON.parse(attrs.readOnly)) {

                            angular.element(document.getElementById(scope.thisId)).css('display', 'block');
                        }
                    }
                }

                scope.closeModal = function () {
                    angular.element(document.getElementById(scope.thisId)).css('display', 'none');
                }
                scope.OKBTN = function (data) {
                    //console.log(data);
                    scope.modalData = angular.copy(data);
                    scope.closeModal();
                }

                scope.LoadContent = function () {
                    if (scope.disable != "true") {
                        angular.element(document.getElementById(scope.thisId + 'Content')).append($compile(
                            '<div partial-view="' + scope.contentUrl + '" ></div> ')(scope));
                        scope.isLoaded = true;
                    }
                }
            }

        }
    }

}
appDirectivesModule.directive('modal', ($http, $compile) => { return app.directives.ModalForm($http, $compile) });