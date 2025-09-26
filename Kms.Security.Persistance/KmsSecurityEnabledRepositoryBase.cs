// Kms.Security.Persistance.KmsSecurityEnabledRepositoryBase<TDomain,TIdentifier>
using System;
using System.Collections.Generic;
using System.Linq;
using Kms.Common.Enums;
using Kms.DataContract;
using Kms.DomainModel;
using Kms.Persistance.Repository.Core;
using Kms.Security.Core;
using Kms.Security.Persistance;
using LabXand.DistributedServices;
using LabXand.DomainLayer.Core;
using LabXand.Infrastructure.Data;
using LabXand.Infrastructure.Data.EF;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Security.Core;


public abstract class KmsSecurityEnabledRepositoryBase<TDomain, TIdentifier> : EFSecurityEnabledRepositoryBase<TDomain, TIdentifier, KmsUserContext> where TDomain : DomainEntityBase<TIdentifier>
{
    protected override List<string> ConstantFields => new List<string> { "TraceData.CreateDate", "TraceData.WriterOrganizationId", "TraceData.WriterMemberId", "TraceData.WriterUserName", "TraceData.WriterEnterprisePositionId" };
    private readonly IOrganizationRepository organizationRepository;
    private readonly IRedisCacheService cacheManager;
    private readonly IEntityMapper<Organization, OrganizationDto> mapper;

    public KmsSecurityEnabledRepositoryBase(IDataContext dataContext, IUserContextDetector<KmsUserContext> userContextDetector, IOrganizationRepository organizationRepository, IRedisCacheService cacheManager, IEntityMapper<Organization, OrganizationDto> mapper)
        : base(dataContext, userContextDetector)
    {
        this.organizationRepository = organizationRepository;
        this.cacheManager = cacheManager;
        this.mapper = mapper;
    }

    public KmsSecurityEnabledRepositoryBase<TDomain, TIdentifier> HasPrimaryAppraisorRestrictionWithAccessSetting(IPrimaryAppraisorsRepository primaryApprisorRepositoy, IAccessSettingRepository accessRepository, IPrimaryAppraisorsOnEnterprisePositionRepository primaryAppraisorsOnEnterprisePositionRepository = null, string knowledgeFieldPropertyName = "KnowledgeFieldId", string organizationIdPropertyName = "TraceData.OwnerOrganizationId", bool usePrimaryAppraisorsOnEnterprisePositionRepository = false)
    {
        ClearRestriction();
        HasRestriction(new PrimaryAppraisorRestrictionWithAccessSetting<TDomain, TIdentifier>(_userContextDetector, primaryApprisorRepositoy, accessRepository, primaryAppraisorsOnEnterprisePositionRepository, organizationIdPropertyName, knowledgeFieldPropertyName, usePrimaryAppraisorsOnEnterprisePositionRepository));
        return this;
    }

    public KmsSecurityEnabledRepositoryBase<TDomain, TIdentifier> HasOrganizationRestrictionWithAccessSetting(RestrictionType restricationType, IAccessSettingRepository accessSettingRepository, Guid? organizationId = null, string organizationIdPropertyName = "TraceData.OwnerOrganizationId")
    {
        ClearRestriction();
        HasRestriction(new OrganizationRestrictionWithAccessSetting<TDomain, TIdentifier>(_userContextDetector, organizationIdPropertyName, restricationType, accessSettingRepository, organizationRepository, cacheManager, mapper, organizationId));
        return this;
    }

    public KmsSecurityEnabledRepositoryBase<TDomain, TIdentifier> HasOrganizationRestriction(string organizationIdPropertyName = "TraceData.OwnerOrganizationId")
    {
        HasRestriction(new OrganizationRestriction<TDomain, TIdentifier>(_userContextDetector, organizationRepository, organizationIdPropertyName, cacheManager, mapper));
        return this;
    }

    public void ClearRestriction()
    {
        List<IRowRestrictionSpecification<TDomain, TIdentifier>> list = base.RowRestrictionSpecifications.Where((IRowRestrictionSpecification<TDomain, TIdentifier> t) => t.GetType().Name.Contains("ConditionRestriction")).ToList();
        base.RowRestrictionSpecifications.Clear();
        foreach (IRowRestrictionSpecification<TDomain, TIdentifier> item in list)
        {
            base.RowRestrictionSpecifications.Add(item);
        }
    }
}
