module nodak.components {
    export interface IMemberAutoComplete extends ng.IScope {

        //todo
        //my member is array of member
        MyMember ;
        selectedPerson;
    }

    export class MemberAutoCompeleteDirective implements ng.IDirective {
        static $inject = ['$http'];
        constructor(public http: ng.IHttpService) {
        

        }

        template = `
     
        <angucomplete id="ex2" placeholder="جستجو کاربر" pause="300" selectedobject="selectedPerson" localdata="mine" searchfields="FirstName,LastName" titlefield="FirstName,LastName" descriptionfield="Organization" insidedes="Name"  minlength="1" inputclass="form-control form-control-small" matchclass="highlight" />
     
         `

        restrict = 'E';

        scope = {
            selectedPerson: '='
        };

        link = (scope: IMemberAutoComplete) => {

            

            this.http.get(Base.Config.ServiceRoot + '/api/Member/Get').then(
                function (response) {
                    scope.MyMember = response.data;
                })

            scope.$watch('selectedPerson', function () {
                
                //console.log("slct", scope.selectedPerson);
            });


            scope.$watch('mine', function () {
                
                //console.log("mine", scope.MyMember);
            }, true);
        };

    }
}
appComponents.directive('MemberAutoCompelete', ($http) => { return new nodak.components.MemberAutoCompeleteDirective($http); });
