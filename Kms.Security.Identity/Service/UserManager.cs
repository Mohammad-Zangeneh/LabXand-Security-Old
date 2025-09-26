using Kms.Security.Common.Domain;
using Kms.Security.Util;
using LabXand.Infrastructure.Data.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Kms.Security.Identity.Service
{
    public static class UserManager
    {
        private static readonly IRedisCacheService redisCacheService = RedisFactory.CreateInstance();


        public static bool IsSuperAdmin(string userName)
        {
            return GetOnlyUserByUsername(userName).IsSuperAdmin;

        }

        public static ApplicationUser GetUserWithUsername(string userName)
        {
            var applicationUserWithDetail = redisCacheService.GetObject<ApplicationUser>(RedisHelper.BuildKey(RedisKey.ApplicationUserWithDetail, userName), RedisDbType.User);

            if (applicationUserWithDetail is null)
            {
                using (var dbcontext = new ApplicationDbContext())
                {
                    applicationUserWithDetail = dbcontext.Users
                        .Include(p => p.EnterprisePosition)
                        .Include(p => p.EnterprisePositionPosts)
                        .Include(p => p.Organization)
                        .Include(p => p.LabxandRoles)
                        .FirstOrDefault(p => p.UserName == userName);
                    var jsonSetting = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                    redisCacheService.SetObject(RedisHelper.BuildKey(RedisKey.ApplicationUserWithDetail, userName), applicationUserWithDetail, RedisDbType.User, TimeSpan.FromHours(5),jsonSetting);

                }

            }

            return applicationUserWithDetail;

        }

        public static ApplicationUser GetOnlyUserByUsername(string userName)
        {
            var applicationUser = redisCacheService.GetObject<ApplicationUser>(RedisHelper.BuildKey(RedisKey.ApplicationUser, userName), RedisDbType.User);
            if (applicationUser == null)
            {
                using (var dbcontext = new ApplicationDbContext())
                {
                    applicationUser = dbcontext.Users.FirstOrDefault(p => p.UserName == userName);
                    redisCacheService.SetObject(RedisHelper.BuildKey(RedisKey.ApplicationUser, userName), applicationUser, RedisDbType.User, TimeSpan.FromHours(5));
                }
            }

            return applicationUser;
        }

        public static List<Permission> GetUserPermissions(Guid userId)
        {
            var redisCacheService = RedisFactory.CreateInstance();
            var permissions = redisCacheService.GetObject<List<Permission>>(RedisKey.Permission + userId, RedisDbType.Permission);
            if (permissions == null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var user = dbContext.Set<ApplicationUser>()
                        .Where(p => p.Id == userId)
                        .Include(p => p.LabxandRoles)
                        .Include("LabxandRoles.Permissions.Permission")
                        .Include(p => p.EnterprisePositionPosts)
                        .Include("EnterprisePositionPosts.Permissions").FirstOrDefault();
                    permissions = user.LabxandRoles.SelectMany(r => r.Permissions).Select(p => p.Permission).Union(user.EnterprisePositionPosts.SelectMany(ep => ep.Permissions)).ToList();

                    redisCacheService.SetObject(RedisKey.Permission + userId, permissions, RedisDbType.Permission);
                }
            }

            return permissions;
        }

        public static void RemoveRedisObjects(string userName)
        {
            redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.ApplicationUser, userName), RedisDbType.User);
            redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.ApplicationUserWithDetail, userName), RedisDbType.User);
        }
    }
}
