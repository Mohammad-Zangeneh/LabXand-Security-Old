module nodak.directives {
    export interface IMultiSelectCombo extends ng.IScope {
        selectList: nodak.models.IBaseMultiSelectCombo<any>;
        model: any;
        withoutHeader: boolean;
        isDisabled: boolean;
        defaultId: string;
        id: string;
        selectedIds: Array<string>;
        selectedModels: Array<Object>;
        handler: nodak.components.DropdownMultiSelectFunctions<any>;
    }

    export class MultiSelectCombo implements ng.IDirective {
        template = `<div oo id='{{handler.textboxId}}' class='customSelectBox input-group' style='width:100%'>
                        <input ng-focus='handler.ChangeVisibility()' type='text' value='{{handler.textSelected}}' title='{{handler.textSelected}}' id={{handler.textId}} readonly style='color:black'/>
                        <span ng-click='handler.ChangeVisibility()' class='input-group-addon'><span class='caret'></span></span>
                    </div>
                    <div class='contentdropdown' id='{{handler.contentId}}' style='position:absolute;visibility:hidden;margin-top:-26px;width:96%'>
                        <div class='customSelectBox input-group'>
                            <input ng-model='filterStatement' type='text' ng-keydown='handler.Navigate($event)' ng-change='handler.Reset()' style='color:black'/>
                            <span ng-click='handler.ChangeVisibility()' class='input-group-addon'><span class='caret'></span></span>
                        </div>
                        <div style='margin-right:0!important;max-height:215px;overflow-y:scroll;overflow-x:hidden'>
                            <table class='contentDropdownContainter'>
                                <thead ng-hide='handler.withoutHeader'>
                                    <tr class='comboTitle'>
                                        <th style='color:black;height:30px;font-size:11px'>
                                            <span ng-mousedown='handler.SetCheckAll($event)' style='color:black' ng-class="handler.SelectedAll()?'fa fa-check-circle-o':'fa fa-circle-thin'"></span>
                                        </th> 
                                        <th ng-repeat='h in handler.headers' style='color:black;height:30px;font-size:11px' class='{{h.className}}'>{{h.displayName}}</th>
                                    </tr>
                                </thead>
                                <tbody class='comboRow'>
                                    <tr ng-repeat='col in handler.model | filter:filterStatement' ng-mousedown='handler.SetCheck($event,col[handler.valueMember])'>
                                        <td style='color:black;height:30px;font-size:11px'><span ng-class="{true:'fa fa-check-circle-o',false:'fa fa-circle-thin'}[handler.IsSelected(col[handler.valueMember])]"></span></td>
                                        <td style='color:black;height:30px;font-size:11px' ng-repeat='h in handler.headers' class='{{h.className}}' ng-bind='col[h.field]'></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>`;
        restrict = 'E';
        scope = {
            id: "@id",
            selectList: "=",
            selectedIds: "=selectedId",
            selectedModels: "=selectedModels"
        };

        link = (scope: IMultiSelectCombo, element: ng.IAugmentedJQuery) => {
            if (!scope.selectedIds || scope.selectedIds == null)
                scope.selectedIds = [];

            scope.selectList.service.Get(nodak.enums.ServiceTypeEnum.Combo).then((response) => { scope.model = response; })
            scope.$watch(() => { return scope.model }, (newValue, oldValue) => {
                scope.handler = new nodak.components.DropdownMultiSelectFunctions(element, scope);
            });
        }
    }
}

comboDirectivesModule.directive('multiSelectCombo', () => { return new nodak.directives.MultiSelectCombo(); });
