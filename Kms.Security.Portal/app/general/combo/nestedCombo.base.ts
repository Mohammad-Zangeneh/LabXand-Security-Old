 module nodak.models {
    export class HeaderNestedCombo {
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

    export interface IBaseNestedCombo<T> {
        displayMember: string;
        valueMember: string;

        parentName: string;

        headers: Array<HeaderNestedCombo>;
        service: nodak.service.IServiceBase<T>
        selectedId: string;
        selectedText: string;
    }

    export class BaseNestedCombo<T> implements IBaseNestedCombo<T>{
        protected model: T;
        displayMember: string;
        valueMember: string;

        parentName: string;

        headers: Array<HeaderNestedCombo>;
        service: nodak.service.IServiceBase<T>;
        selectedId: string;
        selectedText: string;

        constructor() {
            this.headers = [];
        }

        protected Bound<TResult>(property: TResult) {
            var header = new HeaderNestedCombo();
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

        protected IsParentName<TResult>(parentName: TResult) {
            this.parentName = nodak.PropertyManipulating.GetName(parentName);
        }
    }
}
