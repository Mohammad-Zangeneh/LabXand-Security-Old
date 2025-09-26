using Kms.Security.Common.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class ApplicationRoleManager :
        RoleManager<CustomRole, Guid>,
        IApplicationRoleManager
    {
        private readonly IIdentityUnitOfWork _uow;
        private readonly ICustomRoleStore _roleStore;
        private readonly IDbSet<ApplicationUser> _users;
        public ApplicationRoleManager(
            IIdentityUnitOfWork uow,
            ICustomRoleStore roleStore)
            : base((IRoleStore<CustomRole, Guid>)roleStore)
        {
            _uow = uow;
            _roleStore = roleStore;
            _users = _uow.Set<ApplicationUser>();
        }

        public CustomRole FindRoleByName(string roleName)
        {
            return this.FindByName(roleName); // RoleManagerExtensions
        }

        public IdentityResult CreateRole(CustomRole role)
        {
            return this.Create(role); // RoleManagerExtensions
        }

        public IList<CustomUserRole> GetCustomUsersInRole(string roleName)
        {
            return this.Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
            // = this.FindByName(roleName).Users
        }

        public IList<ApplicationUser> GetApplicationUsersInRole(string roleName)
        {
            var roleUserIdsQuery = from role in this.Roles
                                   where role.Name == roleName
                                   from user in role.Users
                                   select user.UserId;
            return _users.Where(applicationUser => roleUserIdsQuery.Contains(applicationUser.Id))
                         .ToList();
        }

        public IList<CustomRole> FindUserRoles(Guid userId)
        {
            var userRolesQuery = from role in this.Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }

        public string[] GetRolesForUser(Guid userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new string[] { };
            }

            return roles.Select(x => x.Name).ToArray();
        }

        public bool IsUserInRole( Guid userId, string roleName)
        {
            var userRolesQuery = from role in this.Roles
                                 where role.Name == roleName
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }

        public Task<List<CustomRole>> GetAllCustomRolesAsync()
        {
            return this.Roles.ToListAsync();
        }
    }
}