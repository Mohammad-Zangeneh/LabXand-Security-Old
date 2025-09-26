module labxand.components.core {
    export class Button {
        id: string;
        idButton: string;
        className: string;
        title: string;
        caption: string;
        visible: boolean;
        disabled: boolean;
        OnClick: Function;
        constructor(id: string) {
            this.visible = true;
            this.disabled = false;
            this.idButton = id + new Date().getTime();
            this.id = id;
        }

        HasTitle(title: string) {
            this.title = title;
            return this;
        }

        HasCaption(caption: string) {
            this.caption = caption;
            return this;
        }

        HasClass(className: string) {
            this.className = className;
            return this;
        }

        SetOnClick(func: Function) {
            this.OnClick = func;
            return this;
        }

        Disable() {
            this.disabled = true;
        }

        Enable() {
            this.disabled = false;
        }

        Visible() {
            this.visible = true;
        }

        InVisible() {
            this.visible = false;
        }
    }

    export interface IActionPanel {
        buttons: Array<Button>;
        panelTitle: string;
        pageType: nodak.enums.PageType;
        visible: boolean;
        AddButton(id: string): Button;
        Visible();
        InVisible();
        Enable();
        Disable();
        SetOnClickButtonById(id: string, func: Function);
    }

    export class ActionPanelBase implements IActionPanel {
        buttons: Array<Button>;
        panelTitle: string;
        pageType: nodak.enums.PageType;
        visible: boolean;

        constructor() {
            this.buttons = new Array<Button>();
            this.visible = true;
        }

        Visible() { this.visible = true; }
        InVisible() { this.visible = false; }
        Enable() { }
        Disable() { }

        AddButton(id: string) {
            let setupButton = new Button(id);
            this.buttons.push(setupButton);
            return setupButton;
        }

        HasTitle(title: string) { this.panelTitle = title; }

        DisbaleButtonById(id: string) {
            let button = this.buttons.filter(q => q.id == id)[0];
            if (button != null)
                button.Disable();
        }

        EnableButtonById(id: string) {
            let button = this.buttons.filter(q => q.id == id)[0];
            if (button != null)
                button.Enable();
        }

        VisibleButtonById(id: string) {
            let button = this.buttons.filter(q => q.id == id)[0];
            if (button != null)
                button.Visible();
        }

        InVisibleButtonById(id: string) {
            let button = this.buttons.filter(q => q.id == id)[0];
            if (button != null)
                button.InVisible();
        }

        SetOnClickButtonById(id: string, func: Function) {
            let button = this.buttons.filter(q => q.id == id)[0];
            if (button != null)
                button.SetOnClick(func);
        }
    }


}