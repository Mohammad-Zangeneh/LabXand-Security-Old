module nodak.common.models {

    export class EnterprisePosition extends nodak.models.ModelBase {

        constructor() {
            super(enums.SubSystems.Common, "EnterprisePosition");
        }
        AddBussinessModelValidation() {

        }
        private id: any
        set Id(value)
        { this.id = value; }
        get Id()
        { return this.id; }

        private name: string
        set Name(value)
        { this.name = value; }
        get Name()
        { return this.name; }

        private parent: EnterprisePosition
        set Parent(value: EnterprisePosition)
        { this.parent = value; }
        get Parent()
        { return this.parent; }

        private parentId: any
        set ParentId(value)
        { this.parentId = value; }
        get ParentId()
        { return this.parentId; }

        private organizationId: any
        set OrganizationId(value)
        { this.organizationId = value; }
        get OrganizationId()
        { return this.organizationId; }

        private organization:models.Organization
        set Organization(value)
        { this.organization = value; }
        get Organization()
        { return this.organization; }

        private sortingNumber;
        set SortingNumber(value) {
            this.sortingNumber = value;
        }
        get SortingNumber() {
            return this.sortingNumber;
        }

    }
}
