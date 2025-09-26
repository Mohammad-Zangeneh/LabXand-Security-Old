using Kms.Security.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kms.Security.Common.Domain

{
    public class MemberDto
    {
        [DataMember]
        public Guid Id { set; get; }
        [DataMember]
        public string UserName { set; get; }
        [DataMember]
        public string LastName { set; get; }
        [DataMember]
        public string FirstName { set; get; }
        [DataMember]
        public string Email { set; get; }
        [DataMember]
        public string Password { set; get; }

        [DataMember]
        public string FullName { get; set; }


        [DataMember]
        public IList<RoleDto> Roles { get; set; }

        [DataMember]
        public IList<EnterprisePositionPostDto> EnterprisePositionPosts { get; set; }

        [DataMember]
        public Guid? EnterprisePositionId { get; set; }
        [DataMember]
        public EnterprisePositionDto EnterprisePosition { get; set; }

        [DataMember]
        public Guid? OrganizationId { get; set; }
        [DataMember]
        public OrganizationDto Organization { get; set; }
        [DataMember]
        public UserStatus UserStatus { set; get; }

        [DataMember]
        public string UserStatusValue { set; get; }

        [DataMember]
        public bool IsSuperAdmin { get; set; }
        [DataMember]
        public DateTime RegisterationDate { get; set; }
        [DataMember]
        public DateTime? LastDeactivationDate { get; set; }
        [DataMember]
        public string CellphoneNumber { get; set; }

        [DataMember]
        public string PersonnelNumber { get; set; }
    }
}
