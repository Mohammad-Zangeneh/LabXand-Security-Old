module nodak.components {

    export interface ICheckBoxScope extends ng.IScope {

        id: string;
        name: string;
        isChecked: boolean;
        caption: string;
        divstyle: string;
        captionstyle: string;
        checked: string;
        updateState: () => void;
    }

    export function CheckBox(): ng.IDirective {

        return {
            restrict: 'E',
            replace: false,
            scope: {
                checked: '=', divstyle: '=', caption: '='
            },
          
            template:
            `<div class="row"> <div style="{{captionstyle}}" class="col-lg-6 col-md-6 col-sm-6">  {{caption}} </div> <div class="col-lg-6 col-md-6 col-sm-6" > <div class="{{divstyle}}">
            <input type="checkbox"  value="{{checked}}" id="for{{id}}" name="{{name}}"  ng-model="checked"  />
            <label for="for{{id}}" style="" > </label>
            </div></div></div>`,
            link: function (scope: ICheckBoxScope, element, attrs) {
                scope.id = attrs.id;
                scope.name = attrs.name;
                scope.divstyle = attrs.divstyle;
                scope.caption = attrs.caption;
                scope.captionstyle = attrs.captionstyle;
            }
        }
    }
}

appDirectivesModule.directive('checkBox', () => { return nodak.components.CheckBox(); });



































