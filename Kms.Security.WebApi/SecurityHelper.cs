using Kms.Security.Core;
using Kms.Security.Identity;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;

namespace Kms.Security.WebApi
{
    public class SecurityHelper
    {
        public static KmsUserContext GetUserInfo(string userName)
        {
            //Guid userId = GetFromCache userid from database using given username
            //KmsUserContext KmsUserContext = KmsSecurityHelper.GetUserById(userid);
            KmsUserContext temp = new KmsUserContext
            {
                FirstName = "vabid",
                LastName = "farahmandian",
                IsSuperAdmin = true,
                UserName = "vahid",
                OrganizationTitle = "MECO"
            };
            SetCurrentUserContext(temp);
            return CurrentUserContext;
        }

        public static KmsUserContext AuthenticateUser(string userName, string password)
        {
            KmsUserContext KmsUserContext = KmsSecurityHelper.AuthenticateUser(userName, password);
            SetCurrentUserContext(KmsUserContext);
            return CurrentUserContext;
        }

        public static KmsUserContext LoginByToken(string token, bool isLoadAccessOperations = false)
        {
            KmsUserContext userContext = GetFromCache(token) as KmsUserContext;
            if (userContext == null)
            {
                userContext = KmsSecurityHelper.LoginByToken(token, isLoadAccessOperations);
            }
            SetCurrentUserContext(userContext);
            return CurrentUserContext;
        }

        public static KmsUserContext CurrentUserContext
        {
            get
            {
                return HttpContext.Current.User as KmsUserContext;
            }
        }

        public static void SetCurrentUserContext(KmsUserContext userContext)
        {
            HttpContext.Current.User = (IPrincipal)userContext;
            if (userContext != null && userContext.Token != null)
                AddToCache(userContext.Token, userContext);
        }

        private static void OnRemoveCallback(string key, object value, CacheItemRemovedReason reason)
        {

        }

        private static void AddToCache(string key, object value)
        {
            if (GetFromCache(key) != null)
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddHours(2), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, OnRemoveCallback);
        }

        private static object GetFromCache(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }
    }
}
