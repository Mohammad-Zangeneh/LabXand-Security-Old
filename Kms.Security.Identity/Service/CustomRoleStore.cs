using Kms.Security.Common.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class CustomRoleStore :
        RoleStore<CustomRole, Guid, CustomUserRole>,
        ICustomRoleStore
    {
        private readonly IIdentityUnitOfWork _context;

        public CustomRoleStore(IIdentityUnitOfWork context)
            : base((DbContext)context)
        {
            _context = context;
        }

        public RoleDto Save(RoleDto domainDto)
        {
            throw new NotImplementedException();
        }
    }
}