module nodak.common.models {
    export class OrganizationDto {
        private id: string;
        set Id(value: string) {
            this.id = value;
        }
        get Id() {
            return this.id;
        }


        private name: string;
        set Name(value: string) {
            this.name = value;
        }
        get Name() {
            return this.name;
        }

        private parentId: string;
        set ParentId(value: string) {
            this.parentId = value;
        }
        get ParentId() {
            return this.parentId;
        }
        private sortingNumber: number;
        set SortingNumber(value: number) {
            this.sortingNumber = value;
        }
        get SortingNumber() {
            return this.sortingNumber;
        }
        //private phone: string;
        //set Phone(value: string) {
        //    this.phone = value;
        //}
        //get Phone() {
        //    return this.phone;
        //}

        //private website: string;
        //set Website(value: string) {
        //    this.website = value;
        //}
        //get Website() {
        //    return this.website;
        //}

        //private address: string;
        //set Address(value: string) {
        //    this.address = value;
        //}
        //get Address() {
        //    return this.address;
        //}

        private enterprisePositions: any;
        set EnterprisePositions(value: string) {
            this.enterprisePositions = value;
        }
        get EnterprisePositions() {
            return this.enterprisePositions;
        }
        //
        private parent: models.OrganizationDto;
        set Parent(value: OrganizationDto) {
            this.parent = value;
        }
        get Parent() {
            return this.parent;
        }


    }
}