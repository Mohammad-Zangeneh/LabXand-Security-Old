module labxand.components.core {
    export enum MenuRoot {
        Dashboard = 1,
        Register,
        Search,
        Appraissing,
        PersonalItem,
        Contact,
        Forum,
        Association,
        Report,
        Admin
    }
    export enum SubSystems {
        Common = 1,


    }


    export class Menu {
        private id: string;
        set Id(value) {
            this.id = value;
        }
        get Id() {
            return SubSystems[this.subSystem] + MenuRoot[this.menuRoot] + this.id;
        }

        href: string;
        authorizedOperation: Array<string>;
        title: string;
        caption: string;
        isSubMenu: boolean;
        isRootMenu: boolean;
        menuRoot: MenuRoot;
        subSystem: SubSystems;
        mainParent: string;
        openInNewWindow: boolean = false;
        private paretnNode: string;
        icon: string;
        set ParetnNode(value) {
            this.paretnNode = value;
        }
        get ParetnNode() {
            return this.paretnNode;
        }
        OpenInNewWindow() {
            this.openInNewWindow = true;
            return this;
        }
        GetParentNode() {
            return SubSystems[this.subSystem] + MenuRoot[this.menuRoot] + this.paretnNode;

        }

        hasChild: boolean = false;

        IsSubMenu() {
            this.isSubMenu = true;
            return this;
        }

        IsRootMenu() {
            this.isRootMenu = true;
            return this;
        }


        HasTitle(title: string) {
            this.title = title;
            return this;
        }
        HasIcon(icon: string) {
            this.icon = icon;
            return this;
        }
        HasCaption(caption: string) {
            this.caption = caption;
            return this;
        }

        HasRoot(menuRoot: MenuRoot) {
            this.menuRoot = menuRoot;
            return this;
        }

        HasAuthorizedOperation(authorizedOperation: Array<string>) {
            this.authorizedOperation = authorizedOperation;
            return this;
        }

        HasParent(parentNodeId: string) {
            this.paretnNode = parentNodeId;
            return this;
        }

        HasChild(value = true) {
            this.hasChild = value;
            return this;
        }

        HasId(id: string) {
            this.id = id;
            return this;
        }

        SetSubSystems(subSystem: SubSystems) {
            this.subSystem = subSystem;
            return this;
        }

        HasHref(href: string, fullAddress = false) {
            let base = Base.Config.AppRoot;
            if (fullAddress)
                base = "";
            if (href != "#")
                this.href = base + href;
            else
                this.href = "#";
            return this;
        }
    }

    export class SubMenuBase {
        menus: Array<Menu>;

        constructor() {
            this.menus = new Array<Menu>();
        }

        IdGenerator(id: string) {
            return id + "_Menu";
        }

        AddSubMenuNode() {
            let menu: Menu = new Menu();
            menu.IsSubMenu().HasParent(null).HasChild();
            this.menus.push(menu);
            return menu;
        }

        CreateRootMenu() {

            this.AddSubMenuNode().SetSubSystems(SubSystems.Common).HasRoot(MenuRoot.Dashboard).HasId("Main").HasAuthorizedOperation([])
                .HasCaption("خانه").HasHref("/");

            this.AddSubMenuNode().SetSubSystems(SubSystems.Common).HasRoot(MenuRoot.Admin).HasId("BasicInformation")
                .HasAuthorizedOperation([
                    "CompanyManagement", "OrganizationManagement", "RoleManagement", "PermissionManagement", "EnterprisePositionPostManagement", "EnterprisePositionManagement"
                   
                ])
                .HasCaption("اطلاعات پایه").HasHref("#");
            this.AddSubMenuNode().SetSubSystems(SubSystems.Common).HasRoot(MenuRoot.Admin).
                HasId("UserManagement").HasAuthorizedOperation([
                    , "ViewMember", "SaveMember"
                ]).HasCaption("کاربران").HasHref("#");
            this.AddSubMenuNode().SetSubSystems(SubSystems.Common).HasRoot(MenuRoot.Admin)
              .HasId("Setting").HasAuthorizedOperation([]).HasCaption("تنظیمات").HasHref("#");

        }
    }


    export class MenuBase {
        menus: Array<Menu>;
        subSystem: SubSystems;
        rootId: string;
        constructor(subSystem: SubSystems) {
            this.subSystem = subSystem;
            this.menus = new Array<Menu>();
            let entranceExitNode = new Menu();
            this.rootId = "Root";
        }

        AddRootNode(menuType: MenuRoot,
            authorizedOperation: string,
            caption: string,
            id: string, href: string) {
            let menu: Menu = new Menu();
            menu.isRootMenu = true;
            menu.subSystem = this.subSystem;
            menu.href = href;
            menu.mainParent = core.SubSystems[core.SubSystems.Common] + core.MenuRoot[menuType] + "Main";
            menu.HasChild().
                HasId(id).
                //HasAuthorizedOperation(authorizedOperation).
                HasCaption(caption)
            //HasParent(core.SubSystems[core.SubSystems.All] + core.MenuType[menuType] + "Main").
            //HasType(menuType);
            this.menus.push(menu);
        }

        //AddNodeToRoot(menuType: MenuType, parentId: string) {
        //    let menu: Menu = new Menu();
        //    let fullParentId = SubSystems[this.subSystem] + MenuType[menuType];
        //    if (parentId != null)
        //        menu.HasParent(parentId);
        //    else
        //        menu.HasParent(this.rootId);
        //    menu.SetSubSystems(this.subSystem).HasType(menuType);
        //    this.menus.push(menu);
        //    return menu;
        //}

        AddNode(menuType: MenuRoot, parentId: string) {
            let menu: Menu = new Menu();
            //menu.SetSubSystems(this.subSystem).HasType(menuType).HasParent(parentId);
            this.menus.push(menu);
            return menu;
        }

    }

}

