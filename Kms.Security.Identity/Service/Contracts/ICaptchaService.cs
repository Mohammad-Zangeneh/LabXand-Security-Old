using Kms.Security.Common.Domain;

namespace Kms.Security.Identity
{
    public interface ICaptchaService
    {
        CaptchaDto Get();
    }
}