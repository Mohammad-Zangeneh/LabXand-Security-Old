module nodak.common.models {
    export class MemberSearchModel {
        constructor(UserStatus = nodak.Tools.UserStatus.Active) {
            this.UserStatus = UserStatus ;
        }
        private userName: string;
        get UserName() {
            return this.userName;
        }
        set UserName(value) {
            this.userName = value;
        }

        private lastName: string;
        get LastName() {
            return this.lastName;
        }
        set LastName(value) {
            this.lastName = value;
        }

        private firstName: string;
        set FirstName(value) {
            this.firstName = value;
        }
        get FirstName() {
            return this.firstName;
        }

        private organizationId: string;
        set OrganizationId(value) {
            this.organizationId = value;
        }
        get OrganizationId() {
            return this.organizationId;
        }
        private userStatus: nodak.Tools.UserStatus;
        set UserStatus(value) {
            this.userStatus = value;
        }
        get UserStatus() {
            return this.userStatus;
        }

        private email: string;
        set Email(value) {
            this.email = value; 
        }
        get Email() {
            return this.email;
        }
        private isAdmin: boolean;
        set IsAdmin(value) {
            this.isAdmin = value;
        }
        get IsAdmin() {
            return this.isAdmin;
        }
        private personnelNumber;
        set PersonnelNumber(value) {
            this.personnelNumber = value;
        }
        get PersonnelNumber() {
            return this.personnelNumber;
        }
    }
}