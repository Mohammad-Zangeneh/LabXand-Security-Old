using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class Company : IDomainBase<Guid>
    {
        public Guid Id { get; protected set; }
        public Company()
        {

        }
        public Company(string name,Guid id)
        {
            this.Name = name;
            this.Id = id;
        }
        public bool IdIsEmpty()
        {
            if (this.Id == Guid.Empty)
                return true;
            return false;
        }

        public void SetNewId()
        {
            this.Id = Guid.NewGuid();
        }
        public string Name { get; protected set; }

    }
}
