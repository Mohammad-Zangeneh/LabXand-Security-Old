using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IServiceBase<TDomain, TDomainDto>
    {
        IList<TDomainDto> GetAll();
        TDomainDto Get(Criteria criteria);
        IList<TDomainDto> GetList(Criteria criteria);
        TDomainDto Save(TDomainDto domainDto);
        List<TDomainDto> RangeInsert(IList<TDomainDto> domainDtos);
        Paginated<TDomainDto> GetOnePageOfList(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems);
        void HasNavigationProperty(Expression<Func<TDomain, dynamic>> value);
        void ClearNavigationProperty();
    }
}
