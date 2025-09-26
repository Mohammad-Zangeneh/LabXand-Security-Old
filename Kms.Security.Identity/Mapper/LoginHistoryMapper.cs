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
    public class LoginHistoryMapper : IEntityMapper<LoginHistory, LoginHistoryDto>
    {
        public LoginHistory CreateFrom(LoginHistoryDto dto)
        {
            var domain = new LoginHistory(dto.Id, dto.UserId, dto.Date, dto.IsSuccess, dto.FingerPrint, dto.ExpireDateToken, dto.LogOutDate);
            return domain;
        }

        public LoginHistoryDto MapTo(LoginHistory domain)
        {
            var dto = new LoginHistoryDto();
            dto.Id = domain.Id;
            dto.UserId = domain.UserId;
            dto.Date = domain.Date;
            dto.IsSuccess = domain.IsSuccess;
            dto.FingerPrint = domain.FingerPrint;
            dto.ExpireDateToken = domain.ExpireDateToken;
            dto.LogOutDate = domain.LogOutDate;
            return dto;
        }
    }
}
