using Kms.DataContract;
using Kms.DomainModel;
using Kms.Persistance.Repository.Core;
using Kms.Security.Core;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class PrimaryAppraisorRestrictionWithAccessSetting<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier> where TDomain : DomainEntityBase<TIdentifier>
{
    private readonly IUserContextDetector<KmsUserContext> _userContextDetector;

    private readonly string _organizationIdPropertyName;

    private readonly string _knowledgeFieldPropertyName;

    private readonly IPrimaryAppraisorsRepository _primaryAppraisorRepository;
    private readonly IPrimaryAppraisorsOnEnterprisePositionRepository _primaryAppraisorsOnEnterprisePositionRepository;
    private readonly bool _usePrimaryAppraisorsOnEnterprisePositionRepository;
    private readonly IAccessSettingRepository _accessSettingRepository;

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

    public PrimaryAppraisorRestrictionWithAccessSetting(IUserContextDetector<KmsUserContext> userContextDetector,
        IPrimaryAppraisorsRepository primaryAppraisorRepository, IAccessSettingRepository accessSettingRepository,
           IPrimaryAppraisorsOnEnterprisePositionRepository primaryAppraisorsOnEnterprisePositionRepository = null,
        string organizationIdPropertyName = "OwnerOrganizationId", string knowledgeFieldPropertyName = "KnowledgeFieldId",
             bool usePrimaryAppraisorsOnEnterprisePositionRepository = false)
    {
        _organizationIdPropertyName = organizationIdPropertyName;
        if (userContextDetector != null)
        {
            _userContextDetector = userContextDetector;
        }
        _knowledgeFieldPropertyName = knowledgeFieldPropertyName;
        _primaryAppraisorRepository = primaryAppraisorRepository;
        _accessSettingRepository = accessSettingRepository;
        _primaryAppraisorsOnEnterprisePositionRepository = primaryAppraisorsOnEnterprisePositionRepository;
        _usePrimaryAppraisorsOnEnterprisePositionRepository = usePrimaryAppraisorsOnEnterprisePositionRepository;
    }

    private Expression GetParameter(string propertyName, ParameterExpression parameter)
    {
        Expression expression = parameter;
        string[] array = propertyName.Split('.');
        foreach (string propertyOrFieldName in array)
        {
            expression = Expression.PropertyOrField(expression, propertyOrFieldName);
        }
        return expression;
    }

    public Expression<Func<TDomain, bool>> Get()
    {
        if (UserContext == null)
        {
            return (TDomain domain) => false;
        }

        PrimaryAppraisor primaryAppraisorsWithMemberId = _primaryAppraisorRepository.GetPrimaryAppraisorsWithMemberId(UserContext.MemberId);
        var parameterExpression = Expression.Parameter(typeof(TDomain), "x");

        if ((DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberId == (DomainEntityBase<Guid>)null && !_usePrimaryAppraisorsOnEnterprisePositionRepository)
        {
            return (TDomain domain) => false;
        }
        else if (!_usePrimaryAppraisorsOnEnterprisePositionRepository)
        {
            var body = GenerateExpressionForKnowledgeField(primaryAppraisorsWithMemberId, parameterExpression);
            return Expression.Lambda<Func<TDomain, bool>>(body, new ParameterExpression[1] { parameterExpression });
        }

        PrimaryAppraisorOnEnterprisePosition primaryAppraisorsWithMemberIdonEnterprise = _primaryAppraisorsOnEnterprisePositionRepository.GetPrimaryAppraisors(UserContext.MemberId);

        if ((DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberIdonEnterprise == (DomainEntityBase<Guid>)null && (DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberId == (DomainEntityBase<Guid>)null)
            return (TDomain domain) => false;

        else if (!((DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberIdonEnterprise == (DomainEntityBase<Guid>)null) && !((DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberId == (DomainEntityBase<Guid>)null))
        {
            var body = GenerateExpressionForKnowledgeField(primaryAppraisorsWithMemberId, parameterExpression);
            body = Expression.Or(body, GenerateExpressionForEnterprisePosition(primaryAppraisorsWithMemberIdonEnterprise, parameterExpression));
            return Expression.Lambda<Func<TDomain, bool>>(body, new ParameterExpression[1] { parameterExpression });
        }
        else if (!((DomainEntityBase<Guid>)(object)primaryAppraisorsWithMemberId == (DomainEntityBase<Guid>)null))
        {
            var body = GenerateExpressionForKnowledgeField(primaryAppraisorsWithMemberId, parameterExpression);
            return Expression.Lambda<Func<TDomain, bool>>(body, new ParameterExpression[1] { parameterExpression });
        }
        else
        {
            var body = GenerateExpressionForEnterprisePosition(primaryAppraisorsWithMemberIdonEnterprise, parameterExpression);
            return Expression.Lambda<Func<TDomain, bool>>(body, new ParameterExpression[1] { parameterExpression });
        }
    }

    private BinaryExpression GenerateExpressionForKnowledgeField(PrimaryAppraisor primaryAppraisorsWithMemberId, ParameterExpression parameterExpression)
    {
        BinaryExpression body;
        IList<Guid> list = new List<Guid>
        {
            UserContext.OrganizationId
        };
        IList<AccessSetting> accessSettingForDestinationOrganization = _accessSettingRepository.GetAccessSettingForDestinationOrganization(UserContext.OrganizationId);
        foreach (AccessSetting item in accessSettingForDestinationOrganization)
        {
            AccessDto val = item.GetAccessList().FirstOrDefault((AccessDto p) => p.Value && p.Id == 8192);
            if (val != null)
            {
                list.Add((item).TraceData.OwnerOrganizationId);
            }
        }

        Expression parameter = GetParameter(_organizationIdPropertyName, parameterExpression);
        Expression parameter2 = GetParameter(_knowledgeFieldPropertyName, parameterExpression);
        Type type = parameter2.Type;
        if (type.IsNullable())
        {
            parameter2 = GetParameter(_knowledgeFieldPropertyName + ".Value", parameterExpression);
        }
        List<Guid> value = primaryAppraisorsWithMemberId.ListOfKnowledgeFields.Select((KnowledgeField p) => ((DomainEntityBase<Guid>)(object)p).Id).ToList();
        MethodInfo method = typeof(List<Guid>).GetMethod("Contains", new Type[1] { typeof(Guid) });
        ConstantExpression instance = Expression.Constant(value);
        MethodCallExpression left = Expression.Call(instance, method, parameter2);
        MethodInfo method2 = typeof(List<Guid>).GetMethod("Contains", new Type[1] { typeof(Guid) });
        ConstantExpression instance2 = Expression.Constant(list);
        MethodCallExpression right = Expression.Call(instance2, method2, parameter);
        body = Expression.And(left, right);
        return body;
    }

    private BinaryExpression GenerateExpressionForEnterprisePosition(PrimaryAppraisorOnEnterprisePosition primaryAppraisorsWithMemberIdonEnterprise, ParameterExpression parameterExpression)
    {
        BinaryExpression body;
        IList<Guid> list = new List<Guid>
        {
            UserContext.OrganizationId
        };
        IList<AccessSetting> accessSettingForDestinationOrganization = _accessSettingRepository.GetAccessSettingForDestinationOrganization(UserContext.OrganizationId);
        foreach (AccessSetting item in accessSettingForDestinationOrganization)
        {
            AccessDto val = item.GetAccessList().FirstOrDefault((AccessDto p) => p.Value && p.Id == 8192);
            if (val != null)
            {
                list.Add((item).TraceData.OwnerOrganizationId);
            }
        }

        Expression parameter = GetParameter(_organizationIdPropertyName, parameterExpression);
        Expression parameter2 = GetParameter("TraceData.OwnerEnterprisePositionId", parameterExpression);
        Type type = parameter2.Type;
        if (type.IsNullable())
        {
            parameter2 = GetParameter("TraceData.OwnerEnterprisePositionId" + ".Value", parameterExpression);
        }
        List<Guid> value = primaryAppraisorsWithMemberIdonEnterprise.ListOfEnterprisePositions.Select((EnterprisePosition p) => ((DomainEntityBase<Guid>)(object)p).Id).ToList();
        MethodInfo method = typeof(List<Guid>).GetMethod("Contains", new Type[1] { typeof(Guid) });
        ConstantExpression instance = Expression.Constant(value);
        MethodCallExpression left = Expression.Call(instance, method, parameter2);
        MethodInfo method2 = typeof(List<Guid>).GetMethod("Contains", new Type[1] { typeof(Guid) });
        ConstantExpression instance2 = Expression.Constant(list);
        MethodCallExpression right = Expression.Call(instance2, method2, parameter);
        body = Expression.And(left, right);
        return body;
    }
}