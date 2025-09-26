using RedisTools.RedisCache;

namespace Kms.Security.Identity.Service
{
    public class RedisFactory
    {
        private RedisFactory() {}
        private static IRedisCacheService redisCacheService;
        public static IRedisCacheService CreateInstance()
        {
            if (redisCacheService == null)
                redisCacheService = new RedisCacheService(new RedisStore());

            return redisCacheService;
        }
    }
}
