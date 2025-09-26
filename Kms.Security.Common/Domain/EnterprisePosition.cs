using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
   public class EnterprisePosition:IDomainBase<Guid>
    {
        protected EnterprisePosition()
        {
        }
        public EnterprisePosition(Guid id, String name, Guid? parentId, Guid organizationId,int? sortingNumber)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            OrganizationId = organizationId;
            this.SortingNumber = sortingNumber;
        }
        public void SetNewId()
        {
            this.Id = Guid.NewGuid();
        }

        public bool IdIsEmpty()
        {
            return Id == Guid.Empty;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public EnterprisePosition Parent { get; protected set; }
        public Guid? ParentId { get; protected set; }
        public Guid OrganizationId { get; protected set; }
        public Organization Organization { get; protected set; }

        public int? SortingNumber { get;protected set; }
    }
}
