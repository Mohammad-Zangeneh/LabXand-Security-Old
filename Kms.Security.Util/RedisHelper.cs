using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public static class RedisHelper
    {
        /// <summary>
        /// return Complete Key Format Based on Kms Agreement
        /// </summary>
        /// <param name="redisKey">use RedisKey static class to choose title of key</param>
        /// <param name="id">id of the object which need to be cache</param>
        /// <returns></returns>
        public static string BuildKey(string redisKey,Guid id) => 
            new StringBuilder(redisKey).Append(id.ToString()).ToString();

        /// <summary>
        /// return Complete Key Format Based on Kms Agreement
        /// </summary>
        /// <param name="redisKey">use RedisKey static class to choose title of key</param>
        /// <param name="value">unique value of the object which need to be cache</param>
        /// <returns></returns>
        public static string BuildKey(string redisKey,string value) => 
            new StringBuilder(redisKey).Append(value).ToString();

    }
}
