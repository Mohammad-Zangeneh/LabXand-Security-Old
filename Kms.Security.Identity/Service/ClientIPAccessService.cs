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
    public class ClientIPAccessService : ServiceBase<Guid, ClientIPAccess, ClientIPAccessDto>, IClientIPAccessService
    {
        public ClientIPAccessService(IEntityMapper<ClientIPAccess, ClientIPAccessDto> mapper) : base(mapper)
        {

        }

        public override ClientIPAccessDto Save(ClientIPAccessDto domainDto)
        {
            var domain = _mapper.CreateFrom(domainDto);
            using (var dbContext = new ApplicationDbContext())
            {
                if (domain.IdIsEmpty())
                {
                    domain.SetNewId();
                    dbContext.Set<ClientIPAccess>().Add(domain);
                }
                dbContext.SaveAllChanges();
                return _mapper.MapTo(domain);
            }
        }

        public new IQueryable<ClientIPAccess> GetAll()
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Set<ClientIPAccess>();
                return query;
            }

        }
        public ClientIPAccess GetWithIP(string ipAddress)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Set<ClientIPAccess>().FirstOrDefault(t => t.IpAddress == ipAddress && t.IsWhiteList == false);
                return query;
            }
        }

        public Paginated<ClientIPAccess> GetAllForGrid(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Set<ClientIPAccess>();
                var result = GetOnePageOfList(query, criteria, pageIndex, pageSize, sortItems);
                return result;
            }
        }

        private Paginated<ClientIPAccess> GetOnePageOfList(IQueryable<ClientIPAccess> query, Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems)
        {
            Expression<Func<ClientIPAccess, bool>> expression = p => true;
            if (criteria != null)
                expression = ExpressionHelper.CreateFromCriteria<ClientIPAccess>(criteria);
            query = query.Where(expression);

            var temp1 = SortItemHelper.GetQueryable(sortItems, query);
            var temp2 = new PagedList<ClientIPAccess>(temp1, pageIndex, pageSize);
            PagedList<ClientIPAccess> pMember = new PagedList<ClientIPAccess>(temp2.ToList(), pageIndex, pageSize, temp2.TotalCount);
            return new Paginated<ClientIPAccess>(pMember);
        }

        public ClientIPAccessDto Delete(ClientIPAccessDto clientIPAccessDto)
        {
            var domain = _mapper.CreateFrom(clientIPAccessDto);
            using (var dbContext = new ApplicationDbContext())
            {
                var clientIPAccess = dbContext.Set<ClientIPAccess>().FirstOrDefault(x => x.Id == domain.Id);
                if (clientIPAccess != null)
                {
                    dbContext.Set<ClientIPAccess>().Remove(clientIPAccess);
                    dbContext.SaveAllChanges();
                    return _mapper.MapTo(clientIPAccess);
                }
                return null;
            }
        }
    }
}
