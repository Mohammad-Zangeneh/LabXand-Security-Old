using Kms.Security.Common.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class CustomUserStore :
        UserStore<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        ICustomUserStore
    {
        private readonly IIdentityUnitOfWork _context;

        public CustomUserStore(IIdentityUnitOfWork context)
            : base((ApplicationDbContext)context)
        {
            _context = context;
        }

        Task<Guid> ICustomUserStore.GetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        Task<Guid> ICustomUserStore.IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}