using Kms.Security.Common.DataContract;
using Kms.Security.Identity.Service.Contracts;
using Kms.Security.Util;
using LabXand.Infrastructure.Data.Redis;
using Org.BouncyCastle.Crypto;
using System;

namespace Kms.Security.Identity.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRedisCacheService _redisCacheService;

        public AuthService(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }
        public KeyDto GenerateKeys()
        {
            var generatedKeys = RSAHelper.GenerateKeys();
            _redisCacheService.SetObject(RedisHelper.BuildKey(RedisKey.PrivateKey, generatedKeys.KeyId),
                generatedKeys.PrivateKeyString, RedisDbType.User, new TimeSpan(0, 15, 0));

            return new KeyDto
            {
                KeyId = generatedKeys.KeyId,
                PublicKey = generatedKeys.PublicKeyString
            };

        }
    }
}
