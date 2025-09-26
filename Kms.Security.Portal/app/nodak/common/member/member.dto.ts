module nodak.common.models {
    export class MemberDto {
        private id: string;
        set Id(value) {
            this.id = value;
        }
        get Id() {
            return this.id;
        }
        //=============
        private lastName: string;
        set LastName(value) {
            this.lastName = value;
        }
        get LastName() {
            return this.lastName;
        }
        //===========
        private firstName: string;
        set FirstName(value) {
            this.firstName = value;
        }
        get FirstName() {
            return this.firstName;
        }
        //=====================
        private userName: string;
        set UserName(value) {
            this.userName = value;
        }
        get UserName() {
            return this.userName;
        }
        //==============
        private email: string;
        set Email(value) {
            this.email = value;
        }
        get Email() {
            return this.email;
        }
      
        //==========================
        private password: string;
        set Password(value) {
            this.password = value;
        }
        get Password() {
            return this.password;
        }
     
        private fullName: string;
        get FullName() {

            return this.fullName;
        }
        set FullName(value) {
            this.fullName = value;
        }

        //protected dynamicPermissions: Array<DynamicPermissionDto>;
        //set DynamicPermissions(value) {
        //    this.dynamicPermissions = value;
        //}
        //get DynamicPermissions() {
        //    return this.dynamicPermissions;
        //}
        protected roles: Array<models.RoleDto>;
        set Roles(value) {
            this.roles = value;
        }
        get Roles() {
            return this.roles;
        }

        protected enterprisePositionPosts: Array<models.EnterprisePositionPostDto>;
        set EnterprisePositionPosts(value) {
            this.enterprisePositionPosts = value;
        }
        get EnterprisePositoinPosts() {
            return this.enterprisePositionPosts;
        }

        private enterprisePosition: models.EnterprisePositionDto;
        set EnterprisePosition(value) {
            this.enterprisePosition = value;
        }
        get EnterprisePosition() {
            return this.enterprisePosition;
        }

        private enterprisePositionId: string;
        set EnterprisePositionId(value) {
            this.enterprisePositionId = value;
        }
        get EnterprisePositionId() {
            return this.enterprisePositionId;
        }


        private organization: models.OrganizationDto;
        set Organization(value) {
            this.organization = value;
        }
        get Organization() {
            return this.organization;
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
        get UserSatuts() {
            return this.userStatus ;
        }

        private userStatusValue: string;
        set UserStatusValue(value) {
            this.userStatusValue = value;
        }
        get UserStatusValue() {
            return this.userStatusValue;
        }
        private isSuperAdmin: boolean;
        set IsSuperAdmin(value) {
            this.isSuperAdmin = value;
        }
        get IsSuperAdmin() {
            return this.isSuperAdmin;
        }

        private registerationDate: Date;
        set RegisterationDate(value) {
            this.registerationDate = value;
        }
        get RegisterationDate() {
            return this.registerationDate;
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