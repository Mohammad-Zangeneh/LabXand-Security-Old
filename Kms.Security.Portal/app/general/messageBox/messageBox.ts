interface IMessageBox {
    OKCancleButton(Title: string, BodyText: string): ng.IPromise<string>;
    YesNoButton(Title: string, BodyText: string): ng.IPromise<string>;
    OkButton(Title: string, BodyText: string): ng.IPromise<boolean>;

}


module imas.modelBase {

    export class MessageBox implements IMessageBox {
        private deferred: ng.IDeferred<{}>;
        private positiveButton: string;
        private negativeButton: string;
        private bodyText: string;
        private titleText: string;
        private htmContent: string;
        private testDefered: ng.IDeferred<{}>;


        static $inject = ['$q'];
        constructor(private $q: ng.IQService) {
            this.positiveButton = "بلی";
            this.negativeButton = "خیر";
        }

        private ShowDialog() {
            //this.htmContent = ' <div id="messageBox" class="modal-dialog modal-sm">' +
            //    '<div class="modal-content">' +
            //    '<div class="modal-header">' +
            //    '<div id= "MessageBoxTitle" ><p>' + this.titleText + '</p></div></div>' +
            //    '<div class="modal-body" id="MessageBoxBody"><p>' + this.bodyText + '</p></div>' +
            //    '<div class="modal-footer" id="MessageBoxFooter"><button type="button" class="btn btn-primary btn-sm"  id="OKShowDialog">' + this.positiveButton + '</button>' +

            //    '<button type="button" class="btn btn-primary btn-sm" id="CancleShowDialog">' + this.negativeButton + ' </button></div ></div></div>'
           
            this.htmContent = `
<div id="messageBox" class="nodakModal ">

            <!-- Modal content -->
            <div class="nodakModal-content nodakModal-sm">
                <!--Header-->
                <div class="nodakModal-header panel-heading  text-right ">
                  <!--  <span ng-click='' class="nodakClose">&times;</span>-->
                    <h4 class="nodakModal-title" id= "MessageBoxTitle">${this.titleText} </h4>
                </div>
                <!--Body-->
                <div class="nodakModal-body text-right" id="MessageBoxBody">
                   ${this.bodyText}
                </div>
                <!--Footer-->
                <div class="nodakModal-footer text-left">
                    <button  id="OKShowDialog" class="kmsUpdateBtn" >${this.positiveButton}</button>
                    <button class="kmsDeleteBtn" id="CancleShowDialog">${this.negativeButton }</md-button>
                </div>
            </div>

        </div>
`;
            angular.element(document.getElementById('DialogResult')).append(
                this.htmContent);
            angular.element(document.getElementById('DialogResult')).css('display', 'block');
            angular.element(document.getElementById('messageBox')).css('display', 'block');
            angular.element(document.getElementById('OKShowDialog')).on('click', () => { this.CloseDialog(); this.deferred.resolve(this.deferred = undefined); })
            angular.element(document.getElementById('CancleShowDialog')).on('click', () => { this.CloseDialog(); this.deferred.reject(this.deferred = undefined); })
        }

        private CloseDialog() {
            document.getElementById('messageBox').remove();
            angular.element(document.getElementById('DialogResult')).css('display', 'none');
        }
        SetTitleOfButton(positive: string, negative: string) {
            this.positiveButton = positive;
            this.negativeButton = negative;
        }
        OKCancleButton(Title: string, BodyText: string): ng.IPromise<string> {
            if (this.deferred == undefined) {
                this.deferred = this.$q.defer();
                this.titleText = Title;
                this.bodyText = BodyText;
                this.positiveButton = "تایید";
                this.negativeButton = "انصراف";
                this.ShowDialog();

                return this.deferred.promise;


            }
            else {
                this.deferred = this.$q.defer();
                return this.deferred.promise;
            }


        }

        YesNoButton(Title: string, BodyText: string): ng.IPromise<string> {
            if (this.deferred == undefined) {
                this.deferred = this.$q.defer();
                //console.log(this.deferred);
                this.titleText = Title;
                this.bodyText = BodyText;

                this.ShowDialog();
                return this.deferred.promise;

            }
            else {
                this.deferred = this.$q.defer();
                return this.deferred.promise;
            }
        }
        OkButton(Title: string, BodyText: string): ng.IPromise<boolean> {
            if (this.deferred == undefined) {
                this.deferred = this.$q.defer();
                console.log(this.deferred);
                this.titleText = Title;
                this.bodyText = BodyText;
                this.positiveButton = "تایید";
               
                this.ShowDialog();
                angular.element(document.getElementById("CancleShowDialog")).css('display', 'none');
                return this.deferred.promise;

            }
            else {
                this.deferred = this.$q.defer();
                return this.deferred.promise;
            }
        }


    }
    appServicesModule.factory('messageBox', ($q: ng.IQService) => { return new MessageBox($q) })
}





