module nodak.common.models {
    export class MemberGrid extends labxand.components.core.GridBase<Member, MemberSearchModel>{
        //static $inject = ["$injector"];
        constructor(injector: ng.auto.IInjectorService, service) {
            super(injector, service);
            this.HasKey(() => this.Model.Id);
            this.Bound(() => this.Model.FirstName).HasTitle("نام").Width(100);
            this.Bound(() => this.Model.LastName).HasTitle("نام خانوادگی").Width(150);
            this.Bound(() => this.Model.UserName).HasTitle("نام کاربری").Width(150);
            this.Bound(() => this.Model.PersonnelNumber).HasTitle("شماره پرسنلی").Width(100);
            this.Bound(() => this.Model.Email).HasTitle("ایمیل").Width(150);
            this.Bound(() => this.Model.UserStatusValue).HasTitle("وضعیت").Width(80);
            this.Bound(() => this.Model.RegisterationDate).HasTitle("تاریخ عضویت").HasFilter(new labxand.components.core.DatePersian());
            this.Bound(() => this.Model.Organization.Name).HasTitle("سازمان مربوطه").Width(100);
            this.Bound(() => this.Model.EnterprisePosition.Name).HasTitle("چارت سازمانی").Width(100);
            this.Bound(() => this.Model.Roles).HasTitle("نقش").Width(100).HasFilter(new labxand.components.core.ListFilter("Title",3)); 
            this.SearchConfiguration.Bound(() => this.Model.UserStatus)
                .IsEqual(() => this.SearchModel.UserStatus);
            this.SearchConfiguration.Bound(() => this.Model.IsSuperAdmin)
                .IsEqual(() => this.SearchModel.IsAdmin);
            this.SearchConfiguration.Bound(() => this.Model.FirstName).IsLike(() => this.SearchModel.FirstName);
            this.SearchConfiguration.Bound(() => this.Model.LastName).IsLike(() => this.SearchModel.LastName);
            this.SearchConfiguration.Bound(() => this.Model.UserName).IsLike(() => this.SearchModel.UserName);
            this.SearchConfiguration.Bound(() => this.Model.Email).IsLike(() => this.SearchModel.Email);
            this.SearchConfiguration.Bound(() => this.Model.PersonnelNumber).IsLike(() => this.SearchModel.PersonnelNumber);

            this.SetPageSize(12);
        }
    }
}