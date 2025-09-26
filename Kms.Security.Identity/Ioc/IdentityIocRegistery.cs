
using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity.Service;
using LabXand.DistributedServices;
using LabXand.Logging.Core;
using LabXand.Logging.LogService;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using LabXand.Infrastructure.Data.Redis;
using StructureMap;
using System;
using System.Web;
using Kms.Security.Util;
using Kms.Security.Identity.Service.Contracts;

namespace Kms.Security.Identity
{
    class test
    {
        public static IAuthenticationManager v()
        {
            try
            {
                if (HttpContext.Current == null) return null;

                return HttpContext.Current.GetOwinContext().Authentication;
            }
            catch
            {
                return null;
            }
        }
    }

    public class IdentityIocRegistery : Registry
    {
        public IdentityIocRegistery()
        {
            For<IIdentityUnitOfWork>().Use<ApplicationDbContext>();
            For<IUserStore<ApplicationUser, Guid>>().Use<CustomUserStore>();
            For<IRoleStore<CustomRole, Guid>>().Use<CustomRoleStore>();
            For<IApplicationSignInManager>().Use<ApplicationSignInManager>();
            For<IApplicationRoleManager>().Use<ApplicationRoleManager>();
            For<ICustomRoleStore>().Use<CustomRoleStore>();
            For<ICustomUserStore>().Use<CustomUserStore>();
            For<IApplicationUserManager>().Use<ApplicationUserManager>();
            For<IKmsApplicationUserManager>().Use<ApplicationUserManager>();
            For<IAuthenticationManager>().Use(() => test.v());
            For<IApplicationSignInManager>().Use<ApplicationSignInManager>();
            For<IEntityMapper<ApplicationUser, MemberDto>>().Use<MemberMapper>();
            #region permission
            For<IEntityMapper<Permission, PermissionDto>>().Use<PermissionMapper>();
            For<IPermissionService>().Use<PermissionService>();

            For<IEntityMapper<LabxandRole, RoleDto>>().Use<RoleMapper>();
            For<IEntityMapper<PermissionRole, PermissionRoleDto>>().Use<PermissionRoleMapper>();
            For<IRoleService>().Use<RoleService>();

            For<IEntityMapper<Organization, OrganizationDto>>().Use<OrganizationMapper>();
            For<IEntityMapper<EnterprisePosition, EnterprisePositionDto>>().Use<EnterprisePositionMapper>();

            For<IOrganizationService>().Use<OrganizationService>();
            For<IEnterprisePositionService>().Use<EnterprisePositioServic>();

            For<IEntityMapper<EnterprisePositionPost, EnterprisePositionPostDto>>().Use<EnterprisePositionPostMapper>();
            For<IEnterprisePositionPostService>().Use<EnterprisePositionPostService>();

            For<IEntityMapper<Company, CompanyDto>>().Use<CompanyMapper>();
            For<ICompanyService>().Use<CompanyService>();

            For<IEntityMapper<PermissionCategory, PermissionCategoryDto>>().Use<PermissionCategoryMapper>();
            For<IPermissionCategoryService>().Use<PermissionCategoryService>();

            For<IEntityMapper<LoginHistory, LoginHistoryDto>>().Use<LoginHistoryMapper>();
            For<ILoginHistoryService>().Use<LoginHistoryService>();

            For<IEntityMapper<ClientIPAccess, ClientIPAccessDto>>().Use<ClientIPAccessMapper>();
            For<IClientIPAccessService>().Use<ClientIPAccessService>();


            For<IEntityMapper<UserToken, UserTokenDto>>().Use<UserTokenMapper>();
            For<IJwtUtil>().Use<JwtUtil>();
            #endregion
            For<ILogger>().Use<ElasticLogger<KmsUserContext>>();
            For<IEntityMapper<SecurityConfiguration, SecurityConfigurationDto>>().Use<SecurityConfigurationMapper>();
            For<ISecurityConfigurationService>().Use<SecurityConfigurationService>();
            For<IRedisCacheService>().Use<RedisCacheService>();
            For<IRedisStore>().Use<RedisStore>();
            For<ICaptchaService>().Use<CaptchaService>();
            For<ICaptchaGenerator>().Use<CaptchaGenerator>();
            For<IAuthService>().Use<AuthService>();

        }
    }
}