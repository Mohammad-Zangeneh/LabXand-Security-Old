using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class ClientIPAccess : IDomainBase<Guid>
    {
        public ClientIPAccess(Guid id, string ipAddress, string port, bool isWhiteList)
        {
            this.Id = id;
            this.IpAddress = ipAddress;
            this.Port = port;
            this.IsWhiteList = isWhiteList;
        }

        protected ClientIPAccess() { }
        public Guid Id { get; protected set; }
        public string IpAddress { get; protected set; }
        public string Port { get; protected set; }
        public bool IsWhiteList { get; protected set; }


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
    }
}
