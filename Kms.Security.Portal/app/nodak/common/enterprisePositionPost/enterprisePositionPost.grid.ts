module nodak.common.models {
    export class EnterprisePositionPostGrid extends labxand.components.core.GridBase<EnterprisePositionPost, EnterprisePositionPostSearchModel>{
        constructor(injector: ng.auto.IInjectorService, service) {
            super(injector, service);
            this.HasKey(() => this.Model.Id);
            this.Bound(() => this.Model.Title).HasTitle("عنوان").Width(300);
            this.Bound(() => this.Model.Description).HasTitle("توضیحات").Width(300);
            this.Bound(() => this.Model.EnterprisePosition.Name).HasTitle("چارت سازمانی").Width(300);
            
            this.SetPageSize(10);
        }
    }
}