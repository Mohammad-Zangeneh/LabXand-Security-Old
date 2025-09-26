using System;
using System.Runtime.Serialization;
using Kms.Security.Common.Domain;

namespace Kms.Security.Core
{

	[DataContract(IsReference = true)]
	public class KmsTraceDataDto
	{
		[DataMember]
		public DateTime CreateDate { get; set; }

		[DataMember]
		public DateTime LastUpdateDate { get; set; }

		[DataMember]
		public Guid OwnerOrganizationId { get; set; }

		[DataMember]
		public Guid WriterOrganizationId { get; set; }

		[DataMember]
		public Guid WriterMemberId { get; set; }

		[DataMember]
		public Guid OwnerMemberId { get; set; }

		[DataMember]
		public string WriterUserName { get; set; }

		[DataMember]
		public Guid LastUpdateMemberId { get; set; }

		[DataMember]
		public Guid? OwnerEnterprisePositionId { get; set; }

		[DataMember]
		public Guid? WriterEnterprisePositionId { get; set; }

		[DataMember]
		public MemberDto OwnerMember { get; set; }

		[DataMember]
		public OrganizationDto OwnerOrganization { get; set; }

		[DataMember]
		public EnterprisePositionDto OwnerEnterprisePosition { get; set; }
	}
}