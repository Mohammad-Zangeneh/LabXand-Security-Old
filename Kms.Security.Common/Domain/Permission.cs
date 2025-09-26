using Kms.Security.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    [Description("دسترسی")]
    public class Permission : IDomainBase<Guid>
    {
        public Permission()
        {
            PermissionType = PermissionType.Normal;
        }

        public Permission(Guid id, string title, string code, Guid? permissionCategoryId, PermissionType permissionType)
        {
            Id = id;
            Title = title;
            Code = code;
            PermissionCategoryId = permissionCategoryId;
            this.PermissionType = permissionType;
        }

        public void SetPermissionCategoryId(PermissionCategory permissionCategory)
        {
            PermissionCategory = permissionCategory;
        }

        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }

        public bool IdIsEmpty()
        {
            return this.Id == Guid.Empty;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public Guid? PermissionCategoryId { get; set; }
        public PermissionCategory PermissionCategory { get; set; }
        public Company Company { get; set; }
        public Guid? CompanyId { get; set; }
        public PermissionType PermissionType { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }
}
