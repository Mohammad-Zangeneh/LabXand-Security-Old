using Kms.Security.Common.DataContract.Member;
using Kms.Security.Common.Domain;
using LabXand.Core;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public interface IKmsApplicationUserManager
    {
        IQueryable<ApplicationUser> GetUser();
        Guid GetJwtUserId();
        Task<bool> ChangePasswordCurrentUser(string password, string newPassword);
        Task<bool> ResetPasswordAsync(string newPassword, Guid userId);
        Task<MemberDto> CreateMemberAsync(ApplicationUser user, string password, string accessToken, bool changeIsAdmin = false);
        Task<MemberDto> RegisterRequest(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false);
        Task<MemberDto> EditMemberAsync(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false);
        Task<MemberDto> ChangeSupperAdmin(ApplicationUser user, string secretKey, bool isSuperAdminChanged = false);
        Task<MemberDto> AcceptRegisterRequestByAdmin(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false);
        Task<MemberDto> ChangeUserStatus(ApplicationUser user, string secretKey, bool isSuperAdminChanged = false);
        List<MemberDto> GetAll();
        List<MemberDto> GetAllWithEntAndOrg();
        List<MemberDto> GetAllWithRoles();
        List<ApplicationUser> GetUsersAsyncWithRoles(Guid id);
        List<ApplicationUser> GetUsersAsyncWithRoles(int userStatus, Guid organizationId);
        Task<List<ApplicationUser>> GetUsersAsyncWithRoles(int userStatus);
        Paginated<MemberDto> GetOnePageOfMemberList(Criteria criteria, int page, int pageSize, List<SortItem> sortItems);
        Task<ApplicationUser> GetCurrentUserAsync();
        ApplicationUser GetCurrentKmsUser();

        //   List<Permission> GetUserPermission(Guid userId);
        IList<ApplicationUser> GetUserWithRoleId(Guid roleId);
        IList<ApplicationUser> GetUserWithRoleName(string roleName);
        MemberDto GetMember(Guid userId);
        MemberDto GetMember(string username);
        MemberDto GetMemberWithRole(string username);
        ApplicationUser GetUserWithUsernameForWebApi(string userName);
        void EditProfile(MemberDto member);
        void DeleteRegisterRequest(MemberDto member);
        MemberDto CreateMember(MemberDto member, string accessToken);
        string GeneratePasswordResetToken(Guid userId);
        Task<IdentityResult> ResetPasswordAsync(Guid userId, string token, string newPassword);
        Task<string> GenerateChangePhoneNumberTokenAsync(Guid userId, string phoneNumber);

        /// <summary>
        /// Verify the code is valid for a specific user and for a specific phone number
        /// </summary>
        /// <param name="userId"/><param name="token"/><param name="phoneNumber"/>
        /// <returns/>
        Task<bool> VerifyChangePhoneNumberTokenAsync(Guid userId, string token, string phoneNumber);
        //
        // Summary:
        //     Used for generating reset password and confirmation tokens
        IUserTokenProvider<ApplicationUser, Guid> UserTokenProvider { get; set; }
        void ResetPassword(Guid userId, string token, string newPassword);
        Task<bool> RecoverPassword(string user, string password, string code);
        string GetTrackEntity();
        Claim[] KeycloakUserProccess(MemberDto memberDto, string secretKey);
        Claim[] CasUserProccess(string userName);
        void EditUser(ApplicationUser user, bool changeIsAdmin, string secretKey);
        MemberSessionInfoDto GetMemberSessionInfoById(Guid id);
        void ProcessPasswordExpiry();
    }
}
