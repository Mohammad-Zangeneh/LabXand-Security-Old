using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface ICustomRoleStore : IDisposable
    {
        Task<CustomRole> FindByIdAsync(Guid roleId);
        Task<CustomRole> FindByNameAsync(string roleName);
        Task CreateAsync(CustomRole role);
        Task DeleteAsync(CustomRole role);
        Task UpdateAsync(CustomRole role);
        DbContext Context { get; }
        bool DisposeContext { get; set; }
        IQueryable<CustomRole> Roles { get; }
    }
}