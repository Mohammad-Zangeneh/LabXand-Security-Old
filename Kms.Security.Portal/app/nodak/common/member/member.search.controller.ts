module nodak.common.controllers {
    export class MemberSearchController extends nodak.core.SearchControllerBase<common.models.Member, common.models.MemberSearchModel>
    { 
        //OrganizationLookUp: labxand.components.ITreeLookup<common.models.Organization>;
        showMember = false;
        NewSearch() {
        }
        static $inject = ['$injector', "common.service.member", "common.service.organization"];
       
        constructor(injector: ng.auto.IInjectorService, memberService, OrganizationSrvice) {

            super(injector, "Member", enums.SubSystems.Common);

            this.gridEntity = new common.models.MemberGrid(injector, memberService);
            this.searchEntity = new common.models.MemberSearchModel();  
            //this.OrganizationLookUp = new common.models.OrganizationLookup("Organization");
            //this.Search();
            this.SearchMember();
        }


        Search() {
            
            this.gridEntity.SelectedRow = null;
            this.gridEntity.SelectedId = null;
            this.gridEntity.Search(this.searchEntity);
        }
        SearchMember() {
            
            this.showMember = true;
            //if (this.searchEntity.Organization != null)
            //    this.searchEntity.OrganizationId = this.searchEntity.Organization.Id;
            this.Search();
        }
        DeleteSelected() {
            this.gridEntity.SelectedRow = null;
            this.gridEntity.SelectedId = null;
        }
        
    }
}
angular.module('common.controllers').controller('common.Member.searchController', nodak.common.controllers.MemberSearchController); 