module nodak.common.models {
    export class RoleGrid extends labxand.components.core.GridBase<Role, RoleSearchModel>{
        //static $inject = ["$injector"];
        constructor(injector: ng.auto.IInjectorService, service) {
            super(injector, service);
            this.HasKey(() => this.Model.Id);
            this.Bound(() => this.Model.Title).HasTitle("عنوان").Width(200);
            this.Bound(() => this.Model.CreateDate).HasFilter(new labxand.components.core.TopersianDateFilter()).HasTitle("تاریخ ایجاد").Width(200);
            this.Bound(() => this.Model.LastUpdateDate).HasFilter(new labxand.components.core.TopersianDateFilter()).HasTitle("تاریخ آخرین تغییر").Width(200);

            this.SetPageSize(10);
        }
    }
}