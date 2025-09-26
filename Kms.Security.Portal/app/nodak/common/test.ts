module nodak.common.models {
    export class testt extends labxand.components.core.ComboBase<models.EnterprisePositionPost>{
        constructor(service) {
            super(service);
            this.HasItemSelectedId(() => this.ModelEntity.Id);
            this.HasItemSelectedText(() => this.ModelEntity.Title);
            this.IsMultiSelect = false ;
            this.AllowAdd = true;
            this.IsStatic = false;
           // this.DataUrl = Base.Config.ServiceRoot + "/api/Permission/get";
           this.Data = new Array<models.EnterprisePositionPost>();
           let m = new models.EnterprisePositionPost();
           m.Id = "morsaaa";
            m.Title = "morsa";
            this.Data.push(m);
        }
    }
}