using System.ComponentModel;

namespace Kms.Security.Util
{
    public enum UserStatus
    {
        [Description("درخواست عضویت")]
        RegisterRequest = 1,
        [Description("فعال")]
        Active = 2,
        [Description("قفل شده")]
        Block = 3,
        [Description("غیر فعال")]
        Disable = 4,
        [Description("رد درخواست عضویت")]
        DeclineRequest = 5

    }
}
