using Kms.Security.Common.DataContract;

namespace Kms.Security.Identity.Service.Contracts
{
    public interface IAuthService
    {
        KeyDto GenerateKeys();
    }
}
