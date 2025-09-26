using Kms.Security.Common.Domain;
using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kms.Security.Identity
{
    public interface ILoginHistoryService : IServiceBase<LoginHistory, LoginHistoryDto>
    {
        void Failed(Guid userId, string fingerPrint);
        void Success(Guid userId, string fingerPrint, int expireMinutesToken);
        IQueryable<LoginHistoryDto> GetWithUserId(Guid userId);
        Paginated<LoginHistory> GetAllHistoryForGrid(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems);
        void SetLogout(Guid userId, string fingerprint);
        IEnumerable<LoginHistoryDto> GetLoginHistoriesWhichAreNotLogoutByUserId(Guid userId);
    }
}
