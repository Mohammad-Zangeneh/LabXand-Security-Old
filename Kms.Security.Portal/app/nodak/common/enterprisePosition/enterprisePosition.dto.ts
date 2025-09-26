module nodak.common.models {
    export class EnterprisePositionDto {
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

        private parent: models.EnterprisePositionDto
        set Parent(value)
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

        private organization: models.OrganizationDto
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
