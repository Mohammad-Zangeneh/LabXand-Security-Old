module nodak.components {
    interface INotification extends ng.IScope {
        id: string;
        counter: number;
        listOfNotification: Array<any>;
    }
    export class NotificationDirective implements ng.IDirective {

        template =
        ` 
<div id='{{id}}'>
                 <div id="noti_Container" style=";bottom: 5px;left: 8px;width: 30px;height: 30px;">
                 
                    <span class="badge" id="noti_Counter" ></span>
                    
                    <div id="noti_Button" >
                        <i class="material-icons">notifications</i>
                    </div> 
                    
                </div>
 </div>`;


        restrict: 'E';

        scope = {

            id: '=',
            counter: '=',
            listOfNotification: '='

        };


        link = (scope: INotification, element: ng.IAugmentedJQuery) => {

        


                $('#noti_Button').click(function () {

                    window.location.href = "/angular/index.html#/Notification/NotificationManagement";
                    $('#noti_Counter').fadeOut('slow');      
                    return false;
                });


            //scope.$watch('counter', function () {
            //    
            //    if (scope.counter != 0) {
            //        $('#noti_Counter').text(scope.counter);
            //        $('#noti_Counter')
            //            .css({ opacity: 0 })
            //            .css({ top: '-10px' })
            //            .animate({ top: '-5px', opacity: 1 }, 500);
            //    }
            //    else {
            //        $('#noti_Counter').text("0");
            //    }

            //});

             $('#noti_Counter').text("0");


        }

    }


}
appDirectivesModule.directive('notification', () => { return new nodak.components.NotificationDirective(); });
