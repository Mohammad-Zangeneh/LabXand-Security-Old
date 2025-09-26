module nodak.models {
    export class HeaderCombo {
        field: string
        title: string;
        htmlAttribute: string;

        Title(title: string) {
            this.title = title;
            return this;
        }

        HtmlAttribute(htmlAttribute: string) {
            this.htmlAttribute = htmlAttribute;
            return this;
        }

    }

    export interface IBaseMultiSelectCombo<T> {
        displayMember: string;
        valueMember: string;
        headers: Array<HeaderCombo>;
        service: nodak.service.IServiceBase<T>
        selectedId: string[];
        selectedText: string[];
    }

    export interface IBaseCombo<T> {
        displayMember: string;
        valueMember: string;
        headers: Array<HeaderCombo>;
        service: nodak.service.IServiceBase<T>
        selectedId: string;
        selectedText: string;
        BeforSelect(id, selectedModel): void;
        AfterSelect(id, selectedModel): void;
        AfterChange(id, selectedModel): void;
        comboModel: Array<T>;
        selectedModel: any;
    }

    export class BaseCombo<T> implements IBaseCombo<T>{
        protected model: T;
        displayMember: string;
        valueMember: string;
        headers: Array<HeaderCombo>;
        service: nodak.service.IServiceBase<T>;
        selectedId: string;
        selectedText: string;
        comboModel: Array<T>;
        selectedModel: any;
        constructor() {
            this.headers = [];
        }

        BeforSelect(id, selectedModel): void { }
        AfterSelect(id, selectedModel): void { }
        AfterChange(id, selectedModel): void { }
        public GetData() {
            return this.service.GetArray(enums.ServiceTypeEnum.Combo);
        }
        protected Bound<TResult>(property: TResult) {
            var header = new HeaderCombo();
            header.field = nodak.PropertyManipulating.GetName(property);
            this.headers.push(header);
            return header;
        }

        protected IsValueMember<TResult>(valueMember: TResult) {
            this.valueMember = nodak.PropertyManipulating.GetName(valueMember);
        }

        protected IsDisplayMember<TResult>(displayMember: TResult) {
            this.displayMember = nodak.PropertyManipulating.GetName(displayMember);
        }
    }

}
