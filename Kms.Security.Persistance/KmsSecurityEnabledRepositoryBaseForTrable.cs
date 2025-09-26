using System.Collections.Generic;
using Kms.Security.Core;
using LabXand.Infrastructure.Data;
using LabXand.Security.Core;
using LabXand.Extensions;
using System;
using Kms.Persistance.Repository.Core;
using LabXand.Infrastructure.Data.Redis;
using LabXand.DistributedServices;
using Kms.DomainModel;
using Kms.DataContract;

namespace Kms.Security.Persistance
{
    public abstract class KmsSecurityEnabledRepositoryBaseForTrable<TDomain, TIdentifier> : KmsSecurityEnabledRepositoryBase<TDomain, TIdentifier>
        where TDomain : KmsTraceableEntity<TIdentifier>
    {
        public KmsSecurityEnabledRepositoryBaseForTrable(IDataContext dataContext, IUserContextDetector<KmsUserContext> userContextDetector, IOrganizationRepository organizationRepository, IRedisCacheService cacheManager, IEntityMapper<Organization, OrganizationDto> mapper)
            : base(dataContext, userContextDetector, organizationRepository, cacheManager, mapper)
        {
        }

        protected override List<string> ConstantFields
        {
            get
            {
                var constantFields = base.ConstantFields;
                if (constantFields == null)
                    constantFields = new List<string>();
                constantFields.Add(ExpressionHelper.GetNameOfProperty<TDomain, DateTime>(t => t.TraceData.CreateDate));
                constantFields.Add(ExpressionHelper.GetNameOfProperty<TDomain, Guid>(t => t.TraceData.OwnerOrganizationId));
                return constantFields;
            }
        }
    }
}
