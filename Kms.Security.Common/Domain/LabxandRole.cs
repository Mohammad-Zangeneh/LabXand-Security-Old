using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class LabxandRole : IDomainBase<Guid> , IDomainEntity
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public LabxandRole(Guid id, string name, string title, Guid? companyId)
        {
            Id = id;
            Name = name;
            Title = title;
            this.CompanyId = companyId;
            this.CreateDate = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
        }
        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }
        public void SetCompany(Company company)
        {
            this.Company = company;
        }
        public void SetPermissions(List<PermissionRole> permissions)
        {
            Permissions = permissions;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public void SetTitle(string title)
        {
            Title = title;
        }

        public bool IdIsEmpty()
        {
            return this.Id == Guid.Empty;
        }
        public void SetCompanyId(Guid? companyId)
        {
            this.CompanyId = companyId;
        }
        public LabxandRole() { }

        public IList<PermissionRole> Permissions { get; set; }
        public string Title { get; set; }

        public Guid? CompanyId { get; protected set; }
        public Company Company { get; protected set; }
        public DateTime? CreateDate { get; protected set; }
        public void SetCreateDate(DateTime? createDate)
        {
            this.CreateDate = createDate;
        }
        public DateTime? LastUpdateDate { get; protected set; }

        object IDomainEntity.Id => Id;

        public void SetLastUpdateDate(DateTime? lastUpdateDate)
        {
            this.LastUpdateDate = lastUpdateDate;
        }
        public override string ToString()
        {
            return Title ?? Name;
        }

        public string EntityDescriptor() => $"نقش « {Title} » ";

        public bool IsValid() => true;
    }
}
