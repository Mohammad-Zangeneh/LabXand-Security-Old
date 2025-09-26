using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
   public interface IDomainBase<TIdentityfier>
    {
         TIdentityfier Id { get;  }
        void SetNewId();
        bool IdIsEmpty();
    }
}
