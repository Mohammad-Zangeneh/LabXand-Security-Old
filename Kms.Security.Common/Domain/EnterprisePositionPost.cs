using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class EnterprisePositionPost : IDomainBase<Guid>
    {
        public EnterprisePositionPost()
        {

        }
        public EnterprisePositionPost(Guid id, string title,Guid enterprisePositionId, string description)
        {
            Id = id;
            Title = title;
            EnterprisePositionId = enterprisePositionId;
            Description = description;
        }
        public Guid Id { protected set; get; }

        public bool IdIsEmpty()
        {
            return Id == Guid.Empty;
        }

        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }
        public string Title { get;  set; }
        public EnterprisePosition EnterprisePosition { get;  set; }
        public Guid EnterprisePositionId { get;  set; }
        public string Description { get;  set; }

        public List<Permission> Permissions { get;  set; }
        public void SetPermissionList(List<Permission> permissionList)
        {
            this.Permissions = permissionList;
        }
    }
}
