
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Util;
using LabXand.Security.Core;
using Microsoft.Owin.Security;
using System.Web;

namespace Kms.Security.WebApi
{
    public class KmsApiUserContextDetector : IUserContextDetector<KmsUserContext>
    {
        public IAuthenticationManager _authenticationManager;
        private KmsUserContext kmsUserContext;

        public KmsUserContext UserContext
        {
            get
            {
                if (DetectHumanAuthorize())
                {
                    if (_authenticationManager != null && _authenticationManager.User != null && _authenticationManager.User.Claims != null)
                        return KmsUserContextConvertor.Convert(_authenticationManager.User.Claims);

                }
                else
                {
                    if (kmsUserContext is null)
                        kmsUserContext = KmsUserContextConvertor.Convert(GetUsername());
                    return kmsUserContext;
                }
                return null;

            }
        }
        bool DetectHumanAuthorize()
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Request.Headers["Username"] is null;
            return true;
        }

        private string GetUsername()
        {
            //string username;

            //if (AuthenticationHelper.GetAuthenticationMode() != AuthenticationMode.Windows)
                //username = HttpContext.Current.Request.Headers["Username"];
            //else
            //{
            //    username = HttpContext.Current.User.Identity.Name;
            //    if (username.Contains("\\"))
            //        username = username.Split('\\')[1];
            //}

            return HttpContext.Current.Request.Headers["Username"];
        }
        public KmsApiUserContextDetector(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
    }
}
