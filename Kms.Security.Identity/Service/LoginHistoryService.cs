using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabXand.DistributedServices;
using LabXand.Core;
using System.Linq.Expressions;
using LabXand.Extensions;
using System.Data.Entity;

namespace Kms.Security.Identity
{
    public class LoginHistoryService : ServiceBase<Guid, LoginHistory, LoginHistoryDto>, ILoginHistoryService
    {
        public LoginHistoryService(IEntityMapper<LoginHistory, LoginHistoryDto> mapper) : base(mapper)
        {
        }

        public override LoginHistoryDto Save(LoginHistoryDto domainDto)
        {
            var domain = _mapper.CreateFrom(domainDto);
            using (var dbContext = new ApplicationDbContext())
            {
                if (domain.IdIsEmpty())
                {
                    domain.SetNewId();
                    domain.SetDate();
                    dbContext.Set<LoginHistory>().Add(domain);
                }
                dbContext.SaveAllChanges();
                return _mapper.MapTo(domain);
            }

        }


        public void Failed(Guid userId, string fingerPrint)
        {
            var history = new LoginHistoryDto();
            history.UserId = userId;
            history.IsSuccess = false;
            history.FingerPrint = fingerPrint;
            Save(history);
        }

        public void Success(Guid userId, string fingerPrint, int expireMinutesToken)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(expireMinutesToken);

            var history = new LoginHistoryDto
            {
                UserId = userId,
                IsSuccess = true,
                FingerPrint = fingerPrint,
                ExpireDateToken = expires
            };
            Save(history);
        }

        public void SetLogout(Guid userId, string fingerprint)
        {
            using (var db = new ApplicationDbContext())
            {
                var domain = db.Set<LoginHistory>().Where(t => t.UserId == userId && t.FingerPrint == fingerprint && t.IsSuccess == true &&
                t.LogOutDate == null && DateTime.UtcNow < t.ExpireDateToken).FirstOrDefault();
                if (domain != null)
                {
                    domain.setLogOutDate(DateTime.UtcNow);
                    db.Set<LoginHistory>().Attach(domain);
                    db.Entry(domain).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }
        public IEnumerable<LoginHistoryDto> GetLoginHistoriesWhichAreNotLogoutByUserId(Guid userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var data = db.Set<LoginHistory>().Where(t => t.UserId == userId && t.IsSuccess == true && /*DateTime.UtcNow < t.ExpireDateToken &&*/ t.LogOutDate == null);
                if (data.Any() == false)
                    return null;
                return data.ToList().Select(t => _mapper.MapTo(t)) ?? null; 
            }
        }

        public new IQueryable<LoginHistoryDto> GetAll()
        {
            using (var db = new ApplicationDbContext())
            {
                var res = db.Set<LoginHistory>().Select(t => _mapper.MapTo(t));
                return res;
            }
        }


        public IQueryable<LoginHistoryDto> GetWithUserId(Guid userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var res = db.Set<LoginHistory>().Where(t => t.UserId == userId).Select(t => _mapper.MapTo(t));
                return res;
            }
        }


        public Paginated<LoginHistory> GetAllHistoryForGrid(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems)
        {
            using (var db = new ApplicationDbContext())
            {

                var query = db.Set<LoginHistory>()/*.Select(t => _mapper.MapTo(t))*/;
                var result = GetOnePageOfList(query.OrderBy(t => t.Date), criteria, pageIndex, pageSize, sortItems);
                return result;
            }

        }

        private Paginated<LoginHistory> GetOnePageOfList(IQueryable<LoginHistory> query, Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems)
        {
            Expression<Func<LoginHistory, bool>> expression = p => true;
            if (criteria != null)
                expression = ExpressionHelper.CreateFromCriteria<LoginHistory>(criteria);
            query = query.Where(expression);

            var temp1 = SortItemHelper.GetQueryable(SortByDate(sortItems), query);
            var temp2 = new PagedList<LoginHistory>(temp1, pageIndex, pageSize);
            PagedList<LoginHistory> pMember = new PagedList<LoginHistory>(temp2.ToList(), pageIndex, pageSize, temp2.TotalCount);
            return new Paginated<LoginHistory>(pMember);
        }

        private List<SortItem> SortByDate(List<SortItem> sortItems)
        {
            if (sortItems.Count == 1 && sortItems[0].SortFiledsSelector == "Id")
            {
                var tmp = new SortItem();
                tmp.SortFiledsSelector = "Date";
                tmp.Direction = SortDirection.Descending;
                sortItems = new List<SortItem>();
                sortItems.Add(tmp);
            }
            else
            {
                var si = new SortItem();
                si.SortFiledsSelector = "Date";
                si.Direction = SortDirection.Descending;
                sortItems.Add(si);
            }
            return sortItems;
        }


    }
}
