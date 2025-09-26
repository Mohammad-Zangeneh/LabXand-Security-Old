using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class TokenStoreService : ServiceBase<Guid, UserToken, UserTokenDto>, ITokenStoreService
    {

        private readonly ISecurityService _securityService;
        private readonly IIdentityUnitOfWork _uow;
        private readonly IKmsApplicationUserManager _userManager;

        public TokenStoreService(ISecurityService securityService, IEntityMapper<UserToken, UserTokenDto> mapper
                                , IIdentityUnitOfWork uow
                                , IKmsApplicationUserManager userManager
                                 ) : base(mapper)
        {
            _securityService = securityService;
            _uow = uow;
            _userManager = userManager;
        }
        public void CreateUserToken(UserToken userToken)
        {
            this.Save(_mapper.MapTo(userToken));
        }
        public void UpdateUserToken(Guid userId, string accessTokenHash)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var token = dbContext.UserTokens.FirstOrDefault(x => x.OwnerUserId == userId && x.AccessTokenHash == null);
                if (token != null)
                    token.AccessTokenHash = accessTokenHash;
                dbContext.SaveAllChanges();
            }
        }

        public void DeleteExpiredTokens()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var now = DateTime.UtcNow;
                var userTokens = dbContext.UserTokens.Where(x => x.RefreshTokenExpiresUtc < now).ToList();
                foreach (var userToken in userTokens)
                {
                    dbContext.UserTokens.Remove(userToken);
                }
                dbContext.SaveAllChanges();
            }
        }

        public void DeleteToken(string refreshTokenIdHash)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var token = FindToken(refreshTokenIdHash);
                if (token != null)
                    dbContext.UserTokens.Remove(token);
                dbContext.SaveAllChanges();
            }
        }
        public UserToken FindToken(string refreshTokenIdHash)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                return dbContext.UserTokens.FirstOrDefault(x => x.RefreshTokenIdHash == refreshTokenIdHash);
            }
        }

        public Guid FindUserIdFromToken(string acessToken)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                return dbContext.UserTokens.FirstOrDefault(x => x.AccessTokenHash == acessToken).OwnerUserId;
            }
        }
        public void InvalidateUserTokens(Guid userId)
        {
            var user = _uow.Set<ApplicationUser>().FirstOrDefault(p => p.Id == userId);
            user.SerialNumber = Guid.NewGuid().ToString();
            _uow.SaveAllChanges();
        }
        public bool IsValidToken(string accessToken, Guid userId)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            using (var dbContext = new ApplicationDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(p => p.Id == userId);
                if (user == null)
                    return false;
                var sn = user.SerialNumber;
                var userToken = dbContext.UserTokens.FirstOrDefault(x => x.AccessTokenHash == accessTokenHash && x.OwnerUserId == userId && x.SerialNumber == sn);
                return userToken?.AccessTokenExpirationDateTime >= DateTime.UtcNow;
            }
        }
        public bool InvalidateUserToken(string accessToken)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            using (var dbContext = new ApplicationDbContext())
            {
                var userToken = dbContext.UserTokens.FirstOrDefault(x => x.AccessTokenHash == accessTokenHash);
                if (userToken == null)
                    return true;
                dbContext.UserTokens.Remove(userToken);
                dbContext.SaveAllChanges();
                return true;
            }
        }
        public bool InvalidateUserTokensByUserId(Guid userId)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var userTokens = dbContext.UserTokens.Where(x => x.OwnerUserId == userId);
                if (userTokens == null)
                    return false;

                dbContext.UserTokens.RemoveRange(userTokens);
                dbContext.SaveAllChanges();
                return true;
            }
        }
        public bool InvalidateUserTokenWithRoleId(Guid roleId)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var sql = @"
                UPDATE users 
                SET SerialNumber = NEWID()
                 WHERE id IN 
                (SELECT UserId FROM dbo.UserRoleTable where LabXandRoleId= @roleId) 
";
                dbContext.Database.ExecuteSqlCommand(sql, new SqlParameter("@roleId", roleId));
            }

            //var users = _userManager.GetUser().Where(x => x.LabxandRoles.Any(t => t.Id == roleId));
            //if(users != null)
            //{
            //    foreach (var item in users)
            //    {
            //        _uow.Set<ApplicationUser>().Attach(item);
            //        item.SerialNumber = Guid.NewGuid().ToString();
            //    }
            //}
            //_uow.SaveAllChanges();
            return true;
        }
    }
}
