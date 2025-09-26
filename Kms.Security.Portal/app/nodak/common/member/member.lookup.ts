module nodak.common.models {
    export class MemberLookup extends labxand.components.core.LookupBase<common.models.Member, common.models.MemberSearchModel>
    {
        constructor(id: string) {
            
            super("ctrl", "common.Member.searchController", "/Template/common/member.selector.html", id, "Member", enums.SubSystems.Common)
        
            this.SetSelectedTextField(() => this.Model.LastName);
            this.SetSelectedIdField(() => this.Model.Id)
            this.staticLoad = true;
        }
    }
}