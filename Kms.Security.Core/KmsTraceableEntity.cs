using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LabXand.Core;
using LabXand.DomainLayer.Core;

namespace Kms.Security.Core
{

	public abstract class KmsTraceableEntity<TIdentifier> : TracableDomainEntityBase<TIdentifier, KmsTraceData, KmsUserContext>
	{
		protected List<string> traceInstance = new List<string>();

		public virtual void SetOwnerTraceData(Guid? ownerEnterprisePositionId, Guid ownerMemberId, Guid ownerOrganizationId)
		{
			base.TraceData.SetOwnerTraceData(ownerEnterprisePositionId, ownerMemberId, ownerOrganizationId);
		}

		public void SetOwnerOrganizationId(Guid ownerOrganizationId)
		{
			base.TraceData.SetOwnerOrganizationId(ownerOrganizationId);
		}

		public void SetOwnerMemberId(Guid ownerMemberId)
		{
			base.TraceData.SetOwnerMemberId(ownerMemberId);
		}

		public void SetOwnerEnterprisePositionId(Guid? ownerEnterprisePositionId)
		{
			base.TraceData.SetOwnerEnterprisePositionId(ownerEnterprisePositionId);
		}

		public void SetTraceData(KmsTraceData traceData)
		{
			base.TraceData.SetFullTraceData(traceData);
		}

		public override void SetTraceData(KmsUserContext userContext)
		{
			KmsTraceData kmsTraceData = new KmsTraceData(userContext);
			base.TraceData.SetTraceData(kmsTraceData);
			SetOwnerTraceData(kmsTraceData.WriterEnterprisePositionId, kmsTraceData.WriterMemberId, kmsTraceData.WriterOrganizationId);
			base.TraceData.SetLastModificationData(userContext);
			Type type = ((object)this).GetType();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (TypeHelper.IsPropertyACollection(propertyInfo))
				{
					Type type2 = propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault();
					if (!(type2 != null) || !(propertyInfo.Name != "Events") || !(propertyInfo.GetValue(this) is IEnumerable enumerable))
					{
						continue;
					}
					foreach (object item in enumerable)
					{
						SetInternalPropertiesTraceData(type2, item, userContext);
					}
				}
				else
				{
					SetInternalPropertiesTraceData(propertyInfo.PropertyType, propertyInfo.GetValue(this), userContext);
				}
			}
		}

		private void SetInternalPropertiesTraceData(Type propertyType, object objectInstance, KmsUserContext userContext)
		{
			if (objectInstance != null && propertyType.GetInterface(typeof(ITracableEntity<KmsUserContext>).Name) != null)
			{
				ITracableEntity<KmsUserContext> val = (ITracableEntity<KmsUserContext>)objectInstance;
				string item = $"{objectInstance.GetType().Name}-{((object)val).GetHashCode()}";
				if (!traceInstance.Contains(item))
				{
					traceInstance.Add(item);
					val.SetTraceData(userContext);
				}
			}
		}

		protected override KmsTraceData CreateNewTraceData()
		{
			return new KmsTraceData();
		}
	}
}