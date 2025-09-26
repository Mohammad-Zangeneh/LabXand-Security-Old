using Kms.Security.Common.Domain;
using Kms.Security.Util;
using System;
using System.Collections.Generic;

namespace Kms.Security.Common.DataContract.Member
{
    public class MemberSessionInfoDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
        public Guid? EnterprisePositionId { get; set; }
        public EnterprisePositionDto EnterprisePosition { get; set; }
        public Guid? OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }
        public UserStatus UserStatus { get; set; }
        public string UserStatusValue { get; set; }
        public bool IsSuperAdmin { get; set; }
        public DateTime RegisterationDate { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PersonnelNumber { get; set; }
        public RequirePasswordChangeStatus RequirePasswordChangeStatus { get; set; }

        //public List<EnterprisePositionPostDto> EnterprisePositionPosts { get; set; } 
        //public string Password { get; set; }
    }
}
