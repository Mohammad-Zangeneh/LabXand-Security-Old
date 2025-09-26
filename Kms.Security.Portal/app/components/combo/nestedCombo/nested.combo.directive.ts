module nodak.directives {
    export interface INestedCombo extends ng.IScope {
        selectList: nodak.models.IBaseNestedCombo<any>;
        model: any;
        withoutHeader: boolean;
        withoutFilter: boolean;
        isDisabled: boolean;
        defaultId: string;
        id: string;
        selectedId: string;
        handler: nodak.components.NestedDropdownFunctions<any>;
        headers: any;
        selectedText: string;
    }

    export class NestedCombo implements ng.IDirective {

        template =
        `<div class="row" style="margin:0;padding:0">
                    <div id={{id}}Combo1 style="margin:0;padding:0;float:right;width:100%">
                        <div id="{{handler.id1}}" class="customSelectBox input-group" style="width:100%">
                            <input ng-focus="handler.ChangeVisibility(handler.contentId1)" type="text" id={{handler.textId1}} readonly style="color:black"/>
                            <span ng-click="handler.ChangeVisibility(handler.contentId1)" class="input-group-addon"><span class="caret"></span></span>
                        </div>
                        <div class="contentdropdown" id="{{handler.contentId1}}" style="position:absolute;visibility:hidden;width:97%;margin-top:-26px">
                            <section style="padding-top:0">
                                <input ng-model="filterStatement" type="text" style="color:black" ng-keydown="handler.Navigate($event, handler.contentId1)" ng-change="handler.Reset(handler.contentId1)" ng-blur="handler.SetBlur(handler.contentId1)"/>
                                <div class="container" style="width:100%!important;margin-right:0!important;height:215px">
                                    <table class="contentDropdownContainter">
                                        <thead>
                                            <tr>
                                                <th colspan="{{handler.headers.length+1}}"></th>
                                            </tr>
                                            <tr class="comboTitle">
                                                <th ng-repeat="h in handler.headers" class="{{h.className}}">{{h.displayName}}</th>
                                            </tr>
                                        </thead>
                                        <tbody class="comboRow">
                                            <tr ng-repeat="col in handler.model1 | filter:filterStatement" ng-mousedown="handler.SetSelected(col[handler.valueMember])" style="height:30px">
                                                <td ng-repeat="h in handler.headers" class="{{h.className}}" ng-bind="col[h.field]" style="font-size:11px;cursor:pointer"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </div>
                    </div>
                    <div id={{id}}Combo2 style="margin:0;padding:0;float:right;width:0">
                        <div id="{{handler.id2}}" class="customSelectBox input-group" style="width:100%;visibility:hidden">
                            <input ng-focus="handler.ChangeVisibility(handler.contentId2)" type="text" id="{{handler.textId2}}" readonly style="color:black"/>
                            <span ng-click="handler.ChangeVisibility(handler.contentId2)" class="input-group-addon"><span class="caret"></span></span>
                        </div>
                        <div class="contentdropdown" id="{{handler.contentId2}}" style="position:absolute;visibility:hidden;width:97%;margin-top:-26px">
                            <section>
                                <input ng-model="filterStatement2" type="text" style='color:black' ng-keydown="handler.Navigate($event, handler.contentId2)" ng-change="handler.Reset(handler.contentId2)" ng-blur="handler.SetBlur(handler.contentId2)"/>
                                <div class="container" style="width:100%!important;margin-right:0!important;height:215px">
                                    <table class="contentDropdownContainter">
                                        <thead>
                                            <tr>
                                                <th colspan="{{handler.headers.length+1}}"></th>
                                            </tr>
                                            <tr class="comboTitle">
                                                <th ng-repeat="h in handler.headers" class="{{h.className}}">{{h.displayName}}</th>
                                            </tr>
                                        </thead>
                                        <tbody class="comboRow">
                                            <tr ng-repeat="col in handler.model2  | filter:filterStatement2" ng-mousedown="handler.SetSelected(col[handler.valueMember])" style="height:30px">
                                                <td ng-repeat="h in handler.headers" class="{{h.className}}" ng-bind="col[h.field]" style="font-size:11px;cursor:pointer"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </div>
                    </div>
                    <div id={{id}}Combo3 style="margin:0;padding:0;float:right;width:0">
                        <div id="{{handler.id3}}" class="customSelectBox input-group" style="width:100%;visibility:hidden">
                            <input ng-focus="handler.ChangeVisibility(handler.contentId3)" type="text" id="{{handler.textId3}}" readonly style="color:black"/>
                            <span ng-click="handler.ChangeVisibility(handler.contentId3)" class="input-group-addon"><span class="caret"></span></span>
                        </div>
                        <div class="contentdropdown" id="{{handler.contentId3}}" style="position:absolute;visibility:hidden;width:97%;margin-top:-26px">
                            <section>
                                <input ng-model="filterStatement3" type="text" style='color:black' ng-keydown="handler.Navigate($event, handler.contentId3)" ng-change="handler.Reset(handler.contentId3)" ng-blur="handler.SetBlur(handler.contentId3)"/>
                                <div class="container" style="width:100%!important;margin-right:0!important;height:215px">
                                    <table class="contentDropdownContainter">
                                        <thead>
                                            <tr>
                                                <th colspan="{{handler.headers.length+1}}"></th>
                                            </tr>
                                            <tr class="comboTitle">
                                                <th ng-repeat="h in handler.headers" class="{{h.className}}">{{h.displayName}}</th>
                                            </tr>
                                        </thead>
                                        <tbody class="comboRow">
                                            <tr ng-repeat="col in handler.model3  | filter:filterStatement3" ng-mousedown="handler.SetSelected(col[handler.valueMember])" style="height:30px">
                                                <td ng-repeat="h in handler.headers" class="{{h.className}}" ng-bind="col[h.field]" style="font-size:11px;cursor:pointer"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </div>
                    </div>
                    <div id={{id}}Combo4 style="margin:0;padding:0;float:right;width:0">
                        <div id="{{handler.id4}}" class="customSelectBox input-group" style="width:100%;visibility:hidden">
                            <input ng-focus="handler.ChangeVisibility(handler.contentId4)" type="text" id="{{handler.textId4}}" readonly style="color:black"/>
                            <span ng-click="handler.ChangeVisibility(handler.contentId4)" class="input-group-addon"><span class="caret"></span></span>
                        </div>
                        <div class="contentdropdown" id="{{handler.contentId4}}" style="position:absolute;visibility:hidden;width:97%;margin-top:-26px">
                            <section>
                                <input ng-model="filterStatement4" type="text" style='color:black' ng-keydown="handler.Navigate($event, handler.contentId4)" ng-change="handler.Reset(handler.contentId4)" ng-blur="handler.SetBlur(handler.contentId4)"/>
                                <div class="container" style="width:100%!important;margin-right:0!important;height:215px">
                                    <table class="contentDropdownContainter">
                                        <thead>
                                            <tr>
                                                <th colspan="{{handler.headers.length+1}}"></th>
                                            </tr>
                                            <tr class="comboTitle">
                                                <th ng-repeat="h in handler.headers" class="{{h.className}}">{{h.displayName}}</th>
                                            </tr>
                                        </thead>
                                        <tbody class="comboRow">
                                            <tr ng-repeat="col in handler.model4  | filter:filterStatement4" ng-mousedown="handler.SetSelected(col[handler.valueMember])" style="height:30px">
                                                <td ng-repeat="h in handler.headers" class="{{h.className}}" ng-bind="col[h.field]" style="font-size:11px;cursor:pointer"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>`;
        restrict = 'E';
        scope = {
            id: "@id",
            selectList: "=",
            selectedId: "=selectedId"
        };
        link = (scope: INestedCombo, element: ng.IAugmentedJQuery) => {
            scope.selectedId = null;

            scope.selectList.service.Get(nodak.enums.ServiceTypeEnum.Combo).then((response) => { scope.model = response; });

            scope.$watch(() => { return scope.model }, (newValue, oldValue) => {
                scope.handler = new nodak.components.NestedDropdownFunctions(element, scope);
                if (oldValue == undefined && newValue != undefined) {
                    scope.handler.model1 = scope.handler.model.filter(function (model) { return model[scope.handler.parentName] == null });
                    scope.handler.getStructureTree();
                }
            });

            //scope.$watch(() => { return scope.handler.selectedText; }, (newValue, oldValue) => {
            //    scope.selectList.selectedText = scope.handler.selectedText;
            //});

            scope.$watch(() => { return scope.selectedId }, (newValue, oldValue) => {

                scope.handler.selectedId = scope.selectedId;
                scope.handler.getStructureTree();
                //scope.selectedText = scope.model.filter(function (m) { return m[scope.handler.valueMember] == scope.selectedId; })[0][scope.handler.displayMember];

            });

            scope.$watch(() => { return scope.handler.selectedId }, (newValue, oldValue) => {

                if (newValue != undefined) {
                    scope.selectedId = scope.handler.selectedId;
                    scope.selectList.selectedId = scope.handler.selectedId;
                }

            });
        }

    }
}

comboDirectivesModule.directive('nestedCombo', () => { return new nodak.directives.NestedCombo(); });
