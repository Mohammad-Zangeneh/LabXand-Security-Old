using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity.Service;
using LabXand.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kms.Security.Identity
{
    public class KmsSecurityHelper
    {
        public static KmsUserContext LoginByToken(string token, bool isLoadAccessOperations = false)
        {
            //UserContext userContext = PsoSecurityHelper.LoginByToken(token);
            //KmsUserContext KmsUserContext = IUserContextConvertor.Convert(userContext);
            //if (isLoadAccessOperations)
            //    KmsUserContext = SetAuthorizationData(KmsUserContext, userContext);
            //return KmsUserContext;
            return null;
        }

        public static KmsUserContext AuthenticateUser(string userName, string password)
        {
            //UserContext userContext = PsoSecurityHelper.Login(userName, password);
            //KmsUserContext KmsUserContext = IUserContextConvertor.Convert(userContext);
            //KmsUserContext = SetAuthorizationData(KmsUserContext, userContext);
            //return KmsUserContext;
            return null;
        }

        private static KmsUserContext SetAuthorizationData(KmsUserContext KmsUserContext/*, UserContext userContext*/)
        {
            //var authorizedOperations = PsoSecurityHelper.GetListUserAccessCode(userContext.UserID).ToList();
            //KmsUserContext = IUserContextConvertor.Convert(userContext);
            //if (authorizedOperations != null)
            //{
            //    KmsUserContext.AuthorizedOperations = authorizedOperations.ToList();
            //}
            //return KmsUserContext;
            return null;
        }

        public static KmsUserData GetUserById(Guid userId)
        {
            //var temp = PsoSecurityHelper.GetAllUsers().FirstOrDefault(u => u.ID.Equals(userId));
            //if (temp != null)
            //    return IUserContextConvertor.UserDataConvert(temp);

            return null;
        }
        public static ApplicationUser GetUserByUsername(string username)
        {
            var user = UserManager.GetUserWithUsername(username);

            if (user == null && username.Contains('\\'))
                // format :  Windows\\Username
                user = UserManager.GetUserWithUsername(username.Split('\\')[1]);


            return user ?? null;
        }

        public static List<Permission> GetUserPermissions(Guid userId)
        {
            return UserManager.GetUserPermissions(userId);
        }

        public static List<KmsUserData> GetUsersByIds(List<Guid> userIds)
        {
            return null;
            //    var temp = Kms.Security.EnterpriseModelerUtility.PsoSecurityHelper.GetAllUsers();
            //    List<KmsUserData> usersData = new List<KmsUserData>();
            //    User user = new User();
            //    foreach (var item in userIds)
            //    {
            //        user = temp.FirstOrDefault(p => p.ID.Equals(item));
            //        if (user != null)
            //            usersData.Add(IUserContextConvertor.UserDataConvert(user));
            //    }

            //    return usersData;
        }
    }
}
