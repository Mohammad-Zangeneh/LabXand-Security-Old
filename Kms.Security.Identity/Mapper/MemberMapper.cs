using Kms.Security.Common.DataContract.Member;
using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using LabXand.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Kms.Security.Identity
{
    public class MemberMapper : IEntityMapper<ApplicationUser, MemberDto>
    {

        IEntityMapper<LabxandRole, RoleDto> _roleMapper;
        IEntityMapper<EnterprisePositionPost, EnterprisePositionPostDto> _postMapper;
        IEntityMapper<EnterprisePosition, EnterprisePositionDto> _enterprisePositionMapper;
        IEntityMapper<Organization, OrganizationDto> _organizationMapper;
        public MemberMapper(IEntityMapper<LabxandRole, RoleDto> roleMapper, IEntityMapper<EnterprisePositionPost, EnterprisePositionPostDto> postMapper
            , IEntityMapper<EnterprisePosition, EnterprisePositionDto> enterprisePositionMapper
            , IEntityMapper<Organization, OrganizationDto> organizationMapper)
        {
            _postMapper = postMapper;
            _roleMapper = roleMapper;
            _enterprisePositionMapper = enterprisePositionMapper;
            _organizationMapper = organizationMapper;
        }
        public ApplicationUser CreateFrom(MemberDto destination)
        {
            var domain = new ApplicationUser();
            domain.FirstName = destination.FirstName.ApplyCorrectYeKe();
            domain.LastName = destination.LastName.ApplyCorrectYeKe();
            domain.UserName = destination.UserName.ApplyCorrectYeKe();
            domain.Id = destination.Id;
            domain.Email = destination.Email.ApplyCorrectYeKe();
            domain.EnterprisePositionPosts = destination.EnterprisePositionPosts != null ? destination.EnterprisePositionPosts.Select(p => _postMapper.CreateFrom(p)).ToList() : null;
            domain.LabxandRoles = destination.Roles != null ? destination.Roles.Select(p => _roleMapper.CreateFrom(p)).ToList() : null;
            domain.Organization = destination.Organization != null ? _organizationMapper.CreateFrom(destination.Organization) : null;
            domain.OrganizationId = destination.OrganizationId;

            domain.EnterprisePosition = destination.EnterprisePosition != null ? _enterprisePositionMapper.CreateFrom(destination.EnterprisePosition) : null;
            domain.EnterprisePositionId = destination.EnterprisePositionId;
            domain.UserStatus = destination.UserStatus;
            domain.IsSuperAdmin = destination.IsSuperAdmin;
            domain.PhoneNumber = destination.CellphoneNumber;
            //  domain.RegisterationDate = destination.RegisterationDate;
            domain.LastDeactivationDate = destination.LastDeactivationDate;
            domain.PersonnelNumber = destination.PersonnelNumber;
            return domain;
        }

        public MemberDto MapTo(ApplicationUser source)
        {

            var domainDto = new MemberDto
            {
                Id = source.Id,
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                UserName = source.UserName,
                FullName = source.FirstName + " " + source.LastName
            };
            domainDto.EnterprisePositionPosts = source.EnterprisePositionPosts != null ? source.EnterprisePositionPosts.Select(p => _postMapper.MapTo(p)).ToList() : null;
            domainDto.Roles = source.LabxandRoles != null ? source.LabxandRoles.Select(p => _roleMapper.MapTo(p)).ToList() : null;
            domainDto.OrganizationId = source.OrganizationId;
            if (source.Organization != null)
                source.Organization.SetEnterprisePositions(null);
            domainDto.Organization = source.Organization != null ? _organizationMapper.MapTo(source.Organization) : null;

            domainDto.EnterprisePositionId = source.EnterprisePositionId;
            domainDto.EnterprisePosition = source.EnterprisePosition != null ? _enterprisePositionMapper.MapTo(source.EnterprisePosition) : null;
            domainDto.UserStatus = source.UserStatus;
            domainDto.UserStatusValue = Util.EnumHelper.GetEnumDescription(source.UserStatus);
            domainDto.IsSuperAdmin = source.IsSuperAdmin;
            domainDto.RegisterationDate = source.RegisterationDate;
            domainDto.LastDeactivationDate = source.LastDeactivationDate;
            domainDto.CellphoneNumber = source.PhoneNumber;
            domainDto.PersonnelNumber = source.PersonnelNumber;
            return domainDto;
        }

        public MemberSessionInfoDto MapToMemberSessionInfoDto(ApplicationUser user)
        {
            var result = new MemberSessionInfoDto();
            result.Id = user.Id;
            result.UserName = user.UserName;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.FullName = user.FirstName + " " + user.LastName;
            result.Email = user.Email;
            //result.Password = user.PasswordHash;
            result.Roles = user.LabxandRoles?.Select(x => _roleMapper.MapTo(x)).ToList() ?? new List<RoleDto>();
            //result.EnterprisePositionPosts = user.EnterprisePositionPosts?.Select(x => _postMapper.MapTo(x)).ToList() ??
            //    new List<EnterprisePositionPostDto>();
            result.EnterprisePositionId = user.EnterprisePositionId;
            result.EnterprisePosition = user.EnterprisePosition != null ? _enterprisePositionMapper.MapTo(user.EnterprisePosition) : null;
            result.OrganizationId = user.OrganizationId;
            result.Organization = user.Organization != null ? _organizationMapper.MapTo(user.Organization) : null;
            result.UserStatus = user.UserStatus;
            result.UserStatusValue = user.UserStatus.GetDescription();
            result.IsSuperAdmin = user.IsSuperAdmin;
            result.RegisterationDate = user.RegisterationDate;
            result.CellPhoneNumber = user.PhoneNumber;
            result.PersonnelNumber = user.PersonnelNumber;
            result.RequirePasswordChangeStatus = user.RequirePasswordChangeStatus;
            return result;
        }
    }
}
