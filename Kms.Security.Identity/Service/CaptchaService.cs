using Kms.Security.Common.Domain;
using Kms.Security.Util;
using LabXand.Infrastructure.Data.Redis;
using System;

namespace Kms.Security.Identity
{
    public class CaptchaService : ICaptchaService
    {
        private readonly ICaptchaGenerator _captchaGenerator;
        private readonly IRedisCacheService _cacheManager;
        public CaptchaService(ICaptchaGenerator captchaGenerator, IRedisCacheService cacheService)
        {
            _captchaGenerator = captchaGenerator;
            _cacheManager = cacheService;
        }

        public CaptchaDto Get()
        {
            var id = Guid.NewGuid();
            var captcha = _captchaGenerator.Generate();
            var key = RedisKey.Captcha + id.ToString().ToLower();
            _cacheManager.SetObject(key, captcha.Text, RedisDbType.LoginAttempts, TimeSpan.FromHours(1));

            return new CaptchaDto { Id = id, Image = captcha.Image};
        }
    }
}