////module labxand.components.core {
////    export enum MenuType {
////        Crud = 1,
////        Search = 2,
////        Report = 3,
////        BaseInformation = 4,
////        dashboard = 5,
////        UserManager = 6,
////        Personal=7
////    }
////    export enum SubSystems {
////        Common = 1,
////        Communications = 2,
////        EntranceExit = 3,
////        Salvation = 4,
////        Environment = 5,
////        PortStateControl = 6,
////        All = 7,
////        VesselFlag = 8,

////    }

////    export enum EntranceExitRootMenu {
////        Zamanha = 1,
////        Darkhastha = 2
////    }

////    export class Menu {
////        private id: string;
////        set Id(value) {
////            this.id = value;
////        }
////        get Id() {
////            return SubSystems[this.subSystem] + MenuType[this.menuType] + this.id;
////        }

////        href: string;
////        authorizedOperation: string;
////        title: string;
////        caption: string;
////        isSubMenu: boolean;
////        isRootMenu: boolean;
////        menuType: MenuType;
////        subSystem: SubSystems;
////        mainParent: string;
////        private paretnNode: string;
////        set ParetnNode(value) {
////            this.paretnNode = value;
////        }
////        get ParetnNode() {
////            return SubSystems[this.subSystem] + MenuType[this.menuType] + this.paretnNode;
////        }

////        hasChild: boolean;

////        IsSubMenu() {
////            this.isSubMenu = true;
////            return this;
////        }

////        IsRootMenu() {
////            this.isRootMenu = true;
////            return this;
////        }


////        HasTitle(title: string) {
////            this.title = title;
////            return this;
////        }

////        HasCaption(caption: string) {
////            this.caption = caption;
////            return this;
////        }

////        HasType(menuType: MenuType) {
////            this.menuType = menuType;
////            return this;
////        }

////        HasAuthorizedOperation(authorizedOperation: string) {
////            this.authorizedOperation = authorizedOperation;
////            return this;
////        }

