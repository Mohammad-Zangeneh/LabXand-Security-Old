module nodak.common.models {
    export class Member extends nodak.models.ModelBase  {
        constructor() {
            super(enums.SubSystems.Common,"Member");
            //this.model = new kms.userManagement.models.MemberDto(); 
            this.Guard(() => this.LastName, "نام خانوادگی").IsRequired();
            this.Guard(() => this.FirstName, "نام ").IsRequired();
            //this.Guard(() => this.Password, "رمز عبور ").IsRequired();
            this.Guard(() => this.UserName, "نام کاربری ").IsRequired();
            this.Guard(() => this.Email, "ایمیل ").IsRequired().IsValidEmail();
            this.UserStatus = nodak.Tools.UserStatus.Active;
        }
        AddBussinessModelValidation() { }
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
        //==========================

        private confirmPassword: string;
        set ConfirmPassword(value) {
            this.confirmPassword = value;
        }
        get ConfirmPassword() {
            return this.confirmPassword;
        }
        //==========================
     


        private fullName: string;
        get FullName() {

            return this.fullName;
        }
        set FullName(value) {
            this.fullName = value;
        }

        //protected dynamicPermissions: Array<DynamicPermission>;
        //set DynamicPermissions(value) {
        //    this.dynamicPermissions = value;
        //}
        //get DynamicPermissions() {
        //    return this.dynamicPermissions;
        //}

        private roles: Array<models.Role>;
        set Roles(value) {
            this.roles = value;
        }
        get Roles() {
            return this.roles;
        }

        private enterprisePositionPosts: Array<models.EnterprisePositionPost>;
        set EnterprisePositionPosts(value) {
            this.enterprisePositionPosts = value;
        }
        get EnterprisePositionPosts() {
            return this.enterprisePositionPosts;
        }

        private enterprisePosition: models.EnterprisePosition;
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


        private organization: models.Organization;
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
        get UserStatus() {
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