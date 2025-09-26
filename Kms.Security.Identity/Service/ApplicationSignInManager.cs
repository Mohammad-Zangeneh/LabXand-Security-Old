using Kms.Security.Common.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, Guid>,
        IApplicationSignInManager
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationSignInManager(
            IApplicationUserManager userManager,
            IAuthenticationManager authenticationManager) :
            base((ApplicationUserManager)userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            var result = _userManager.GenerateUserIdentityAsync(user);

            var temp = Kms.Security.Identity.Service.UserManager.GetUserPermissions(user.Id);
            foreach (var item in temp)
            {
                result.Result.AddClaim(new Claim(item.Code, item.CompanyId.ToString()));
            }
            return result;
        }

        /// <summary>
        /// How to refresh authentication cookies
        /// </summary>
        public async Task RefreshSignInAsync(ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            // await _userManager.UpdateSecurityStampAsync(user.Id).ConfigureAwait(false); // = used for SignOutEverywhere functionality
            var claimsIdentity = await _userManager.GenerateUserIdentityAsync(user).ConfigureAwait(false);
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, claimsIdentity);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            // var user = _userManager.GetCurrentKmsUser();

            //  user.Claims.Add(new CustomUserClaim { ClaimType = "morsa", ClaimValue = "123" });


            return result;
        }

    }
}