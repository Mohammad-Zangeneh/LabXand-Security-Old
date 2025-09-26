using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class PermissionCategory : IDomainBase<Guid>
    {
        public PermissionCategory()
        {
            PermissionType = Util.PermissionType.Normal;
        }
        public PermissionCategory(Guid id,string name,Guid? parentId,Guid? companyId,PermissionType permissionType)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            CompanyId = companyId;
            PermissionType = permissionType;
        }
        public void SetParent(PermissionCategory parent)
        {
            Parent = parent;
        }

        public void SetCompany(Company company)
        {
            Company = company;
        }
        public void SetPermissions(IList<Permission> permissions)
        {
            Permissions = permissions;
        }
        public Guid Id { get; protected set; }
        public bool IdIsEmpty()
        {
            return Id == Guid.Empty;
        }
        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; protected set; }

        public Guid? ParentId { get; protected set; }
        public PermissionCategory Parent { get; protected set; }

        public Guid? CompanyId { get; protected set; }
        public Company Company { get; protected set; }
        public PermissionType PermissionType { get; set; }
        public IList<Permission> Permissions { get; protected set; }
    }
}
