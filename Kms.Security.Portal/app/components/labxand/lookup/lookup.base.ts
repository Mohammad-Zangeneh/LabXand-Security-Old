module labxand.components.core {
    export interface ILookupBase<T extends nodak.models.ModelBase, SearchModel> extends ILookup<T, SearchModel> {
        LoadContent();
        AfterCompile();
        SetController();
        OnSelected();
        SetSelectedTextAndId();
        ControllerNotAssigned(): boolean;
        SetSelectedTextField<U>(Property: U);
        SetSelectedIdField<U>(Property: U);
        selectedText: string;
        selectedId: string;
        Model: T;
        isLoaded: boolean;
        contenUrl: string;
        id: string;
        eventName: string;
        eventFired: boolean;
        staticLoad: boolean ;
    }

    export class LookupBase<T extends nodak.models.ModelBase, SearchModel> implements ILookupBase<T, SearchModel> {
        searchController: nodak.core.ISearchControllerBase<T, SearchModel>;
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
        selectedId: string;
        Model: T;
        disableCaption: boolean;
        eventName: string;
        eventFired: boolean;
        //morsa for load look up after load page
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
            return this.searchController == null;
        }

        SetController() {
            let fullControllerName = this.controllerName + ' as ' + this.aliasName;
            let dom_element = document.getElementById(this.id).querySelector(`[ng-controller="` + fullControllerName + `"]`);
            let ng_element = angular.element(dom_element);
            let evalString = "ng_element.scope()." + this.aliasName;
            let scope = eval(evalString);
            this.searchController = <nodak.core.ISearchControllerBase<T, SearchModel>>scope;
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
            //negin:baraye inke betune entekhabi k kardaro pak kone commentesh kardam
            //if (this.searchController.gridEntity.SelectedRow == null) {
            //    alert('not selected');
            //    return;
            //}

            let captionField = "this.searchController.gridEntity.SelectedRow." + this.selectedTextField;
            let idField = "this.searchController.gridEntity.SelectedRow." + this.selectedIdField;

            this.selectedText = eval(captionField);
            this.selectedId = eval(idField);
            this.SetSelectedTextAndId();
            this.Close();
            this.AfterSelect(this.searchController.gridEntity.SelectedRow);
        }

        AfterSelect(selectedModel: T) { }
        AfterChange(changedModel: T) { }
        Disable() { }
        Enable() { }
        Clear() { }
        AfterClose() { }
        AfterLoaded() { }
        Close() { }
        Show() { }
        LoadContent() { }

        AfterShow() {
        }

    }
}
