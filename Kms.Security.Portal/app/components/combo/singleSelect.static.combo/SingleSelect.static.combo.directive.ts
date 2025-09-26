module nodak.directives {
    export interface IStaticSingleSelctCombo extends ng.IScope {
        selectList: nodak.models.BaseStaticCombo<any>;
        model: any;
        withoutHeader: boolean;
        withoutFilter: boolean;
        isDisabled: boolean;
        defaultId: string;
        id: string;
        selectedId: string;
        selectedText: string;
        handler: nodak.components.StaticDropdownMultiColumnsFunctions<any>;
    }

    export class StaticSingleSelectCombo implements ng.IDirective {
        template = `<div id='{{handler.textboxId}}' class='customSelectBox input-group' style='width:100%' title='{{selectedText}}'>
                        <input ng-focus='handler.ChangeVisibility()' type='text' value='{{selectedText}}' id='{{handler.textId}}' readonly title='{{handler.selectedText}}' />
                        <span ng-click='handler.ChangeVisibility()' class='input-group-addon'><span class='caret'></span></span>
                    </div>
                    <div class='contentdropdown' id='{{handler.contentId}}' style='position:absolute;visibility:hidden;margin-top:-26px;width:96%'>
                        <section class=''>
                            <input ng-model='filterStatement' type='text' ng-keydown='handler.Navigate($event)' ng-change='handler.Reset()' />
                            <div class='container' style='width:100%!important;margin-right:0!important;height:215px'>
                                <table class='contentDropdownContainter'>
                                    <thead>
                                        <tr class='comboTitle'>
                                            <th ng-repeat='h in handler.headers' class='{{h.className}}'>
                                                <div>{{ h.displayName }}</div>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody class='comboRow'>
                                        <tr ng-repeat='col in handler.model | filter:filterStatement' ng-mousedown='handler.SetCheck(col[handler.valueMember]);' style='height:30px'>
                                            <td ng-repeat='h in handler.headers' class='{{h.className}}' ng-bind='col[h.field]' style='font-size:11px;cursor:pointer'></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                    </section>
                </div>`;
        restrict = 'E';
        scope = {
            id: "@id",
            selectList: "=",
            selectedId: "=selectedId"
        };
        link = (scope: IStaticSingleSelctCombo, element: ng.IAugmentedJQuery) => {
            if (!scope.selectedId) {
                scope.selectedId = null;
            }

            scope.$watch(() => { return scope.selectList.model }, (newValue, oldValue) => {
                if (newValue) {
                    scope.model = newValue;
                    scope.handler = new nodak.components.StaticDropdownMultiColumnsFunctions(element, scope);
                }
            })

        }
    }
}
angular.module('nodak.components').directive('staticSingleSelectCombo', () => { return new nodak.directives.StaticSingleSelectCombo()});