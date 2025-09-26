using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kms.Common.Enums;
using Kms.DataContract;
using Kms.DomainModel;
using Kms.Persistance.Repository.Core;
using Kms.Security.Core;
using Kms.Security.Util;
using LabXand.Core;
using LabXand.DistributedServices;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Security.Core;

public class OrganizationRestrictionWithAccessSetting<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier> where TDomain : DomainEntityBase<TIdentifier>
{
    private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
    private readonly string _organizationIdPropertyName;
    private readonly RestrictionType _restricationType;
    private readonly IAccessSettingRepository _accessSettingRepository;
    private Guid? _organizationId;
    private readonly bool _isFromSource;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IRedisCacheService _cacheManager;
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

    public OrganizationRestrictionWithAccessSetting(IUserContextDetector<KmsUserContext> userContextDetector, string organizationIdPropertyName, RestrictionType restricationType, IAccessSettingRepository accessSettingRepository,  
        IOrganizationRepository organizationRepository, IRedisCacheService cacheManager, IEntityMapper<Organization, OrganizationDto> mapper, Guid? organizationId = null, bool IsFromSource = false)
    {
        _organizationIdPropertyName = organizationIdPropertyName;
        if (userContextDetector != null)
        {
            _userContextDetector = userContextDetector;
        }
        _restricationType = restricationType;
        _accessSettingRepository = accessSettingRepository;
        _organizationId = _userContextDetector?.UserContext?.OrganizationId;
        if (organizationId.HasValue)
        {
            _organizationId = organizationId.Value;
        }
        _isFromSource = IsFromSource;
        _organizationRepository = organizationRepository;
        _cacheManager = cacheManager;
        _mapper = mapper;
    }

    public Expression<Func<TDomain, bool>> Get()
    {

        if (UserContext == null || _accessSettingRepository == null)
        {
            return (TDomain domain) => false;
        }

        var superAdminSubOrganizations = new List<Guid>();

        if (UserContext.IsSuperAdmin)
        {
            string cacheKey = RedisKey.OrganizationDtoList + RedisKey.All;
            var orgsFromRedis = _cacheManager.GetObject<List<OrganizationDto>>(cacheKey, RedisDbType.Organization);

            if (orgsFromRedis == null || orgsFromRedis.Count() == 0)
            {
                var orgsFromRepository = _organizationRepository.GetAllWithNullNavigationProperty().ToList();
                orgsFromRedis = new List<OrganizationDto>();
                foreach (var org in orgsFromRepository)
                {
                    orgsFromRedis.Add(_mapper.MapTo(org));
                }
                _cacheManager.SetObject(cacheKey, orgsFromRedis, RedisDbType.Organization);
            }

            superAdminSubOrganizations.AddRange(GetSubTree(orgsFromRedis, UserContext.OrganizationId).Select(t => t.Id));
        }

        var allowedOrganizationIdList = new List<Guid>() { UserContext.OrganizationId };
        allowedOrganizationIdList.AddRange(superAdminSubOrganizations);

        IList<AccessSetting> accessSettings = _isFromSource ?
            _accessSettingRepository.GetAccessSettingForSourceOrganization(_organizationId.Value) :
            _accessSettingRepository.GetAccessSettingForDestinationOrganization(_organizationId.Value);
        
        allowedOrganizationIdList.AddRange(accessSettings.Where(t => t.GetAccessList().Any(x => x.Value && x.Id == (int)_restricationType)).Select(t => t.TraceData.OwnerOrganizationId));
        allowedOrganizationIdList = allowedOrganizationIdList.Distinct().ToList();

        ParameterExpression parameterExpression = Expression.Parameter(typeof(TDomain), "x");
        
        return LinqExpressionHelper.CreateContainsMethod<TDomain, List<Guid>>(allowedOrganizationIdList, _organizationIdPropertyName, parameterExpression);
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
