
module labxand.components.controller {
    export class MenuController {
        FullName: string;
        LogoutCaption = "خروج";
        menuArray: Array<core.Menu>;
        Date: any;
        trueMode: boolean = true;
        //عکس کاربر
        AddPictureModal: labxand.components.core.ModalSectionOperation;
        CurrentMemberEntity: nodak.common.models.Member;

        ModalDirective: labxand.components.core.ModalSectionOperation;
        //startTime()
        //{
        //    debugger;
        //var today = new Date();
        //var h = today.getHours();
        //var m = today.getMinutes();
        //m = this.checkTime(m);
        //document.getElementById('txt').innerHTML =
        //    h + ":" + m;
        //var t = setTimeout(this.startTime, 500);
        //}
        //checkTime(i)
        //{
        //    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
        //    return i;
        //}

        CreateElement(element: string, className: string) {
            let mainElement = document.createElement(element);
            if (className != null)
                mainElement.className = className;
            return mainElement;
        }


        GetUserInformation() {
            var obj = JSON.parse(localStorage.getItem("User"));
            if (obj != null) {
                this.FullName = obj.FullName;
            }
        }

        getParameterByName(name) {
            var url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        CheckToken() {
            //var token = this.getParameterByName('token');
            //var redirectPath = this.getParameterByName('redirectPath');
            //if (token != null) {
            //    localStorage.setItem("authenticationToken", token);
            //}
            //if (redirectPath != null)
            //    localStorage.setItem("redirectPath", redirectPath);
            //if (localStorage.getItem("redirectPath") != undefined) {
            //    this.LogoutCaption =  'پرتال';
            //}
        }
        Logout() {
            //if (localStorage.getItem("redirectPath") != undefined) {
            //    var temp = localStorage.getItem("redirectPath");
            //    localStorage.clear();
            //    window.location.href = temp;
            //    return;
            //}
            var token = localStorage.getItem("authenticationToken");


            $.ajax({
                //headers: { 'Authorization': 'Bearer ' + token },
                url: Base.Config.AppRoot + "/logout",
                type: 'POST'
            }).then(function (response) {
                localStorage.clear();
                window.location.href = Base.Config.LoginPage + "?redirectPath=" + Base.Config.AppRoot;

            }, function (xhr, status, error) {
                localStorage.clear();
                window.location.href = Base.Config.LoginPage + "?redirectPath=" + Base.Config.AppRoot;
            });
        }
      

        //morsa
        CreatMenuElement(item: core.Menu) {
            let operations: Array<any> = JSON.parse(localStorage.getItem("Permissions"));
            if (operations != null) {
                let temp = false;
                item.authorizedOperation.forEach((w) => {
                    if (operations.filter(q => q.Code == w))
                    if (operations.filter(q => q.Code== w).length != 0) {
                            temp = true;
                        }
                });
                if (temp == false && item.authorizedOperation.length!=0)
                    return;
            }
            let liMainElement;
            let icon;
            if (item.icon != undefined)
                icon = this.CreateElement('li', item.icon);

            if (item.hasChild)
                liMainElement = this.CreateElement('li', 'nav-item nav-dropdown');
            else
                liMainElement = this.CreateElement('li', 'nav-item');

            let aElement;
            if (item.hasChild)
                aElement = this.CreateElement("a", "nav-link nav-dropdown-toggle");
            else
                aElement = this.CreateElement("a", "nav-link");


            if (item.href == null)
                item.href = "#";
            if (item.openInNewWindow)
                aElement.setAttribute("target", "_blank");
            aElement.setAttribute("href", item.href);
            var newBaitText = document.createTextNode("  " + item.caption);
            if (icon != undefined) {

                aElement.appendChild(icon);
            }
            aElement.appendChild(newBaitText);

            liMainElement.appendChild(aElement);
            liMainElement.id = item.Id;

            if (item.ParetnNode == null)
                document.getElementById('testMenu').appendChild(liMainElement);
            else {
                let ulMainElement = this.CreateElement('ul', 'nav-dropdown-items');
                ulMainElement.appendChild(liMainElement);
                document.getElementById(item.GetParentNode()).appendChild(ulMainElement);

            }

        }

        GetFormDefinition(service: any, typeDef: string) {

            let url = "Document";
            if (typeDef == "KnowledgeRegister")
                url = "Knowledge";
            service.GetArray(nodak.enums.ServiceTypeEnum.Get).then(
                (res) => {
                    let tempMenu = new core.SubMenuBase();
                    for (var i = 0; i < res.length; i++) {
                        if (res[i].IsDisabled == false) {
                            if ((typeDef == "KnowledgeRegister" && res[i].FormDefinitionTypeId == 1) /*|| (typeDef == "DocumentRegister" && res[i].FormDefinitionTypeId == 2)*/)
                                tempMenu.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                                    .HasRoot(labxand.components.core.MenuRoot.Register)
                                    .HasId(typeDef + i).HasAuthorizedOperation([]).HasCaption(res[i].Name).HasChild(false).HasParent(typeDef).HasHref("/" + url + "/Index?id=" + res[i].Id);

                            if (typeDef == "KnowledgeRegister" && res[i].FormDefinitionTypeId == 1)
                                tempMenu.AddSubMenuNode().SetSubSystems(labxand.components.core.SubSystems.Common)
                                    .HasRoot(labxand.components.core.MenuRoot.Admin)
                                    .HasId(typeDef + i + i).HasAuthorizedOperation([]).HasCaption(res[i].Name).HasChild(false).HasParent("DirectKnowledgeAdmin").HasHref("/" + url + "/Index?id=" + res[i].Id + "&knowledgeState=1");

                        }
                    }

                    tempMenu.menus.forEach((item) => {
                        this.CreatMenuElement(item);

                    });


                }
            )
        }


        GetDate() {
            let m = new labxand.components.core.DateWithoutTimePersianMonth();
            this.Date = new Date();
            this.Date = m.Set(this.Date.toString());
        }

   

        constructor() {
            //this.CurrentMemberEntity.Picture = new nodak.common.models.AttachmentDescription();
            this.CheckToken();
            this.GetUserInformation();
            let d = new core.SubMenuBase();
            d.CreateRootMenu();

            d.menus.forEach((subMenuItem) => {
                this.CreatMenuElement(subMenuItem);
            });


            this.menuArray = new Array<core.Menu>();

            let commonMenu = new nodak.common.models.commonMenu();
            commonMenu.menus.forEach((item) => {

                this.CreatMenuElement(item);

            });


          
            //  this.GetFormDefinition(this.knowledgeService, "DocumentRegister");
         //   this.GetCurrentMember();
            this.GetDate();
        }
    }
}
angular.module('common.controllers').controller('menu', labxand.components.controller.MenuController);






////module labxand.components.controller {
////    export class MenuController {
////        FullName: string;
////        LogoutCaption = "خروج";
////        menuArray: Array<core.Menu>;
////        CreateElement(element: string, className: string) {
////            let mainElement = document.createElement(element);
////            if (className != null)
////                mainElement.className = className;
////            return mainElement;
////        }
////        CreateBElement() {
////            let mainElement = document.createElement("b");
////            mainElement.className = "arrow";
////            return mainElement;
////        }

////        CreateMenu(menu: core.Menu) {

////            let ul = this.CreateElement("ul", "nav-dropdown-items");
////            let li = this.CreateElement("li", "nav-item");
////            let aElement = this.CreateElement("a", "nav-link nav-dropdown-toggle");
////            aElement.setAttribute("href", menu.href == undefined ? "#" : Base.Config.AppRoot+ menu.href);
////            //  aElement.setAttribute("href", menu.href == undefined ? "#" :Base.Config.AppRoot + menu.href);
////            aElement.setAttribute("class", "nav-link");
////            var newBaitText = document.createTextNode(menu.caption);
////            aElement.appendChild(newBaitText);


////            li.appendChild(aElement);
////            ul.id = menu.Id;
////            ul.appendChild(li);
////            if (document.getElementById(menu.mainParent)) {
////                document.getElementById(menu.mainParent).appendChild(ul);
////            }
////            //let ul = this.CreateElement("ul", "submenu");
////            //ul.id = menu.Id;
////            //li.appendChild(ul);

////            this.CreateNode(menu.Id);

////        }

////        CreateNode(id) {
////            let me = this.menuArray.filter(q => q.ParetnNode == id);
////            me.forEach((item) => {
////                let ul = this.CreateElement("ul", "nav-dropdown-items");
////                let li = this.CreateElement("li", "nav-item");
////                let aElement = this.CreateElement("a", "nav-link nav-dropdown-toggle");
////                aElement.setAttribute("href", item.href);
////                aElement.setAttribute("class", "nav-link");
////                var newBaitText = document.createTextNode(item.caption);
////                aElement.appendChild(newBaitText);


////                li.appendChild(aElement);
////                ul.appendChild(li);

////                ul.id = item.Id;
////                if (document.getElementById(item.ParetnNode)) {
////                    document.getElementById(item.ParetnNode).appendChild(ul);
////                }

////                //if (item.hasChild) {
////                //    let ul = this.CreateElement("ul", "submenu");
////                //    ul.id = item.Id;
////                //    li.appendChild(ul);
////                //    this.CreateNode(item.Id);
////                //}
////                //else {
////                //    li.id = item.Id;
////                //    a.setAttribute('href', item.href);
////                //}
////            })

////        }
////        GetUserInformation() {
////            var obj = JSON.parse(localStorage.getItem("User"));
////            if (obj != null) {
////                this.FullName = obj.FullName;

////            }

////        }

////        getParameterByName(name) {
////            var url = window.location.href;
////            name = name.replace(/[\[\]]/g, "\\$&");
////            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
////                results = regex.exec(url);
////            if (!results) return null;
////            if (!results[2]) return '';
////            return decodeURIComponent(results[2].replace(/\+/g, " "));
////        }

////        CheckToken() {
////            var token = this.getParameterByName('token');
////            var redirectPath = this.getParameterByName('redirectPath');
////            if (token != null) {
////                localStorage.setItem("authenticationToken", token);
////            }
////            if (redirectPath != null)
////                localStorage.setItem("redirectPath", redirectPath);
////            if (localStorage.getItem("redirectPath") != undefined) {
////                this.LogoutCaption =  'پرتال';
////            }
////        }
////        Logout() {
////            if (localStorage.getItem("redirectPath") != undefined) {
////                var temp = localStorage.getItem("redirectPath");
////               // localStorage.clear();
////                window.location.href = temp;
////                return;
////            }
////            var token = localStorage.getItem("authenticationToken");

////            $.ajax({
////                headers: { 'Authorization': 'Bearer ' + token },
////                url: Base.Config.ServiceRoot + "/logout",
////                type: 'POST'
////            }).then(function (response) {
////                localStorage.clear();
////                window.location.href = Base.Config.AppRoot+'/login';

////            }, function (xhr, status, error) {
////                var response = xhr.responseText;
////                alert(JSON.stringify(JSON.parse(response), null, ' '));
////            });
////        }
////        constructor() {
////            this.CheckToken();
////            this.GetUserInformation();
////            let d = new core.SubMenuBase();
////            d.CreateSubMenu();

////            d.menus.forEach((subMenuItem) => {
////                let liMainElement = this.CreateElement('li', 'nav-item nav-dropdown');
////                let aElement = this.CreateElement("a", "nav-link nav-dropdown-toggle");
////                aElement.setAttribute("href", subMenuItem.href);
////                var newBaitText = document.createTextNode(subMenuItem.caption);
////                aElement.appendChild(newBaitText);

////                liMainElement.appendChild(aElement);
////                liMainElement.id = subMenuItem.Id;
////                document.getElementById('testMenu').appendChild(liMainElement);
////            });


////            this.menuArray = new Array<core.Menu>();
////            let operations: Array<string> =JSON.parse(localStorage.getItem("Permissions"));
////            console.log("Permissions", operations);
////            let commonMenu = new nodak.common.models.commonMenu();
////            commonMenu.menus.forEach((item) => {
////                if (operations != null) {
////                    //if (operations.filter(q => q == item.authorizedOperation))
////                    //    if (operations.filter(q => q == item.authorizedOperation).length != 0)
////                    this.menuArray.push(item);
////                }
////            });



////            ////let communicationsMenu = new imas.communications.models.CommunicationsMenu();
////            ////communicationsMenu.menus.forEach((item) => {
////            ////    if (operations != null)
////            ////        if (operations.filter(q => q == item.authorizedOperation))
////            ////            if (operations.filter(q => q == item.authorizedOperation).length != 0)
////            ////                this.menuArray.push(item);
////            ////});

////            if (this.menuArray != null)
////                this.menuArray.filter(q => q.isRootMenu == true).forEach((item) => {
////                    this.CreateMenu(item);
////                });

////        }
////    }
////}
////angular.module('common.controllers').controller('menu', labxand.components.controller.MenuController);