////        HasParent(parentNode: string) {
////            this.paretnNode = parentNode;
////            return this;
////        }

////        HasChild() {
////            this.hasChild = true;
////            return this;
////        }

////        HasId(id: string) {
////            this.id = id;
////            return this;
////        }

////        SetSubSystems(subSystem: SubSystems) {
////            this.subSystem = subSystem;
////            return this;
////        }

////        HasHref(href: string) {
            
////            if (href != "#")
////                this.href = Base.Config.AppRoot + href;
////            else
////                this.href = "#"; 
////            return this;
////        }
////    }

////    export class SubMenuBase {
////        menus: Array<Menu>;

////        constructor() {
////            this.menus = new Array<Menu>();
////        }

////        IdGenerator(id: string) {
////            return id + "_Menu";
////        }

////        AddSubMenuNode() {
////            let menu: Menu = new Menu();
////            menu.IsSubMenu().HasParent(null).HasChild();
////            this.menus.push(menu);
////            return menu;
////        }

////        CreateSubMenu() {
////            this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.dashboard).HasId("Main").HasAuthorizedOperation("dfsd").HasCaption("خانه").HasHref("/");
////            this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.Crud).HasId("Main").HasAuthorizedOperation("dfsd").HasCaption("اطلاعات پایه").HasHref("#");
////            this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.UserManager).HasId("Main").HasAuthorizedOperation("dfsd").HasCaption("کاربران").HasHref("#");
////            this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.Personal)
////                .HasId("Main").HasAuthorizedOperation("dfsd").HasCaption("تنظیمات").HasHref("#");
////            //this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.Crud).HasId("Main").HasAuthorizedOperation("dfd").HasCaption("ایجاد اسناد");
////            //this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.Search).HasId("Main").HasAuthorizedOperation("jj").HasCaption("جستجوی اسناد");
////            //this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.BaseInformation).HasId("Main").HasAuthorizedOperation("jj").HasCaption("اطلاعات پایه");
////            //this.AddSubMenuNode().SetSubSystems(SubSystems.All).HasType(MenuType.Report).HasId("Main").HasAuthorizedOperation("jj").HasCaption("گزارشات")
////        }
////    }


////    export class MenuBase {
////        menus: Array<Menu>;
////        subSystem: SubSystems;
////        rootId: string;
////        constructor(subSystem: SubSystems) {
////            this.subSystem = subSystem;
////            this.menus = new Array<Menu>();
////            let entranceExitNode = new Menu();
////            this.rootId = "Root";
////        }

////        AddRootNode(menuType: MenuType,
////            authorizedOperation: string,
////            caption: string,
////            id: string, href: string) {
////            let menu: Menu = new Menu();
////            menu.isRootMenu = true;
////            menu.subSystem = this.subSystem;
////            menu.href = href;
////            menu.mainParent = core.SubSystems[core.SubSystems.All] + core.MenuType[menuType] + "Main";
////            menu.HasChild().
////                HasId(id).
////                HasAuthorizedOperation(authorizedOperation).
////                HasCaption(caption).
////                HasParent(core.SubSystems[core.SubSystems.All] + core.MenuType[menuType] + "Main").
////                HasType(menuType);
////            this.menus.push(menu);
////        }

////        //AddNodeToRoot(menuType: MenuType, parentId: string) {
////        //    let menu: Menu = new Menu();
////        //    let fullParentId = SubSystems[this.subSystem] + MenuType[menuType];
////        //    if (parentId != null)
////        //        menu.HasParent(parentId);
////        //    else
////        //        menu.HasParent(this.rootId);
////        //    menu.SetSubSystems(this.subSystem).HasType(menuType);
////        //    this.menus.push(menu);
////        //    return menu;
////        //}

////        AddNode(menuType: MenuType, parentId: string) {
////            let menu: Menu = new Menu();
////            menu.SetSubSystems(this.subSystem).HasType(menuType).HasParent(parentId);
////            this.menus.push(menu);
////            return menu;
////        }

////    }

////}