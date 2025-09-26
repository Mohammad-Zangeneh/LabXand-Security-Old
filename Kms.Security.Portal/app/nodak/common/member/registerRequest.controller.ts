module nodak.common.controllers {
	export class RegisterRequestController extends nodak.core.CrudControllerBase<models.Member, models.MemberDto>{
		organizationLookup: labxand.components.ITreeLookup<models.Organization>;
		enterprisePositionLookup: labxand.components.ITreeLookup<models.EnterprisePosition>;
		public actionPanelRequest: labxand.components.core.IActionPanel;
		static $inject = ["$injector", "$scope", "common.service.member", "common.member.mapper"
			, "common.service.role", "common.service.enterprisePositionPost"];
		constructor(injector, scope, private memberService: services.IMemberService, mapper, roleService,
			enterprisePositionService) {
			super(injector, scope, memberService, mapper, enums.SubSystems.Common, "Member", "ثبت کاربران", false);
			this.RequestPage();

			this.currentEntity = new models.Member();
			this.organizationLookup = new nodak.common.models.OrganizationLookup("Organizationss");
			this.enterprisePositionLookup = new nodak.common.models.EnterprisePositionLookup("EnterprisePositionLookup");
			this.organizationLookup.AfterSelect = (item) => {
				this.enterprisePositionLookup.treeController.treeEntity.Data = item.EnterprisePositions;
				this.enterprisePositionLookup.treeController.treeEntity.Refresh();
				this.enterprisePositionLookup.Clear();
				this.enterprisePositionLookup.treeController.treeEntity.SelectedRow = null;
			}

		}

		RequestPage() {
			this.actionPanelRequest = new labxand.components.core.ActionPanelBase();
			this.actionPanelRequest.panelTitle = "درخواست عضویت";
			this.actionPanelRequest.AddButton("RegisterRequestButton").HasCaption("درخواست عضویت")
				.HasClass(nodak.enums.NodakCss.OKBtn).HasTitle("ذخیره")
				.SetOnClick(() => { this.RegisterRequest(); });
		}

		RegisterRequest() {

			let defer: ng.IDeferred<{}> = this.$q.defer();

			this.BeforSave().then(() => {

				this.memberService.RegisterRequest(this.mapper.MapToDto(this.currentEntity)).then(() => {
					this.messageBox.OkButton("", "اطلاعات با موفقیت ثبت شد");
					this.ResetForm();
				}).catch((error) => {
					this.messageBox.OkButton("خطا", error.data.ExceptionMessage);
				});


			}).catch(() => {
				defer.reject();
			});
			return defer.promise;

		}

		AfterGetDetails() { }
		BeforSave() {

			let defer: ng.IDeferred<{}> = this.$q.defer();
			if (!this.currentEntity.IsValid()) {
				defer.reject();
				return defer.promise;
			}

			if (this.currentEntity.Password != this.currentEntity.ConfirmPassword && (this.currentEntity.Id == null
				|| this.currentEntity.Id == undefined)) {
				this.messageBox.OkButton("خطا", "رمز عبور و تکرار آن برابر نیست");
				defer.reject();
				return defer.promise;
			}

			if (this.currentEntity.UserName == null || this.currentEntity.Email == null ||
				this.currentEntity.Password == null || this.currentEntity.FirstName == null
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
			}
			let organ = this.organizationLookup.treeController.treeEntity.SelectedRow;
			if (organ != null || organ != undefined)
				this.currentEntity.OrganizationId = organ.Id;
			else {
				this.messageBox.OkButton("خطا", " سازمان اجباری است");
				defer.reject();
			}

			defer.resolve();
			return defer.promise;
		}
		AfterSave() {
			this.messageBox.OkButton("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شد");
			this.ResetForm();
		}
		AddBussinessControllerValidation() {
			let defer: ng.IDeferred<{}> = this.$q.defer();
			defer.resolve();
			return defer.promise;

		}

        LoginPage() {
            let url = Base.Config.LoginPage;
            let redirectPath = this.GetParameterByName("redirectPath");
            if (redirectPath != null)
                url += "?redirectPath=" + redirectPath;
            window.location.href = url;
        }
		ResetForm() {
			this.currentEntity = new models.Member();
			this.enterprisePositionLookup.Clear();
			this.organizationLookup.Clear();
		}

	}
}
angular.module("common.controllers").controller("common.registerRequest.controller", nodak.common.controllers.RegisterRequestController);