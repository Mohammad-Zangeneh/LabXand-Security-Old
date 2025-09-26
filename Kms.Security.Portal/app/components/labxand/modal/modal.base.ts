module labxand.components.core {
    export interface IModalBase extends IModal {
        isLoaded: boolean;
        contentUrl: string;
        id: string;
        idModalButton: string;
        eventName?: string;
        eventFired?: boolean;
        withController?: boolean;
        actionPanel: IActionPanel;
        AfterCompile();
        LoadContent();
        ControllerNotAssigned?: Function;
        SetController?: Function;
    }

    export interface IModalWithControllerBase<TController> extends IModalWithController<TController>, IModalBase {
        
        withController: boolean;
        aliasName: string;
        controllerName: string;
        eventName: string;
        eventFired: boolean;
        ControllerNotAssigned();
        SetController();
    }

    export class ModalBase implements IModalBase {
        
        isLoaded: boolean;
        contentUrl: string;
        id: string;
        idModalButton: string;
        actionPanel: IActionPanel;

        constructor(
            contentUrl: string,
            id: string
        ) {
           
            this.isLoaded = false;
            this.id = id + new Date().getTime() + "Modal";
            this.contentUrl = contentUrl;
            this.actionPanel = new labxand.components.core.ActionPanelBase();
            
            this.actionPanel.AddButton(id + "CloseModal").HasCaption("خروج").HasTitle("بستن پنجره").HasClass("btn btn-sm btn-danger").SetOnClick(() => { this.Close(); });
       
        }

        AfterCompile() {
            
            this.AfterLoaded();
            this.AfterShow();
            document.getElementById(this.id + 'Parent').style.display = "block";
        }

        LoadContent() { }
        Disable() { }
        Enable() { }
        AfterShow() { }
        Show() {  }
        Close() { }
        AfterClose() { }
        AfterLoaded() { }
    }

    export class ModalWithControllerBase<TController> extends ModalBase implements IModalWithControllerBase<TController> {
    
        withController: boolean;
        aliasName: string;
        controllerName: string;
        eventName: string;
        eventFired: boolean;
        controller: TController;

        constructor(aliasName: string,
           
            controllerName: string,
            contentUrl: string,
            id: string,
            entityName: string,
            subSystem: nodak.enums.SubSystems,
            typeOfController: nodak.enums.TypeOfController) {
            
            super(contentUrl, id);
            this.withController = true;
            this.controllerName = controllerName;
            this.aliasName = aliasName;
            if (typeOfController == nodak.enums.TypeOfController.CurdController)
                this.eventName = subSystem + entityName + "ControllerEvent";
            else
                this.eventName = subSystem + entityName + "SControllerEvent";

            //alert(this.eventName + "'");
        }

        Clear() { }
        AfterShow() {  }
        AfterClose() { }
        AfterLoaded() {  }
        AfterCompile()
        {
            this.SetController();
            this.AfterLoaded();
            this.AfterShow();
            document.getElementById(this.id + 'Parent').style.display = "block";
        }
        ControllerNotAssigned() { return this.controller == null; }
        SetController()
        {
            let fullControllerName = this.controllerName + ' as ' + this.aliasName;
            let dom_element = document.getElementById(this.id).querySelector(`[ng-controller="` + fullControllerName + `"]`);
            let ng_element = angular.element(dom_element);
            let evalString = "ng_element.scope()." + this.aliasName;
            let scope = eval(evalString);
            this.controller = <TController>scope;
        }
    }

}