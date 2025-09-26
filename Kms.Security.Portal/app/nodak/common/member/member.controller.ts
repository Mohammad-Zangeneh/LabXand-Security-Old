module nodak.common.controllers {
    export class MemberController extends nodak.core.CrudControllerBase<models.Member, models.MemberDto>{
        gridEntity: models.MemberGrid;
        memberActionPanel: labxand.components.core.IActionPanel;
        RoleCombo: models.RoleCombo;
        EnterprisePositionPostCombo: models.EnterprisePositionPostCombo;
        searchModel: models.MemberSearchModel;
        organizationLookup: labxand.components.ITreeLookup<models.Organization>;
        enterprisePositionLookup: labxand.components.ITreeLookup<models.EnterprisePosition>;
        EditMode: boolean = false;
        RegisterMode = false;
        modalResetPassword: labxand.components.core.ModalSectionOperation;
        public actionPanelRequest: labxand.components.core.IActionPanel;
        static $inject = ["$injector", "$scope", "common.service.member", "common.member.mapper"
            , "common.service.role", "common.service.enterprisePositionPost"];
        constructor(injector, scope, private memberService: services.IMemberService, mapper, roleService,
            enterprisePositionService) {
            super(injector, scope, memberService, mapper, enums.SubSystems.Common, "Member", "ثبت کاربران", true);
            this.gridEntity = new models.MemberGrid(injector, memberService);
            this.searchModel = new models.MemberSearchModel();
            this.currentEntity = new models.Member();
            if (this.GetParameterByName("status") == "DisableMember") {
                this.searchModel.UserStatus = nodak.Tools.UserStatus.Block;
                this.gridEntity.Search(this.searchModel);
            }
            if (this.GetParameterByName("status") == "RegisterRequest") {
                this.searchModel.UserStatus = nodak.Tools.UserStatus.RegisterRequest;
                this.gridEntity.Search(this.searchModel);
            }
            else if (this.GetParameterByName("status") == "Register") {
                this.RegisterMode = true;
            }
            else {
                this.gridEntity.Search(this.searchModel);
            }


            this.RoleCombo = new models.RoleCombo(roleService);
            this.EnterprisePositionPostCombo = new models.EnterprisePositionPostCombo(enterprisePositionService);
            this.actionPanel.AddButton("resetForm").HasCaption("پاک کردن / انصراف").SetOnClick(() => this.ResetForm()).HasClass(nodak.enums.NodakCss.CancelBtn);

            this.modalResetPassword = new labxand.components.core.ModalSectionOperation("جایگذاری رمز ورود");
            this.modalResetPassword.ConfirmButtonClick = () => { this.ResetPassword(); }

            this.organizationLookup = new nodak.common.models.OrganizationLookup("Organizationss");
            this.enterprisePositionLookup = new nodak.common.models.EnterprisePositionLookup("EnterprisePositionLookup");
            this.organizationLookup.AfterSelect = (item) => {
                this.enterprisePositionLookup.treeController.treeEntity.Data = item.EnterprisePositions;
                this.enterprisePositionLookup.treeController.treeEntity.Refresh();
                this.enterprisePositionLookup.Clear();
                this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = null;
            }
            if (this.GetParameterByName("status") == "RegisterRequest")
                this.MemberActionForRgisterRequest();
            else
                this.MemberAction();

        }

        MemberAction() {
            this.memberActionPanel = new labxand.components.core.ActionPanelBase();
            //this.memberActionPanel.panelTitle = "مدیریت کاربران";
            this.memberActionPanel.AddButton("resetForm").HasCaption("ویرایش").SetOnClick(() => this.Edit()).HasClass(nodak.enums.NodakCss.Update);

            this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("فعال کردن کاربر").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
                .SetOnClick(() => {

                    this.ChangeUserStatus(nodak.Tools.UserStatus.Active);
                });

            if (this.GetParameterByName("status") != "RegisterRequest") {
                this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("غیر فعال کردن ").HasClass(nodak.enums.NodakCss.CancelBtn).HasTitle("")
                    .SetOnClick(() => {

                        this.ChangeUserStatus(nodak.Tools.UserStatus.Block);
                    });
                this.memberActionPanel.AddButton("resetPassword").HasCaption("جایگذاری رمز ورود").SetOnClick(() => this.showResetModal()).HasClass(nodak.enums.NodakCss.Update);

            }

            this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("مشاهده کاربران فعال").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
                .SetOnClick(() => {
                    this.searchModel = new models.MemberSearchModel();
                    this.searchModel.UserStatus = nodak.Tools.UserStatus.Active;
                    this.gridEntity.Search(this.searchModel);
                });

            //this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("مشاهده درخواست های عضویت").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
            //    .SetOnClick(() => { this.searchModel.UserStatus = nodak.Tools.UserStatus.RegisterRequest; this.gridEntity.Search(this.searchModel); });

            this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("مشاهده کاربران غیر فعال").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
                .SetOnClick(() => { this.searchModel.UserStatus = nodak.Tools.UserStatus.Block; this.gridEntity.Search(this.searchModel); });


            let permissions: Array<models.Permission> = JSON.parse(localStorage.getItem("Permissions"));
            let User: models.Member = JSON.parse(localStorage.getItem("User"));
            let hasSuperAdmin = permissions.filter((item) => {
                return item.Code == "SuperAdminManagement"
            });
            if (hasSuperAdmin.length == 0 || User.IsSuperAdmin != true)
                return;
            this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("مشاهده کاربران سوپرادمین").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
                .SetOnClick(() => { this.searchModel.IsAdmin = true; this.gridEntity.Search(this.searchModel); });

            this.memberActionPanel.AddButton("SuperAdminUser").HasCaption("افزودن به سوپرادمین").HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("")
                .SetOnClick(() => {
                    this.ChangeUserToSuperAdmin(true);
                });
            this.memberActionPanel.AddButton("RemoveSuperAdminUser").HasCaption("حذف از سوپرادمین").HasClass(nodak.enums.NodakCss.CancelBtn).HasTitle("")
                .SetOnClick(() => {
                    this.ChangeUserToSuperAdmin(false);
                });
        }
        MemberActionForRgisterRequest() {
            this.memberActionPanel = new labxand.components.core.ActionPanelBase();
            this.memberActionPanel.AddButton("resetForm").HasCaption("ویرایش و تایید").SetOnClick(() => this.Edit(nodak.Tools.UserStatus.Active)).HasClass(nodak.enums.NodakCss.Update);
            this.memberActionPanel.AddButton("MemberAtiveButton").HasCaption("رد درخواست").HasClass(nodak.enums.NodakCss.Delete).HasTitle("")
                .SetOnClick(() => {
                    this.DeleteRegisterRequest();
                });
        }
        DeleteRegisterRequest() {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای رد انتخاب نشده است");
                return;
            }

            if (this.gridEntity.SelectedRow.UserStatus != nodak.Tools.UserStatus.RegisterRequest) {
                this.messageBox.OkButton("خطا", "امکان حذف کاربر مورد نظر وجود ندارد");
                return;
            }
            let temp = angular.copy(this.gridEntity.SelectedRow);
            this.memberService.DeleteRegisterRequest(temp).then(() => {
                this.gridEntity.Search(this.searchModel);
                this.messageBox.OkButton("", "درخواست کاربر مورد نظر رد شد");
            }).catch((error) => {
                this.messageBox.OkButton("", "امکان حذف کاربر مورد نظر وجود ندارد");
                console.log("error in decline register request", error);
            });
        }
        AfterGetDetails() { }
        BeforSave() {

            let defer: ng.IDeferred<{}> = this.$q.defer();
            if (this.currentEntity.Password != this.currentEntity.ConfirmPassword && (this.currentEntity.Id == null || this.currentEntity.Id == undefined)) {
                this.messageBox.OkButton("خطا", "رمز عبور و تکرار آن برابر نیست");
                defer.reject();
                return defer.promise;
            }

            if (this.currentEntity.UserName == null || this.currentEntity.Email == null || (this.currentEntity.Password == null && this.EditMode != true) || this.currentEntity.FirstName == null
                || this.currentEntity.LastName == null
            ) {

                defer.reject();
                this.messageBox.OkButton("خطا", "کلیه اطلاعات اجباری می باشد");
                return defer.promise;
            }

            let v = this.enterprisePositionLookup.treeController.treeEntity.SelectedRow;

            if (v != undefined && v != null) {
                this.currentEntity.EnterprisePositionId = v.Id;
            }
            else {
                this.messageBox.OkButton("خطا", " چارت سازمانی اجباری است");
                defer.reject();
                return defer.promise;
            }
            let organ = this.organizationLookup.treeController.treeEntity.SelectedRow;
            if (organ != null || organ != undefined)
                this.currentEntity.OrganizationId = organ.Id;
            else {
                this.messageBox.OkButton("خطا", " سازمان اجباری است");
                defer.reject();
                return defer.promise;
            }
            this.currentEntity.Roles = this.RoleCombo.SelectedItems;
            this.currentEntity.EnterprisePositionPosts = this.EnterprisePositionPostCombo.SelectedItems;
            if (this.RoleCombo.SelectedItems == null || this.RoleCombo.SelectedItems.length == 0) {
                //this.messageBox.YesNoButton("اخطار", "نقشی به کاربر اختصاص نداده اید، آیا اطلاعات ذخیره شود؟").then(() => {
                //    debugger;
                //    defer.resolve();
                //    return defer.promise;
                //}).catch(() => {
                //    defer.reject();
                //    return defer.promise;
                //});
                //var r = confirm("نقشی به کاربر اختصاص نداده اید، آیا اطلاعات ذخیره شود؟");
                defer.reject();
                toastr.error("یک نقش برای کاربر تعیین نمایید");
                return defer.promise;

            } else {
                defer.resolve();
                return defer.promise;
            }

        }
        AfterSave() {
            toastr.success("اطلاعات با موفقیت ثبت شد");
            this.gridEntity.Search(this.searchModel);
            this.ResetForm();
        }
        AddBussinessControllerValidation() {
            let defer: ng.IDeferred<{}> = this.$q.defer();
            defer.resolve();
            return defer.promise;

        }
        Return() {
            this.EditMode = false;
            this.RegisterMode = false;
        }
        Edit(userStatus: nodak.Tools.UserStatus = null) {
            if (this.gridEntity.SelectedRow == null) {
                toastr.error("موردی برای ویرایش انتخاب نشده است");
                return;
            }
            this.EditMode = true;
            this.RegisterMode = true;
            let temp = angular.copy(this.gridEntity.SelectedRow);
            this.currentEntity = this.mapper.MapToEntity(temp);
            if (userStatus != null)
                this.currentEntity.UserStatus = userStatus;
            this.EnterprisePositionPostCombo.SelectedItems = this.currentEntity.EnterprisePositionPosts;
            this.RoleCombo.SelectedItems = this.currentEntity.Roles;
            this.organizationLookup.treeController.treeEntity.SelectedRow = this.organizationLookup.treeController.treeEntity.SearchById(this.currentEntity.OrganizationId);
            if (this.currentEntity.OrganizationId != null)
                this.organizationLookup.OnSelected();
            setTimeout(() => {
                if (this.currentEntity.EnterprisePosition != null) {
                    this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = this.enterprisePositionLookup.treeController.treeEntity.SearchById(this.currentEntity.EnterprisePositionId);
                    this.enterprisePositionLookup.OnSelected();
                }
                else {
                    this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = null;
                    this.enterprisePositionLookup.OnSelected();
                }

            }, 100);

            //if (this.currentEntity.EnterprisePosition != null) {
            //    this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = this.enterprisePositionLookup.treeController.treeEntity.SearchById(this.currentEntity.EnterprisePositionId);
            //    this.enterprisePositionLookup.OnSelected();
            //}
            //else {
            //    this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = null;
            //    this.enterprisePositionLookup.OnSelected();
            // }

        }
        ChangeUserToSuperAdmin(isAdmin) {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ویرایش انتخاب نشده است");
                return;
            }
            //let status = nodak.Tools.UserStatus.Active;
            //if (isAdmin)
            //    status = nodak.Tools.UserStatus.SuperAdmin;
            if (this.gridEntity.SelectedRow.IsSuperAdmin == isAdmin) {
                this.messageBox.OkButton("خطا", "کاربر مورد نظر در حالت انتخاب شده است  ");
                return;
            }
            let temp = angular.copy(this.gridEntity.SelectedRow);
            temp.IsAdmin = isAdmin;

            if (isAdmin)
                this.memberService.ChangeToSuperAdmin(temp).then(() => {
                    this.gridEntity.Search(this.searchModel);
                });
            else
                this.memberService.RemoveFromSuperAdmin(temp).then(() => {
                    this.gridEntity.Search(this.searchModel);
                }).catch((ex) => {
                    toastr.error(ex.data.ExceptionMessage);
                    //console.log("ex", ex);
                });
        }

        ChangeUserStatus(status) {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ویرایش انتخاب نشده است");
                return;
            }

            if (this.gridEntity.SelectedRow.UserStatus == status) {
                this.messageBox.OkButton("خطا", "کاربر مورد نظر در حالت انتخاب شده است  ");
                return;
            }
            let temp = angular.copy(this.gridEntity.SelectedRow);

            temp.UserStatus = status;
            this.memberService.ChangeStatus(temp).then(() => {
                this.gridEntity.Search(this.searchModel);
            });
        }

        ResetForm() {
            this.EditMode = false;
            if (this.GetParameterByName("status") == "Register")
                this.RegisterMode = true;
            else
                this.RegisterMode = false;
            this.currentEntity = new models.Member();
            this.EnterprisePositionPostCombo.Clear();
            this.RoleCombo.Clear();
            this.gridEntity.ClearSelected();
            this.gridEntity.SelectedRow = null;
            this.enterprisePositionLookup.Clear();
            this.organizationLookup.Clear();
        }
        showResetModal() {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ریست کردن پسورد انتخاب نشده است");
                return;
            }
            let temp = angular.copy(this.gridEntity.SelectedRow);
            this.currentEntity = this.mapper.MapToEntity(temp);
            this.modalResetPassword.ShowModal();
        }
        ResetPassword() {
            if (this.gridEntity.SelectedRow == null) {
                this.messageBox.OkButton("خطا", "موردی برای ریست کردن پسورد انتخاب نشده است");
                return;
            }
            let temp = new models.MemberDto();
            temp.Id = this.gridEntity.SelectedRow.Id;
            temp.Password = this.currentEntity.Password;
            this.memberService.ResetPassword(temp).then((res) => {
                this.messageBox.OkButton("", "اطلاعات با موفقیت ثبت شد").then(() => { this.modalResetPassword.CloseModal(); });
                this.ResetForm();
            }).catch((error) => {
                //console.log("ERRR", error);
                this.messageBox.OkButton("خطا", error.data.ExceptionMessage);
            });
        }
        Search() {
            this.gridEntity.Search(this.searchModel);
        }

    }
}
angular.module("common.controllers").controller("common.member.controller", nodak.common.controllers.MemberController);