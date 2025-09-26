using Kms.Security.Util;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Kms.Security.Common.Domain

{
    public class ApplicationUser : IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationUser()
        {
            RegisterationDate = DateTime.Now;

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual IList<EnterprisePositionPost> EnterprisePositionPosts { get; set; }

        public virtual IList<LabxandRole> LabxandRoles { set; get; }
        public string SerialNumber { get; set; }//for checking token not change - when user permission change , this key is change
        public Organization Organization { get; set; }
        public Guid? OrganizationId { get; set; }


        public EnterprisePosition EnterprisePosition { get; set; }
        public Guid? EnterprisePositionId { get; set; }
        public UserStatus UserStatus { set; get; }
        public bool IsSuperAdmin { get; set; }
        public DateTime RegisterationDate { get; set; }
        public DateTime? LastDeactivationDate { get; set; }
        public string PersonnelNumber { get; set; }
        public int NumberOfFailedLogin { get; set; }
        public DateTime? LastTimeLoginFailed { get; set; }

        public int LoginValue { get; set; }
        public RequirePasswordChangeStatus RequirePasswordChangeStatus { get; set; }
        public DateTime? LastUserPasswordChange { get; set; }


        public override string ToString()
        {
            return $" کاربر {this.UserName} ";
        }
    }
}