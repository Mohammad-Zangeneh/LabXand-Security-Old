module labxand.components.core {
    export interface IPartialViewBase extends IPartialView {
        AfterCompile: Function;
        contenUrl: string;
        id: string;
        eventName: string;
        eventFired: boolean;
    }

    export interface IPartialViewWithControllerBase<TController> extends IPartialViewWithController<TController>, IPartialViewBase {
        aliasName: string;
        controllerName: string;
    }

    export class PartialViewBase implements IPartialViewBase {
        isLoaded: boolean;
        contenUrl: string;
        id: string;
        loadAutomatically: boolean;
        eventName: string;
        eventFired: boolean;

        constructor(
            contentUrl: string,
            id: string
        ) {
            this.loadAutomatically = true;
            this.isLoaded = false;
            this.id = id + new Date().getTime() + "partialView";
            this.contenUrl = contentUrl;
        }

        AfterCompile() { this.AfterLoaded(); }

        AfterLoaded() { }

        LoadContent() { }

    }

    export class PartialViewWithControllerBase<TController> extends PartialViewBase implements IPartialViewWithControllerBase<TController>{
        aliasName: string;
        controllerName: string;
        controller: TController;

        constructor(contentUrl: string, id: string, controllerName: string, aliasName: string, entityName: string, subSystem: nodak.enums.SubSystems) {
            super(contentUrl, id);
            this.eventName = subSystem + entityName + "SControllerEvent";
            this.controllerName = controllerName;
            this.aliasName = aliasName;
            this.AfterCompile = () => {
                this.SetController();
                this.AfterLoaded();
            }
        }

        SetController() {
            let fullControllerName = this.controllerName + ' as ' + this.aliasName;
            let dom_element = document.getElementById(this.id).querySelector(`[ng-controller="` + fullControllerName + `"]`);
            let ng_element = angular.element(dom_element);
            let evalString = "ng_element.scope()." + this.aliasName;
            if (dom_element != null) {
                let scope = eval(evalString);
                this.controller = <TController>scope;
            }
        }

        ControllerNotAssigned() {
            return this.controller == null;
        }

        CallFunction<U>(functionName: U) {
            if (this.ControllerNotAssigned())
                this.SetController();
            return eval(nodak.PropertyManipulating.GetNameForPartialView(functionName));
        }

        GetProperty<U>(propertyName: U) {
            if (this.ControllerNotAssigned())
                this.SetController();
            return eval(nodak.PropertyManipulating.GetNameForPartialView(propertyName));
        }

        SetProperty<U>(propertyName: U, value: any) {
            if (this.ControllerNotAssigned())
                this.SetController();
            return eval(nodak.PropertyManipulating.GetNameForPartialView(propertyName) + "=" + "'" + value + "';");
        }
    }
}
