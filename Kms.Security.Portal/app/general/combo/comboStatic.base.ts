module nodak.models {
    export class HeaderStaticCombo {
        field: string
        title: string;
        htmlAttribute: string;
    }

    export class BaseStaticCombo<T>
    {
        displayMember: string;
        valueMember: string;
        headers: Array<HeaderStaticCombo>;
        model: Array<T>;
    }
}
