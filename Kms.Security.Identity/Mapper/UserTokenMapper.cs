using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    class UserTokenMapper : IEntityMapper<UserToken, UserTokenDto>
    {
        public UserToken CreateFrom(UserTokenDto destination)
        {
            var userToken = new UserToken();
            userToken.AccessTokenExpirationDateTime = destination.AccessTokenExpirationDateTime;
            userToken.AccessTokenHash = destination.AccessTokenHash;
            userToken.Id = destination.Id;
            userToken.OwnerUserId = destination.OwnerUserId;
            userToken.RefreshToken = destination.RefreshToken;
            userToken.RefreshTokenExpiresUtc = destination.RefreshTokenExpiresUtc;
            userToken.RefreshTokenIdHash = destination.RefreshTokenIdHash;
            userToken.SerialNumber = destination.SerialNumber;
            return userToken;
        }

        public UserTokenDto MapTo(UserToken source)
        {
            var userToken = new UserTokenDto();
            userToken.AccessTokenExpirationDateTime = source.AccessTokenExpirationDateTime;
            userToken.AccessTokenHash = source.AccessTokenHash;
            userToken.Id = source.Id;
            userToken.OwnerUserId = source.OwnerUserId;
            userToken.RefreshToken = source.RefreshToken;
            userToken.RefreshTokenExpiresUtc = source.RefreshTokenExpiresUtc;
            userToken.RefreshTokenIdHash = source.RefreshTokenIdHash;
            userToken.SerialNumber = source.SerialNumber;
            return userToken;
        }
    }
}
