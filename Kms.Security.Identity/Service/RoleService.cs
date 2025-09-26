using System;
using System.Collections.Generic;
using System.Linq;
using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System.Data.Entity;
using LabXand.Infrastructure.Data.EF;

namespace Kms.Security.Identity
{
    public class RoleService : ServiceBase<Guid, LabxandRole, RoleDto>, IRoleService
    {
        ITokenStoreService _tokenService;
        private string _trackEntity;
        public RoleService(IEntityMapper<LabxandRole, RoleDto> mapper
            , ITokenStoreService tokenService
            ) : base(mapper)
        {
            this.HasNavigationProperty(p => p.Permissions);
            this.HasNavigationProperty(a => a.Company);
            _tokenService = tokenService;
        }
        public override RoleDto Save(RoleDto domainDto)
        {
            var role = this._mapper.CreateFrom(domainDto);
            using (var dbContext = new ApplicationDbContext())
            {
                //contain all permission include permission and permission category
                var permissions = role.Permissions.Select(p => p.PermissionId);

                //separate permissions from permission category and select permission
                var selectedPermissions = dbContext.Permissions.Where(p => permissions.Any(x => x.Equals(p.Id))).ToList();
                if (role.Id == Guid.Empty)
                    role.SetNewId();
                else
                {
                    var originalRole = dbContext.LabxandRoles.Include(p => p.Permissions)
                        .FirstOrDefault(p => p.Id == role.Id);
                    if (role.Permissions != null)
                    {
                        foreach (var item in role.Permissions)
                            if (!originalRole.Permissions.Any(t => t.PermissionId == item.PermissionId) && (selectedPermissions.Any(k => k.Id == item.PermissionId)))
                            {
                                originalRole.Permissions.Add(item);
                                item.SetPermission(dbContext.Permissions.Find(item.PermissionId));
                            }
                    }
                    if (originalRole.Permissions != null)
                    {
                        var temp = new List<PermissionRole>(originalRole.Permissions);
                        foreach (var item in temp)
                            if (!role.Permissions.Any(t => t.PermissionId == item.PermissionId))
                            {
                                originalRole.Permissions.Remove(item);
                                item.SetPermission(dbContext.Permissions.Find(item.PermissionId));
                            }
                    }

                    originalRole.SetName(role.Name);
                    originalRole.SetTitle(role.Title);
                    originalRole.SetCompanyId(role.CompanyId);
                    if (originalRole.CreateDate == null)
                        originalRole.SetCreateDate(role.CreateDate);
                    originalRole.SetLastUpdateDate(role.LastUpdateDate);
                    _trackEntity = dbContext.StringifyDbContextChanges();
                    dbContext.SaveAllChanges();
                    _tokenService.InvalidateUserTokenWithRoleId(role.Id);
                    domainDto.Id = role.Id;
                    return domainDto;
                }
                var removeList = new List<PermissionRole>();
                foreach (var item in role.Permissions)
                {
                    if (selectedPermissions.Any(p => p.Id == item.PermissionId))
                        item.SetRoleId(role.Id);
                    else
                        removeList.Add(item); 
                }
                foreach (var item in removeList)
                {
                    role.Permissions.Remove(item);
                    //item.SetPermission(dbContext.Permissions.Find(item.PermissionId));
                }

                dbContext.LabxandRoles.Add(role);
                _trackEntity = dbContext.StringifyDbContextChanges();
                dbContext.SaveAllChanges();
                _tokenService.InvalidateUserTokenWithRoleId(role.Id);
                domainDto.Id = role.Id;
                return domainDto;

            }
        }
        public void Delete(Guid id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var role = dbContext.LabxandRoles.FirstOrDefault(t => t.Id == id);
                if (role == null)
                    throw new Exception("نقش مورد نظر پیدا نشد");
                dbContext.LabxandRoles.Remove(role);
                dbContext.SaveAllChanges();
            }
        }

        public string GetTrackEntity()
        {
            return _trackEntity ?? null;
        }
    }
}
