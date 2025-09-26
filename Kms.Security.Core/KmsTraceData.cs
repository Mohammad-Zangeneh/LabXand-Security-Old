using System;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace Kms.Security.Core
{

	public class KmsTraceData : ITraceData
	{
		public DateTime CreateDate { get; protected set; }

		public DateTime LastUpdateDate { get; protected set; }

		public Guid OwnerOrganizationId { get; protected set; }

		public Guid WriterOrganizationId { get; protected set; }

		public Guid WriterMemberId { get; protected set; }

		public Guid OwnerMemberId { get; protected set; }

		public string WriterUserName { get; protected set; }

		public Guid LastUpdateMemberId { get; protected set; }

		public Guid? OwnerEnterprisePositionId { get; set; }

		public Guid? WriterEnterprisePositionId { get; set; }
       // public Guid OrganizationId { get; protected set; }


        public KmsTraceData()
		{
		}

		public KmsTraceData(DateTime createDate, DateTime lastUpdateDate, Guid ownerOrganizationId, Guid writerOrganizationId, Guid writerMemberId, Guid ownerMemberId, string writerUserName, Guid lastUpdateMemberId, Guid? ownerEnterprisePositionId, Guid? writerEnterprisePositionId)
		{
			CreateDate = createDate;
			LastUpdateDate = lastUpdateDate;
			OwnerOrganizationId = ownerOrganizationId;
			WriterOrganizationId = writerOrganizationId;
			WriterMemberId = writerMemberId;
			OwnerMemberId = ownerMemberId;
			WriterUserName = writerUserName;
			LastUpdateMemberId = lastUpdateMemberId;
			OwnerEnterprisePositionId = ownerEnterprisePositionId;
			WriterEnterprisePositionId = writerEnterprisePositionId;
		}

		public KmsTraceData(KmsUserContext userContext)
		{
			CreateDate = DateTime.Now;
			WriterOrganizationId = userContext.OrganizationId;
			WriterMemberId = userContext.MemberId;
			WriterUserName = ((UserContextBase)userContext).UserName;
			WriterEnterprisePositionId = userContext.EnterprisePositionId;
		}

		public void SetFullTraceData(KmsTraceData traceData)
		{
			CreateDate = traceData.CreateDate;
			LastUpdateDate = traceData.LastUpdateDate;
			WriterOrganizationId = traceData.WriterOrganizationId;
			WriterMemberId = traceData.WriterMemberId;
			WriterUserName = traceData.WriterUserName;
			LastUpdateMemberId = traceData.LastUpdateMemberId;
			WriterEnterprisePositionId = traceData.WriterEnterprisePositionId;
			SetOwnerTraceData(traceData.OwnerEnterprisePositionId, traceData.OwnerMemberId, traceData.OwnerOrganizationId);
		}

		public void SetTraceData(KmsTraceData traceData)
		{
			CreateDate = traceData.CreateDate;
			LastUpdateDate = traceData.LastUpdateDate;
			WriterOrganizationId = traceData.WriterOrganizationId;
			WriterMemberId = traceData.WriterMemberId;
			WriterUserName = traceData.WriterUserName;
			LastUpdateMemberId = traceData.LastUpdateMemberId;
			WriterEnterprisePositionId = traceData.WriterEnterprisePositionId;
		}

		public virtual void SetOwnerTraceData(Guid? ownerEnterprisePositionId, Guid ownerMemberId, Guid ownerOrganizationId)
		{
			OwnerEnterprisePositionId = ownerEnterprisePositionId;
			OwnerMemberId = ownerMemberId;
			OwnerOrganizationId = ownerOrganizationId;
		}

		public void SetLastModificationData(KmsUserContext userContext)
		{
			LastUpdateDate = DateTime.Now;
			LastUpdateMemberId = userContext.MemberId;
		}

		public void SetOwnerOrganizationId(Guid ownerOrganizationId)
		{
			OwnerOrganizationId = ownerOrganizationId;
		}

		public void SetOwnerMemberId(Guid ownerMemberId)
		{
			OwnerMemberId = ownerMemberId;
		}

		public void SetOwnerEnterprisePositionId(Guid? ownerEnterprisePositionId)
		{
			OwnerEnterprisePositionId = ownerEnterprisePositionId;
		}
	}
}