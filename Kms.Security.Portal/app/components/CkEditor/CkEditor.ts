
module nodak.components {
    interface ICkeditor extends ng.IScope {
        id: string;
        ckeditorvalue: string;
        set: boolean;
        $apply(): any;
        $apply(exp: string): any;
        $apply(exp: (scope: ng.IScope) => any): any;

        $applyAsync(): any;
        $applyAsync(exp: string): any;
        $applyAsync(exp: (scope: ng.IScope) => any): any;
    }
    export class ckeditorDirective implements ng.IDirective {
       

        restrict = 'E';

        scope = {
            id: '=',
            ckeditorvalue: '=',
            set: '='
        };



        link = (scope: ICkeditor, element: ng.IAugmentedJQuery, ngModel) => {

            var ck;
            var count = 0;
           

           ck = CKEDITOR.replace(element[0]);

           
            ck.on('change', function () {
                

                scope.ckeditorvalue = CKEDITOR.instances[element[0].id].getData();
                if (!scope.$$phase) {
                    scope.$applyAsync();
                }

                //if (scope.set == true) {
                //    scope.set = false;
                //    }


            });


            scope.$watch('set', function ()
            {
                
                if (scope.set == true) {
                    CKEDITOR.instances[element[0].id].setData(scope.ckeditorvalue);
                    scope.set = false;
                }


            });




        }

    }

    export class CkeditorOperation {

        public Id: string;

        constructor(id: string) {
            this.Id = id;

        }

        //GetText() {
        //    
        //    return CKEDITOR.instances[this.Id].getData();
        //}
        // SetText(data: string) {
        //     CKEDITOR.instances[this.Id].setData(data);
        // }



    }
}

appDirectivesModule.directive('ckeditor', () => { return new nodak.components.ckeditorDirective(); });