module nodak {
    export class PropertyManipulating {

        private static propertyName: string;

        static GetNameForPartialView<TResult>(name: TResult): string {
            this.propertyName = "";
            let varExtractor = new RegExp("return (.*);");
            let m = varExtractor.exec(name + "");
            let section1 = m[1].split('.');
            this.propertyName = "this.";
            for (let i = 2; i < section1.length; i++) {
                this.propertyName += section1[i]
                if (section1.length - 1 != i)
                    this.propertyName += ".";
            }
            //console.log(this.propertyName);
            return this.propertyName;
        }

        static GetNameModelGrid<TResult>(name: TResult): string {
            this.propertyName = "";
            let varExtractor = new RegExp("return (.*);");
            let m = varExtractor.exec(name + "");
            this.propertyName = m[1].toString().replace('_this.Model.', '')
            this.propertyName = this.propertyName.replace('[0]', '');
            return this.propertyName;
        }

        static GetNameSearchModelGrid<TResult>(name: TResult): string {
            this.propertyName = "";
            var varExtractor = new RegExp("return (.*);");
            var m = varExtractor.exec(name + "");
            this.propertyName = m[1].toString().replace('_this.SearchModel.', '')

            return this.propertyName;
        }

        static GetFullNameOfProperty<TResult>(name: TResult): string {
            let propertyName = "";
            let varExtractor = new RegExp("return (.*);");
            propertyName = varExtractor.exec(name + "")[1].toString().substr(1);;
            //console.log(propertyName);
            return propertyName;
        }

        static GetName<TResult>(name: TResult): string {
            this.propertyName = "";
            let varExtractor = new RegExp("return (.*);");
            let m = varExtractor.exec(name + "");

            if (m != null) {
                let ss = m[1].toString().split('.');
                this.propertyName = ss[ss.length - 1];
            }

            if (m == null) throw new Error("The function does not contain a statement matching 'return variableName;'");

            return this.propertyName;
        }

        static ConvertToPascalCase(value: string) {
            return value.slice(0, 1).toUpperCase() + value.slice(1, value.length + 1);

        }
    }
}
