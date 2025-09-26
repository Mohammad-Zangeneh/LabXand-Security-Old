using Kms.DataContract;
using Kms.DomainModel;
using Kms.Persistance.Repository.Core;
using Kms.Security.Core;
using LabXand.Core;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kms.Security.Util;
using LabXand.Infrastructure.Data.Redis;
using LabXand.DistributedServices;


namespace Kms.Security.Persistance
{
    public class OrganizationRestriction<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier> where TDomain : DomainEntityBase<TIdentifier>
    {
        private IUserContextDetector<KmsUserContext> _userContextDetector;
        private string _organizationIdPropertyName;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IRedisCacheService cacheManager;
        public readonly IEntityMapper<Organization, OrganizationDto> _mapper;
        public KmsUserContext UserContext
        {
            get
            {
                if (_userContextDetector != null)
                {
                    return _userContextDetector.UserContext;
                }
                return null;
            }
        }

        public OrganizationRestriction(IUserContextDetector<KmsUserContext> userContextDetector, IOrganizationRepository organizationRepository, string organizationIdPropertyName, IRedisCacheService cacheManager, IEntityMapper<Organization, OrganizationDto> mapper)
        {
            _organizationIdPropertyName = organizationIdPropertyName;
            this.organizationRepository = organizationRepository;
            this.cacheManager = cacheManager;
            _mapper = mapper;
            if (userContextDetector != null)
            {
                _userContextDetector = userContextDetector;
            }
        }

        public Expression<Func<TDomain, bool>> Get()
        {
            if (UserContext == null)
            {
                return (TDomain domain) => false;
            }
            if (UserContext.IsSuperAdmin)
            {
                string cacheKey = RedisKey.OrganizationDtoList + RedisKey.All;
                var orgsFromRedis = cacheManager.GetObject<List<OrganizationDto>>(cacheKey, RedisDbType.Organization);

                if (orgsFromRedis == null || orgsFromRedis.Count() == 0)
                {
                    var orgsFromRepository = organizationRepository.GetAllWithNullNavigationProperty().ToList();
                    orgsFromRedis = new List<OrganizationDto>();
                    foreach ( var org in orgsFromRepository )
                    {
                        orgsFromRedis.Add(_mapper.MapTo(org));
                    }
                    cacheManager.SetObject(cacheKey, orgsFromRedis, RedisDbType.Organization);
                }

                var allOrganizations = orgsFromRedis;
                var superAdminSubOrganizations = GetSubTree(allOrganizations, UserContext.OrganizationId).Select(t => t.Id);
                var parameter = Expression.Parameter(typeof(TDomain), "p");

                var criteria = CriteriaBuilder.CreateFromilterOperation<TDomain>(FilterOperations.Contains, _organizationIdPropertyName, superAdminSubOrganizations);

                return ExpressionHelper.CreateFromCriteria<TDomain>(criteria);
            }
            return ExpressionHelper.CreateEqualCondition<TDomain, Guid>(_organizationIdPropertyName, UserContext.OrganizationId);
        }

        private List<OrganizationDto> GetSubTree(List<OrganizationDto> organizationsTree, Guid organizationId)
        {
            var subTree = new List<OrganizationDto>
            {
                organizationsTree.Where(t => t.Id == organizationId).FirstOrDefault()
            };

            while (true)
            {
                var addingOrganizations = organizationsTree.Where(t => subTree.Any(s => s.Id == t.ParentId));
                addingOrganizations = addingOrganizations.Where(t => !subTree.Any(s => s.Id == t.Id));
                if (addingOrganizations.Count() == 0)
                    break;
                subTree.AddRange(addingOrganizations);
            }

            return subTree;
        }
    }

}
