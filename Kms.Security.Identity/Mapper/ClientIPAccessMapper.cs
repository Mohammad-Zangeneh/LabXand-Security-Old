using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using LabXand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class ClientIPAccessMapper : IEntityMapper<ClientIPAccess, ClientIPAccessDto>
    {
        public ClientIPAccess CreateFrom(ClientIPAccessDto dto)
        {
            var domain = new ClientIPAccess(dto.Id, dto.IpAddress, dto.Port, dto.IsWhiteList);
            return domain;
        }

        public ClientIPAccessDto MapTo(ClientIPAccess domain)
        {
            var dto = new ClientIPAccessDto();
            dto.Id = domain.Id;
            dto.IpAddress = domain.IpAddress;
            dto.Port = domain.Port;
            dto.IsWhiteList = domain.IsWhiteList;

            return dto;
        }
    }
}
