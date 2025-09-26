using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using LabXand.Core;
using LabXand.Extensions;

namespace Kms.Security.Identity
{
    public class ServiceBase<TIdentityFier, TDomain, TDomainDto> : IServiceBase<TDomain, TDomainDto>
        where TDomain : class, IDomainBase<TIdentityFier>
    {
        public readonly IEntityMapper<TDomain, TDomainDto> _mapper;
        List<Expression<Func<TDomain, dynamic>>> navigationProperty;
        List<Expression<Func<TDomain, bool>>> Restriction;
        public ServiceBase(IEntityMapper<TDomain, TDomainDto> mapper)
        {
            _mapper = mapper;
            navigationProperty = new List<Expression<Func<TDomain, dynamic>>>();
            Restriction = new List<Expression<Func<TDomain, bool>>>();
        }
        public void HasNavigationProperty(Expression<Func<TDomain, dynamic>> value)
        {
            navigationProperty.Add(value);
        }
        public void ClearNavigationProperty()
        {
            navigationProperty.Clear();
        }
        public void HasRestriction(Expression<Func<TDomain, bool>> value)
        {
            Restriction.Add(value);
        }
        public void ClearRestriction()
        {
            Restriction.Clear();
        }

        public virtual IList<TDomainDto> GetAll()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                IQueryable<TDomain> dbQuery = null;
                IDbSet<TDomain> dbSet = dbContext.Set<TDomain>();
                foreach (var include in navigationProperty)
                    dbQuery = dbSet.Include(include);

                if (dbQuery != null)
                {
                    foreach (var restric in Restriction)
                        dbQuery = dbQuery.Where(restric);

                    var r = dbQuery.ToList();
                    return r.Select(p => _mapper.MapTo(p)).ToList();

                }

                foreach (var restric in Restriction)
                    dbQuery = dbSet.Where(restric);
                if (dbQuery == null)
                {
                    var result2 = dbSet.Where(p => true).ToList();
                    return result2.Select(p => _mapper.MapTo(p)).ToList();
                }
                var r2 = dbQuery.ToList();
                return r2.Select(p => _mapper.MapTo(p)).ToList();
            }

        }
        public virtual Paginated<TDomainDto> GetOnePageOfList(Criteria criteria, int pageIndex, int pageSize, List<SortItem> sortItems)
        {

            Expression<Func<TDomain, bool>> expression = p => true;

            if (criteria != null)
            {
                expression = ExpressionHelper.CreateFromCriteria<TDomain>(criteria);
            }
            using (var db = new ApplicationDbContext())
            {
                IQueryable<TDomain> dbQuery = null;
                var result = db.Set<TDomain>().Where(expression);
                foreach (var item in navigationProperty)
                {
                    if (dbQuery == null)
                        dbQuery = result.Include(item);
                    else
                        dbQuery = dbQuery.Include(item);
                }
                if (dbQuery == null)
                {
                    var temp1 = SortItemHelper.GetQueryable(sortItems, result);
                    var temp2 = new PagedList<TDomain>(temp1, pageIndex, pageSize);
                    PagedList<TDomainDto> pMember = new PagedList<TDomainDto>(temp2.Select(p => _mapper.MapTo(p)).ToList(), pageIndex, pageSize, temp2.TotalCount);
                    return new Paginated<TDomainDto>(pMember);
                }
                var temp11 = SortItemHelper.GetQueryable(sortItems, dbQuery);
                var temp22 = new PagedList<TDomain>(temp11, pageIndex, pageSize);
                PagedList<TDomainDto> pMember1 = new PagedList<TDomainDto>(temp22.Select(p => _mapper.MapTo(p)).ToList(), pageIndex, pageSize, temp22.TotalCount);
                return new Paginated<TDomainDto>(pMember1);

            }
        }

        public virtual TDomainDto Save(TDomainDto domainDto)
        {
            var domain = _mapper.CreateFrom(domainDto);

            using (var dbContext = new ApplicationDbContext())
            {
                if (domain.IdIsEmpty())
                {
                    domain.SetNewId();
                    dbContext.Set<TDomain>().Add(domain);
                }
                else
                {
                    var d = dbContext.Set<TDomain>().Find(domain.Id);
                    if (d == null)
                        dbContext.Set<TDomain>().Add(domain);
                    else
                    {
                        dbContext.Entry(d).State = EntityState.Detached;
                        dbContext.Set<TDomain>().Attach(domain);
                        dbContext.Entry(domain).State = EntityState.Modified;
                    }
                }
                dbContext.SaveAllChanges();
                return _mapper.MapTo(domain);
            }
        }

        public virtual List<TDomainDto> RangeInsert(IList<TDomainDto> domainDtos)
        {
            var result = new List<TDomainDto>();
            var domains = new List<TDomain>();
            foreach (var domainDto in domainDtos)
            {
                domains.Add(_mapper.CreateFrom(domainDto));
            }
            using (var dbContext = new ApplicationDbContext())
            {
                foreach (var domain in domains)
                {
                    if (domain.IdIsEmpty())
                    {
                        domain.SetNewId();
                        dbContext.Set<TDomain>().Add(domain);
                    }
                    else
                    {
                        dbContext.Set<TDomain>().Add(domain);
                    }
                    result.Add(_mapper.MapTo(domain));
                }
                dbContext.SaveAllChanges();

                return result;
            }
        }

        public TDomainDto Get(Criteria criteria)
        {
            Expression<Func<TDomain, bool>> expression = p => true;
            if (criteria != null)
                expression = ExpressionHelper.CreateFromCriteria<TDomain>(criteria);

            using (var db = new ApplicationDbContext())
            {
                IQueryable<TDomain> dbQuery = db.Set<TDomain>().Where(expression); ;
                foreach (var item in navigationProperty)
                    dbQuery = dbQuery.Include(item);
                return _mapper.MapTo(dbQuery.FirstOrDefault());
            }
        }

        public IList<TDomainDto> GetList(Criteria criteria)
        {
            Expression<Func<TDomain, bool>> expression = p => true;
            if (criteria != null)
                expression = ExpressionHelper.CreateFromCriteria<TDomain>(criteria);

            using (var db = new ApplicationDbContext())
            {
                IQueryable<TDomain> dbQuery = db.Set<TDomain>().Where(expression); ;
                foreach (var item in navigationProperty)
                    dbQuery = dbQuery.Include(item);

                return dbQuery.ToList().Select(t => _mapper.MapTo(t)).ToList();
            }
        }
    }
}
