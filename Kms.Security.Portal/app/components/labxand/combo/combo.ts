interface JQuery {
    fastselect;
    select2;
}
interface JQueryStatic {
    Fastselect;
}
module labxand.components.core {
    export interface IComboBaseScope<T> extends ng.IScope {
        Base: IComboBase<T>;
        Id: string;
    }

    export class LabXandCombo<T> implements ng.IDirective {
        $inject = ['$compile'];


        constructor(public $compile: ng.ICompileService) {

        }

        template = `
   
                    <select id="{{Base.Id}}" tabindex="-1" class="{{Base.Id}}"  name="language" ng-model="Base.SelectedItem" ng-change="Base.changeSelection()">
                        <option   ng-repeat="x in Base.Data" value="{{x[Base.ItemSelectedId]}}" >{{x[Base.ItemSelectedText]}}</option>
                    </select>
`;
        restrict = 'E';
        scope: any = {
            Base: "=base",
            Id: "=id"
        };

        link = (scope: IComboBaseScope<T>, el, attrs) => {

            if (!scope.Base.IsStatic)
                scope.Base.Service.Get(nodak.enums.ServiceTypeEnum.Get).then((res) => {

                    scope.Base.Data = res;
                });

            if (scope.Base.IsMultiSelect) {
                el.children("select").attr("multiple", true);
            }

            //if (scope.Base.AllowAdd)
            //    el.children("select").attr("data-user-option-allowed", true);


            scope.$watch(() => { return scope.Base.SelectedItems }, (n, o) => {
                if (o != n) {

                    let temp = [];

                    for (var i = 0; i < scope.Base.SelectedItems.length; i++) {
                        if (scope.Base.SelectedItems[i] != null)
                            temp.push(scope.Base.SelectedItems[i][scope.Base.ItemSelectedId]);
                    }

                    $('#' + scope.Base.Id).val(temp); // Select the option with a value of 'US'
                    $('#' + scope.Base.Id).trigger('change'); // Notify any JS components that the value changed


                }
            });
            scope.$watch(() => { return scope.Base.Data }, (n, o) => {

                $("#" + scope.Base.Id).select2({
                    placeholder: scope.Base.PlaceHolder,
                    allowClear: scope.Base.AllowClear,
                    dir: scope.Base.RTL == true ? "rtl" : "ltr",
                    tags: true,
                    selectOnClose: false
                });
                $("#" + scope.Base.Id).val(null);
                $("#" + scope.Base.Id).val(null).trigger('change');

            });
        }
    }

}

angular.module('nodak.components').directive('labxandCombo', ($compile) => { return new labxand.components.core.LabXandCombo($compile); });
