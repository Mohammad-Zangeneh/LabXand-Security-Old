using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
   public interface ITokenStoreService : IServiceBase<UserToken,UserTokenDto>
    {
        void CreateUserToken(UserToken userToken);
        bool IsValidToken(string accessToken, Guid userId);
        void DeleteExpiredTokens();
        UserToken FindToken(string refreshTokenIdHash);
        void DeleteToken(string refreshTokenIdHash);
        void InvalidateUserTokens(Guid userId);
        void UpdateUserToken(Guid userId, string accessTokenHash);
        bool InvalidateUserToken(string accessToken);
        bool InvalidateUserTokenWithRoleId(Guid roleId);
        Guid FindUserIdFromToken(string acessToken);
        bool InvalidateUserTokensByUserId(Guid userId);
    }
}
