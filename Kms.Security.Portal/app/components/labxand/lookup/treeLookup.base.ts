module labxand.components.core {
    export class TreeLookupBase<T extends nodak.models.ModelBase> implements ILookupBase<T, any>, ITreeLookup<T> {
        searchController: nodak.core.ISearchControllerBase<T, any>;
        treeController: nodak.core.ITreeSearchControllerBase<T>;
        aliasName: string;
        controllerName: string;
        contenUrl: string;
        isLoaded: boolean;
        id: string;
        idInput: string;
        idModalButton: string;
        selectedTextField: string;
        selectedIdField: string;
        selectedText: string;
        selectedId: any;
        Model: T;
        disableCaption: boolean;
        eventName: string;
        eventFired: boolean;
        staticLoad: boolean = false;
        constructor(aliasName: string,
            controllerName: string,
            contenUrl: string,
            id: string,
            entityName: string,
            subSystem: nodak.enums.SubSystems
           ) {
            this.aliasName = aliasName;
            this.controllerName = controllerName;
            this.contenUrl = contenUrl;
            this.disableCaption = true;
            this.id = 'Lookup' + new Date().getTime() + id;
            this.idInput = this.id + "LUInput";
            this.idModalButton = this.id + "LUButton";
            this.eventName = subSystem + entityName + "SControllerEvent";
            //this.SetController();
            
           // this.load();

        }

      

        UpdateTextAndId() {

            if (this.treeController.treeEntity.SelectedRow == undefined || this.treeController.treeEntity.SelectedRow == null) {
                // alert('not selected');
                this.selectedId = null;
                this.selectedText = "";
                this.Close();
                this.SetSelectedTextAndId();
                return;
            }
            if (this.treeController.treeEntity.MultiSelect == true) {
                this.selectedText = "";
                this.selectedId = [];
                for (let i = 0; i < this.treeController.treeEntity.SelectedRows.length; i++) {
                    this.selectedId.push(this.treeController.treeEntity.SelectedRows[i][this.selectedIdField]);
                    if (i < 3)
                        this.selectedText += ", " + this.treeController.treeEntity.SelectedRows[i][this.selectedTextField];
                }
                if (this.selectedText.length > 0)
                    this.selectedText = this.selectedText.slice(1);

                if (this.treeController.treeEntity.SelectedRows.length > 3)
                    this.selectedText += ", ..."
            }
            else {
                this.selectedId = this.treeController.treeEntity.SelectedRow[this.selectedIdField];
                this.selectedText = this.treeController.treeEntity.SelectedRow[this.selectedTextField];
            }
            this.SetSelectedTextAndId();
        }
        SetSelectedTextField<T>(Property: T) {
            let propName = nodak.PropertyManipulating.GetNameModelGrid(Property);
            this.selectedTextField = propName;
        }
        SetSelectedIdField<U>(Property: U) {
            let propName = nodak.PropertyManipulating.GetNameModelGrid(Property);
            this.selectedIdField = propName;
        }

        ControllerNotAssigned(): boolean {
            return this.treeController == null;
        }

        SetController() {
            let fullControllerName = this.controllerName + ' as ' + this.aliasName;
            let dom_element = document.getElementById(this.id).querySelector(`[ng-controller="` + fullControllerName + `"]`);
            let ng_element = angular.element(dom_element);
            let evalString = "ng_element.scope()." + this.aliasName;
            let scope = eval(evalString);
            this.treeController = <nodak.core.ITreeSearchControllerBase<T>>scope;
        }

        SetSelectedTextAndId() { };

        AfterCompile() {
            this.SetController();
            this.AfterLoaded();
            this.AfterShow();
            document.getElementById(this.id + 'Parent').style.display = "block";
        };

        SetDisableCaption() {
            this.disableCaption = true;
            // document.getElementById(this.idInput).setAttribute("disabled", "disabled");
        }

        SetEnableCaption() {
            this.disableCaption = false;
            // document.getElementById(this.idInput).removeAttribute("disabled");
        }

        OnSelected() {
            if (this.ControllerNotAssigned())
                this.SetController();

            this.UpdateTextAndId();
            this.Close();
            if (this.treeController.treeEntity.MultiSelect)
                this.AfterSelect(this.treeController.treeEntity.SelectedRows)
            else
                this.AfterSelect(this.treeController.treeEntity.SelectedRow);
        }

        AfterSelect(selectedModel: any) { }
        AfterChange(changedModel: T) { }
        Disable() { }
        Enable() { }
        Clear() {
            this.selectedText = "";
            this.selectedId = null;
            if (this.treeController.treeEntity != null) {
                this.treeController.treeEntity.SelectedRow = null;
                this.treeController.treeEntity.SelectedRows = [];
                

            }
            this.SetSelectedTextAndId();
        }
        AfterClose() { }
        AfterLoaded() { }
        Close() { }
        Show() { }
        LoadContent() { }

        AfterShow() {
        }

    }
}
