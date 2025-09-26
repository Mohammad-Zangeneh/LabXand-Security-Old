module labxand.components.core {
    interface IModalSection extends ng.IScope {
        operation: ModalSectionOperation;
    }

    export class ModalSectionDirective implements ng.IDirective {
        template = `

<div class="nodakModal" id="{{operation.Id}}">
        <div class="nodakModal-content">
            <div class="nodakModal-header panel-heading  text-right">
               <span ng-click='operation.CloseModal()' class="nodakClose">&times;</span>
               <action-panel operation="operation.actionPanel"></action-panel>
            </div>
            <div  ng-transclude>
                <div class="nodakModal-body " id="{{operation.id}}">
                
                 </div>
            </div>
           
            <div class="nodakModal-footer text-left">
              
            </div>
        </div>
 
</div>


`;
        restrict = 'E';
        transclude = true;

        scope = {
            operation: '='
        };
        link = (scope: IModalSection, element: ng.IAugmentedJQuery) => {

        }

    }

    export class ModalSectionOperation {
        public Id: string;
        public Title: string;
        actionPanel: IActionPanel;
        confirmId: string;
        cancelId: string;

        constructor(title: string) {
            this.Id = Math.random().toString();
            this.Title = title;
            //this.CancelButtonTitle = "بستن";
            this.cancelId = this.Id + "Cancel";
            this.confirmId = this.Id + "Confirm";
           
            this.actionPanel = new labxand.components.core.ActionPanelBase();
            this.actionPanel.panelTitle = this.Title;
            this.actionPanel.AddButton(this.confirmId).HasCaption("تایید").HasTitle("تایید").SetOnClick(() => { this.ConfirmButtonClick() }).HasClass("btn  btn-success");
            this.actionPanel.AddButton(this.cancelId).HasCaption("خروج").HasTitle("بستن پنجره").SetOnClick(() => { this.CancelButtonClick() }).HasClass("btn  btn-danger");
        }

        ShowModal() {
            
            //console.log(this.Id);
            document.getElementById(this.Id).style.display = "block";
        }

        CloseModal() {
            document.getElementById(this.Id).style.display = "none";
        }

        ConfirmButtonClick() { }
        CancelButtonClick() {
            this.CloseModal();
        }

        //VisibleCancelButton() {
        //    this.actionPanel.VisibleButtonById(this.cancelId);
        //}

        //InVisibleCancelButton() {
        //    this.actionPanel.InVisibleButtonById(this.cancelId);
        //}

        //SetTitleAndCaptionCofirmButton(caption: string, title: string) {
        //    this.actionPanel.SetTitleAndCaptionButtonById(this.confirmId, caption, title);
        //}

    }
}
appDirectivesModule.directive('modalSection', () => { return new labxand.components.core.ModalSectionDirective(); });
