module labxand.components.core {
    //Columns
    export interface IGridColumns {
        section1: string;
        section2: string;
        section3: string;
        Hidden: boolean;
        Title: string;
        HtmlAttribute: string;
        HeaderHtmlAttributes: string;
        Filter: IFilterGrid;
        WithoutFilter: boolean;
        oneSection: boolean;
        twoSection: boolean;
        threeSection: boolean;
        field: string;
        trStyle: string;
    }

    export class GridColumns implements IGridColumns {
        section1: string;
        section2: string;
        section3: string;
        section4: string;
        private width: number=100;
        Hidden: boolean;
        Title: string;
        HtmlAttribute: string;
        HeaderHtmlAttributes: string;
        Filter: IFilterGrid;
        WithoutFilter: boolean;
        oneSection: boolean;
        twoSection: boolean;
        threeSection: boolean;
        fourSection: boolean;
        field: string;
        trStyle: string;
        FilterName: string;


        constructor(propertyName: string) {
            this.field = propertyName;
            let sectionOfPropertyName = propertyName.split('.');
            this.section1 = sectionOfPropertyName[0];
            this.section2 = sectionOfPropertyName[1];
            this.section3 = sectionOfPropertyName[2];
            this.section4 = sectionOfPropertyName[3];

            if (sectionOfPropertyName.length == 1)
                this.oneSection = true;
            else if (sectionOfPropertyName.length == 2)
                this.twoSection = true;
            else if (sectionOfPropertyName.length == 3)
                this.threeSection = true;
            else if (sectionOfPropertyName.length == 4)
                this.fourSection = true;

            this.WithoutFilter = true;
        }

        Width(width: number) {
            this.width = width;
            if (!this.trStyle) this.trStyle = ""; this.trStyle += 'min-width:' + width + 'px;width:' + width + 'px;'; return this;
        }
        IsHidden() { this.Hidden = true; return this; }
        HasTitle(title: string) { this.Title = title; return this; }
        HasHtmlAttribute(htmlAttribute: string) { this.HtmlAttribute = htmlAttribute; return this; }
        HasHeaderHtmlAttributes(headerHtmlAttribute: string) { this.HeaderHtmlAttributes = headerHtmlAttribute; return this; }
        HasFilter(filter: IFilterGrid) {
            this.WithoutFilter = false; this.Filter = filter;
            this.FilterName = filter.constructor.name
        return this;
        }
        IsLTR() { if (!this.trStyle) this.trStyle = ""; this.trStyle += "direction:ltr!important;"; this.trStyle += " text-align:left!important;" }


    }
   
}