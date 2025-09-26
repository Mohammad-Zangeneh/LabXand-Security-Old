module nodak.common.models {

    export class Tag extends nodak.models.ModelBase{

        constructor() {
            super(nodak.enums.SubSystems.Common, "Tag");
        }
        AddBussinessModelValidation() {

        }
        private id:any
        set Id(value)  
         { this.id = value; }
        get Id()  
         { return this.id; }

        private name:string
        set Name(value)  
         { this.name = value; }
        get Name()  
         { return this.name; }

        private description:string
        set Description(value)  
         { this.description = value; }
        get Description()  
         { return this.description; }

        private count:Number
        set Count(value)  
         { this.count = value; }
        get Count()  
         { return this.count; }


    }
}
