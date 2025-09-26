module nodak.common.models {
    export class CompanyGrid extends labxand.components.core.GridBase<Company, CompanySearchModel>{
        //static $inject = ["$injector"];
        constructor(injector: ng.auto.IInjectorService, service) {
            super(injector, service);
            this.HasKey(() => this.Model.Id);
            this.Bound(() => this.Model.Name).HasTitle("نام").Width(100);
            this.SetPageSize(10);
        }
    }
}