
module nodak.components {
    interface IDatePicker extends ng.IScope {
        id: string;
        date: Date;
        class: string;
        $apply(): any;
        $apply(exp: string): any;
        $apply(exp: (scope: ng.IScope) => any): any;
        $applyAsync(): any;
        $applyAsync(exp: string): any;
        $applyAsync(exp: (scope: ng.IScope) => any): any;
    }
    
    export class DatePickerDirective implements ng.IDirective
    {

        template =
        `<div ><input id="dp1" placeholder="تاریخ" class="scope.class" style="float:right;" readonly ng-model="date"  ng-date-picker="load()" ng-date-picker-today="'1396/04/28'"  /></div>`;
        restrict = 'E';
        scope =
        {
            id: '=',
            date: '=',
            class:'='
            
        };
        
        link = (scope: IDatePicker, element: ng.IAugmentedJQuery, ngModel) => {

            //console.log(scope.class);
            scope.$watch("date", function () {
                
                //console.log(scope.date);
            })
            
        }

    }
}

appDirectivesModule.directive('labxand', () => { return new  nodak.components.DatePickerDirective(); });