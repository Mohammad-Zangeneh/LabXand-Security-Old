using System.ComponentModel;

namespace Kms.Security.Util
{
    public enum RequirePasswordChangeStatus
    {
        [Description("عدم نیاز به تغییر")]
        NotRequired = 0,

        [Description("نیاز به تغییر پس از تغییر پسورد توسط ادمین")]
        DueToAdminChange = 1,

        [Description("نیاز به تغییر پس از ثبت نام کاربر")]
        DueToUserRegistration = 2,

        [Description("نیاز به تغییر پس از انقضای ۳۰ روز")]
        DueToExpiration30Days = 3

    }
}
