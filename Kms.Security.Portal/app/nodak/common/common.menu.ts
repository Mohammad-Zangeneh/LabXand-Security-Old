
module nodak.common.models {
    import core = labxand.components.core;
    export class commonMenu extends labxand.components.core.SubMenuBase {
        constructor() {

            super();
            //this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
            //    .HasRoot(labxand.components.core.MenuRoot.Admin)
            //    .HasId("CompanyManagement").HasAuthorizedOperation(["CompanyManagement"]).HasCaption("مدیریت شرکت")
            //    .HasParent("BasicInformation").HasHref("/Company").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);

            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("OrganizationManagement").HasAuthorizedOperation(["OrganizationManagement"]).HasCaption("مدیریت سازمان")
                .HasParent("BasicInformation").HasHref("/Organization").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);

            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("RoleManagement").HasAuthorizedOperation(["RoleManagement"]).HasCaption("مدیریت نقش ها")
                .HasParent("BasicInformation").HasHref("/Role").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);

            //this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
            //    .HasRoot(labxand.components.core.MenuRoot.Admin)
            //    .HasId("PermissionManagement").HasAuthorizedOperation(["PermissionManagement"]).HasCaption("مدیریت دسترسی ها")
            //    .HasParent("BasicInformation").HasHref("/Permission").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);


            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "مدیریت دسترسی ها", "PermissionManagement", "/Permission");
            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("EnterprisePositionPostManagement").HasAuthorizedOperation(["EnterprisePositionPostManagement"]).HasCaption("مدیریت پست ها سازمانی")
                .HasParent("BasicInformation").HasHref("/EnterprisePositionPost").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "مدیریت پست های سازمانی", "EnterprisePositionPostManagement", "/EnterprisePositionPost");

            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("EnterprisePositionManagement").HasAuthorizedOperation(["EnterprisePositionManagement"]).HasCaption("مدیریت چارت ها سازمانی")
                .HasParent("BasicInformation").HasHref("/EnterprisePosition").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "مدیریت چارت سازمانی", "EnterprisePositionManagement", "/EnterprisePosition");
            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("UserManagement").HasAuthorizedOperation(["ViewMember","SaveMember"]).HasCaption("مشاهده تمام اعضا")
                .HasParent("UserManagement").HasHref("/Member").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "مشاهده تمام اعضا", "UserManagement", "/Member");
            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("UserRegister").HasAuthorizedOperation(["SaveMember"]).HasCaption("ثبت عضو جدید")
                .HasParent("UserManagement").HasHref("/Member?status=Register").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "ثبت عضو جدید", "RegisterMember", "/Member?status=Register");
            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("RegisterRequest").HasAuthorizedOperation(["ViewMember", "SaveMember"]).HasCaption("مدیریت درخواست های عضویت")
                .HasParent("UserManagement").HasHref("/Member?status=RegisterRequest").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "مشاهده درخواست های عضویت", "REquestRegister", "/Member?status=RegisterRequest");
            this.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                .HasRoot(labxand.components.core.MenuRoot.Admin)
                .HasId("ChangePassword").HasAuthorizedOperation([]).HasCaption("تغییر رمز ورود")
                .HasParent("Setting").HasHref("/ChangePassword").HasIcon("fa fa-lightbulb-o fa-lg mt-1").HasChild(false);
            //this.AddRootNode(core.MenuRoot.Admin, "Common-Menu-View", "تغییر رمز ورود", "ChangePassword", "/ChangePassword");



        }
    }
}
