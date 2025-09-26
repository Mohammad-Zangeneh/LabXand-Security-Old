using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
   public class Organization: IDomainBase<Guid>
    {
        public Organization(Guid id, string name, Guid? parentId/*,string code,string address, string website,string phone*/, int sortingNumber)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            SortingNumber = sortingNumber;
            //Code = code;
            //Address = address;
            //Website = website;
            //Phone = phone;
        }
        protected Organization()
        {

        }

        public void SetNewId()
        {
            this.Id = Guid.NewGuid();
        }
        public bool IdIsEmpty()
        {
            return this.Id == Guid.Empty;
        }
        public void SetEnterprisePositions(IList<EnterprisePosition> enterprisePosins)
        {
            EnterprisePositions = enterprisePosins;
        }
        public IList<EnterprisePosition> EnterprisePositions { get; protected set; }
        public void AddParent(Organization parent)
        {
            this.Parent = parent;
        }
        public string Name { protected set; get; }

        public virtual Organization Parent { protected set; get; }

        public Guid? ParentId { protected set; get; }
        public Guid Id { get; protected set; }
        public int SortingNumber { get; set; }
        //public string Phone { get; protected set; }


        //public string Website { get; protected set; }

        //public string Address { get; protected set; }

        //public string Code { get; protected set; }

    }
}
