using Kms.Common.Util;
using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Common.DataContract.Member;
using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Kms.Security.Jwt;
using Kms.Security.OpenId;
using Kms.Security.Util;
using Kms.Security.WebApi;
using Kms.Security.WebApi.RateLimit;
using LabXand.Core;
using LabXand.DistributedServices;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Logging.Core;
using LabXand.Security.Core;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RecoverPasswordModel
    {
        public string Password { get; set; }
        public string User { get; set; }
        public string Code { get; set; }
    }

    public class LogoutModel
    {
        public Guid UserId { get; set; }
        public string FingerPrint { get; set; }

    }

    [DisplayName("کنترلر نمونه با سطح دسترسی پویا")]
    public class MemberController : ApiController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IKmsApplicationUserManager _userManager;
        private readonly IEntityMapper<ApplicationUser, MemberDto> _memberMapper;
        ITokenStoreService _tokenService;
        ILoginHistoryService _loginHistoryService;
        private readonly ILogger _logger;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IEnterprisePositionService _enterprisePositionService;

        public MemberController(IKmsApplicationUserManager userManager,
                                 IApplicationSignInManager signInManager,
                                 IAuthenticationManager authenticationManager, IEntityMapper<ApplicationUser, MemberDto> memberMapper,
                                 ITokenStoreService tokenService,
                                  ILoginHistoryService loginHistoryService,
                                  ILogger logger,
                                 IUserContextDetector<KmsUserContext> userContextDetector,
                                 IRedisCacheService redisCacheService
            , IEnterprisePositionService enterprisePositionService
                                )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _memberMapper = memberMapper;
            _tokenService = tokenService;
            _loginHistoryService = loginHistoryService;
            _logger = logger;
            _userContextDetector = userContextDetector;
            _redisCacheService = redisCacheService;
            _enterprisePositionService = enterprisePositionService;
        }
        //protected string GetAccessToken() => ActionContext.Request.Headers.Authorization.Parameter;
        protected string GetSecretKey() => WebConfigValues.SecretKey;

        [Description("ذخیره بهداشت")]
        [HttpPost]
        [Route("api/member/SaveForBehdasht", Name = "MemberSaveForBehdasht")]
        [RateLimit]
        [MachineAuthorize]
        public async Task<MemberDto> SaveForBehdasht(MemberDto memberDto)
        {
            MemberDto result = null;
            string description;
            try
            {
                var user = _memberMapper.CreateFrom(memberDto);
                result = await _userManager.CreateMemberAsync(user, memberDto.Password, GetSecretKey());
                var defaultDescription = $" ثبت کاربر جدید به وسیله احراز هویت بهداشت « {_userContextDetector.UserContext.UserName} »  با مشخصات ، نام : « {memberDto.FirstName}  {memberDto.LastName} - نام کاربری : [ {memberDto.UserName} ] » با موفقیت انجام گرفت ";
                description = _userManager.GetTrackEntity() ?? defaultDescription;

                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            }
            catch (Exception exception)
            {
                description = $"عدم موفقیت در ثبت کاربر به وسیله احراز هویت بهداشت  « {_userContextDetector.UserContext.UserName} »  برای « {memberDto.FirstName}  {memberDto.LastName} - [ {memberDto.UserName} ] » ";
                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
                throw new ViolationSecurityPolicyException(exception.Message, "");
            }

            return result;
        }

        [Description("ذخیره")]
        [HttpPost]
        [Route("api/member/Save", Name = "MemberSave")]
        [RateLimit]
        [JwtAuthorize(Permission = "SaveMember")]
        public async Task<MemberDto> Save(MemberDto memberDto)
        {
            MemberDto result = null;
            string description;
            _redisCacheService.RemoveKey(RedisKey.Permission + memberDto.Id, RedisDbType.Permission);
            UserManager.RemoveRedisObjects(memberDto.UserName);
            try
            {
                var user = _memberMapper.CreateFrom(memberDto);
                result = await _userManager.CreateMemberAsync(user, memberDto.Password, GetSecretKey());
                var defaultDescription = $" ثبت کاربر جدید توسط ادمین « {_userContextDetector.UserContext.UserName} »  با مشخصات ، نام : « {memberDto.FirstName}  {memberDto.LastName} - نام کاربری : [ {memberDto.UserName} ] » با موفقیت انجام گرفت ";
                description = _userManager.GetTrackEntity() ?? defaultDescription;

                _redisCacheService.RemoveKey(RedisKey.SuperAdminMembers, RedisDbType.User);
                _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.UserSession, (Guid)memberDto?.Id), RedisDbType.User);

                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            }
            catch (Exception exception)
            {
                description = $"عدم موفقیت در ثبت|ویرایش در اطلاعات حساب کاری توسط ادمین  « {_userContextDetector.UserContext.UserName} »  برای « {memberDto.FirstName}  {memberDto.LastName} - [ {memberDto.UserName} ] » ";
                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
                throw new ViolationSecurityPolicyException(exception.Message, "");
            }

            return result;
        }

        [RateLimit]
        [Description("ویرایش")]
        [HttpPost]
        [Route("api/member/EditUser", Name = "EditUser")]
        [JwtAuthorize(Permission = "SaveMember")]
        public async Task<MemberDto> EditUser(MemberDto memberDto)
        {
            MemberDto result = null;
            string description;
            _redisCacheService.RemoveKey(RedisKey.Permission + memberDto.Id, RedisDbType.Permission);
            UserManager.RemoveRedisObjects(memberDto.UserName);

            try
            {
                var user = _memberMapper.CreateFrom(memberDto);
                result = await _userManager.EditMemberAsync(user, memberDto.Password, GetSecretKey());
                var defaultDescription = $" ثبت کاربر جدید توسط ادمین « {_userContextDetector.UserContext.UserName} »  با مشخصات ، نام : « {memberDto.FirstName}  {memberDto.LastName} - نام کاربری : [ {memberDto.UserName} ] » با موفقیت انجام گرفت ";
                description = _userManager.GetTrackEntity() ?? defaultDescription;

                _redisCacheService.RemoveKey(RedisKey.SuperAdminMembers, RedisDbType.User);
                _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.UserSession, (Guid)memberDto?.Id), RedisDbType.User);

                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            }
            catch (Exception exception)
            {
                description = $"عدم موفقیت در ثبت|ویرایش در اطلاعات حساب کاری توسط ادمین  « {_userContextDetector.UserContext.UserName} »  برای « {memberDto.FirstName}  {memberDto.LastName} - [ {memberDto.UserName} ] » ";
                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
                throw new ViolationSecurityPolicyException(exception.Message, "");
            }

            return result;
        }
        [RateLimit]
        [Description("پذیرش درخواست عضویت کاربر توسط ادمین")]
        [HttpPost]
        [Route("api/member/AcceptRegisterRequestByAdmin", Name = "AcceptRegisterRequestByAdmin")]
        [JwtAuthorize(Permission = "SaveMember")]
        public Task<MemberDto> AcceptRegisterRequestByAdmin(MemberDto memberDto)
        {
            Task<MemberDto> result = null;
            _redisCacheService.RemoveKey(RedisKey.SuperAdminMembers, RedisDbType.User);
            UserManager.RemoveRedisObjects(memberDto.UserName);

            string description;
            try
            {
                var user = _memberMapper.CreateFrom(memberDto);
                result = _userManager.AcceptRegisterRequestByAdmin(user, memberDto.Password, GetSecretKey());
                description = $" درخواست کاربر جدید توسط ادمین « {_userContextDetector.UserContext.UserName} »  با مشخصات ، نام : « {memberDto.FirstName}  {memberDto.LastName} - نام کاربری : [ {memberDto.UserName} ] »  تایید شد ";

                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            }
            catch
            {
                description = $" عدم موفقیت در پذیرش کاربر جدید توسط ادمین  « {_userContextDetector.UserContext.UserName} »  برای  « {memberDto.FirstName}  {memberDto.LastName} - [ {memberDto.UserName} ] » ";
                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            }

            return result;
        }
        [RateLimit]
        [Description("تغییر وضعیت کاربر")]
        [HttpPost]
        [Route("api/member/ChangeUserStatus", Name = "UserStatus")]
        [JwtAuthorize(Permission = "SaveMember")]
        public Task<MemberDto> ChangeUserStatus(MemberDto memberDto)
        {
            var user = _memberMapper.CreateFrom(memberDto);
            var result = _userManager.ChangeUserStatus(user, GetSecretKey());

            var description = (memberDto.UserStatus == UserStatus.Active) ?
                $" حساب کاربری مسدود شده  {memberDto.UserName} با موفقیت فعال شد  " :
                $" حساب کاربری  {memberDto.UserName} مسدود شد  ";

            _redisCacheService.RemoveKey(RedisKey.SuperAdminMembers, RedisDbType.User);
            _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.UserSession, (Guid)memberDto?.Id), RedisDbType.User);
            UserManager.RemoveRedisObjects(memberDto.UserName);

            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            return result;
        }
        [RateLimit]
        [Description("تغییر سوپر ادمین")]
        [HttpPost]
        [Route("api/Member/ChangeSupperAdmin")]
        [JwtAuthorize(Permission = "SuperAdminManagement")]
        public Task<MemberDto> ChangeSupperAdmin(MemberDto memberDto)
        {
            var user = _memberMapper.CreateFrom(memberDto);
            var result = _userManager.ChangeSupperAdmin(user, GetSecretKey(), true);

            _redisCacheService.RemoveKey(RedisKey.SuperAdminMembers, RedisDbType.User);
            _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.UserSession, (Guid)memberDto?.Id), RedisDbType.User);
            UserManager.RemoveRedisObjects(memberDto.UserName);

            return result;
        }
        [RateLimit]
        [Description("دریافت لیست کاربران برای گرید")]
        [JwtAuthorizeAttribute(Permission = "ViewMember")]
        [HttpPost]
        [Route("api/member/GetForGrid", Name = "GetMemberForGrid")]
        public object GetMemberForGrid(SpecificationOfDataList<MemberDto> member)
        {
            Criteria criteria = member.GetCriteria() ?? CriteriaBuilder.CreateNew<MemberDto>();

            var userStatus = member.FilterSpecifications?.Find(x => x.PropertyName == "UserStatus");
            if (userStatus == null)
                criteria = criteria.And(CriteriaBuilder.CreateFromilterOperation<MemberDto>(FilterOperations.NotEqual,
                   "UserStatus", 1));

            var result = _userManager.GetOnePageOfMemberList(criteria, member.PageIndex, member.PageSize, member.GetSortItem());

            var enterprisesName = _enterprisePositionService.GetAllEnterprisePositionName();
            result.Data.Each(k => k.EnterprisePosition.Name = enterprisesName.ContainsKey((Guid)k.EnterprisePosition?.Id) ?
                enterprisesName[(Guid)k.EnterprisePosition?.Id] : k.EnterprisePosition.Name);

            return new
            {
                result.TotalCount,
                Results = result.Data
            };
        }
        [RateLimit]
        [Description("دریافت لیست کاربران برای گرید")]
        [HttpPost]
        [Route("api/member/GetForGridReport", Name = "GetMemberForGridReport")]
        [MachineAuthorize]
        public object GetMemberForGridReport(SpecificationOfDataList<MemberDto> member)
        {
            Criteria criteria = member.GetCriteria() ?? CriteriaBuilder.CreateNew<MemberDto>();

            var userStatus = member.FilterSpecifications?.Find(x => x.PropertyName == "UserStatus");
            if (userStatus == null)
                criteria = criteria.And(CriteriaBuilder.CreateFromilterOperation<MemberDto>(FilterOperations.NotEqual,
                   "UserStatus", 1));

            var result = _userManager.GetOnePageOfMemberList(criteria, member.PageIndex, member.PageSize, member.GetSortItem());

            var enterprisesName = _enterprisePositionService.GetAllEnterprisePositionName();
            result.Data.Each(k => k.EnterprisePosition.Name = enterprisesName.ContainsKey((Guid)k.EnterprisePosition?.Id) ?
                enterprisesName[(Guid)k.EnterprisePosition?.Id] : k.EnterprisePosition.Name);

            return new
            {
                result.TotalCount,
                Results = result.Data
            };
        }
        [RateLimit]
        [Description("خروج عادی کاربر")]
        [HttpPost]
        [Route("Logout", Name = "Logout")]
        [JwtAuthorize]
        public bool Logout(LogoutModel model)
        {
            //var accessToken = Request.Headers.Authorization.Parameter;
            var claimsIdentity = HttpContext.Current.GetOwinContext().Request.User.Identity as ClaimsIdentity;
            var accessToken = claimsIdentity.Claims.FirstOrDefault(x => x.Type == LabxandClaimTypes.AccessToken)?.Value ??
                Request.Headers.Authorization.Parameter;


            Task.Factory.StartNew(() =>
              {
                  _tokenService.InvalidateUserToken(accessToken);
                  _loginHistoryService.SetLogout(model.UserId, model.FingerPrint);
                  _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.Token, model.UserId), RedisDbType.User);

              });

            //Remove cookie from pipeline
            _authenticationManager.SignOut(KmsCookieOptions.AUTHENTICATION_TYPE);
            UserManager.RemoveRedisObjects(_userContextDetector.UserContext.UserName);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += $" -> کاربر {_userContextDetector.UserContext.UserName} از سامانه خارج شد";
            return true;
        }
        [RateLimit]
        [Description("تغییر رمز عبور توسط ادمین")]
        [HttpPost]
        [Route("api/member/ResetPasswordByAdmin", Name = "MemberChangePasswordByAdmin")]
        [JwtAuthorize(Permission = "SaveMember")]
        public Task<bool> ResetPasswordByAdmin(MemberDto memberDto)
        {
            Task<bool> result = null;
            string description;
            try
            {
                result = _userManager.ResetPasswordAsync(memberDto.Password, memberDto.Id);
                description = " رمز عبور حساب کاربری " + memberDto.UserName + "  توسط ادمین سیستم، با موفقیت تغییر کرد  ";
            }
            catch
            {
                description = "خطا در تغییر رمز عبور حساب کاربری  " + memberDto.UserName + " توسط ادمین سیستم  ";
            }

            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            UserManager.RemoveRedisObjects(memberDto.UserName);
            return result;
        }
        [RateLimit]
        [Description("خروج اجباری کاربر")]
        [HttpPost]
        [Route("api/member/ForceLogout", Name = "ForceLogout")]
        [JwtAuthorize]
        public bool ForceLogout(MemberDto memberDto)
        {
            _tokenService.InvalidateUserTokensByUserId(memberDto.Id);
            _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.Token, _userContextDetector.UserContext.MemberId), RedisDbType.User);

            var loginHistories = _loginHistoryService.GetLoginHistoriesWhichAreNotLogoutByUserId(memberDto.Id);
            if (loginHistories == null)
                return false;

            foreach (var item in loginHistories)
                _loginHistoryService.SetLogout(item.UserId, item.FingerPrint);

            _authenticationManager.SignOut(KmsCookieOptions.AUTHENTICATION_TYPE);
            string description = $" خروج اجباری به دلیل تغییر در ویژگی های امنیتی کاربر « {memberDto.UserName} » ";
            string reasonOfForceLogut = "";
            switch (memberDto.UserStatusValue)
            {
                case nameof(ForceLogoutType.ChangePassword):
                    reasonOfForceLogut = " - تغییر رمز عبور ";
                    break;
                case nameof(ForceLogoutType.BlockedIP):
                    reasonOfForceLogut = " - مسدود شدن ای پی آدرس ";
                    break;
                case nameof(ForceLogoutType.ChangeRolePermissions):
                    reasonOfForceLogut = " - تغییر سطح دسترسی نقش کاربر";
                    break;
                case nameof(ForceLogoutType.ChangeUserRole):
                    reasonOfForceLogut = " - تغییر نقش کاربر ";
                    break;
                case nameof(ForceLogoutType.BlockUser):
                    reasonOfForceLogut = " - مسدود شدن حساب کاربری ";
                    break;
                case nameof(ForceLogoutType.ChangeSecurityConfiguration):
                    reasonOfForceLogut = " - تغییر تنظیمات امنیتی";
                    break;
                case nameof(ForceLogoutType.TokenExpirationMinute):
                    reasonOfForceLogut = " - انقضای توکن";
                    break;
            }
            UserManager.RemoveRedisObjects(memberDto.UserName);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description + reasonOfForceLogut;

            return true;
        }
        [RateLimit]
        [Description("خروج اجباری کاربر")]
        [HttpPost]
        [Route("api/member/ForceLogoutWithCookie", Name = "ForceLogoutJwt")]
        public bool ForceLogoutWithCookie()
        {
            var claimsIdentity = HttpContext.Current.GetOwinContext()?.Request?.User?.Identity as ClaimsIdentity;
            var accessToken = claimsIdentity?.Claims?.FirstOrDefault(x => x.Type == LabxandClaimTypes.AccessToken)?.Value ??
                Request.Headers.Authorization?.Parameter;
            var userId = claimsIdentity?.Claims?.FirstOrDefault(x => x.Type == LabxandClaimTypes.UserId)?.Value.ToGuid();

            if (claimsIdentity == null || accessToken == null || userId == null)
                return true;

            _tokenService.InvalidateUserTokensByUserId((Guid)userId);
            _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.Token, _userContextDetector.UserContext.MemberId), RedisDbType.User);

            var loginHistories = _loginHistoryService.GetLoginHistoriesWhichAreNotLogoutByUserId((Guid)userId);
            if (loginHistories == null)
                return false;

            foreach (var item in loginHistories)
                _loginHistoryService.SetLogout(item.UserId, item.FingerPrint);

            _authenticationManager.SignOut(KmsCookieOptions.AUTHENTICATION_TYPE);
            var username = claimsIdentity?.Claims?.FirstOrDefault(x => x.Type == LabxandClaimTypes.UserName)?.Value;
            UserManager.RemoveRedisObjects(username);

            string description = $" خروج اجباری به دلیل تغییر در ویژگی های امنیتی کاربر « {username} » ";
            string reasonOfForceLogut = " - انقضای توکن";
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description + reasonOfForceLogut;

            return true;
        }
        [RateLimit]
        [Description("ثبت درخواست عضویت")]
        [HttpPost]
        [Route("api/member/RegisterRequest", Name = "MemberRegisterRequest")]
        public async Task<MemberDto> RegisterRequest(MemberDto memberDto)
        {
            memberDto.UserStatus = UserStatus.RegisterRequest;
            try
            {
                var user = _memberMapper.CreateFrom(memberDto);
                var result = await _userManager.RegisterRequest(user, memberDto.Password, GetSecretKey());
                _redisCacheService.RemoveKey(RedisKey.Notification + _userContextDetector.UserContext.MemberId, RedisDbType.Notification);
                _redisCacheService.RemoveKey(RedisKey.Permission + result.Id, RedisDbType.Permission);

                var description = $" درخواست عضویت توسط کاربر با نام [ {memberDto.FirstName} {memberDto.LastName} ] -  کد ملی {memberDto.UserName} ثبت شده است ";

                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
                return result;
            }
            catch (Exception ex)
            {
                var description = $" خطا در ثبت درخواست عضویت توسط کاربر با نام [ {memberDto.FirstName} {memberDto.LastName} ] -  کد ملی {memberDto.UserName}  ";
                ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
                throw new ViolationSecurityPolicyException(ex.Message, "");
            }
        }

        [RateLimit]
        [Description("تغییر رمز")]
        [HttpPost]
        [Route("api/member/ChangePassword")]
        [JwtAuthorize]
        public Task<bool> ChangePassword(ChangePasswordModel domain)
        {
            Task<bool> result = null;
            string description;
            try
            {
                if (domain.Password.Length < 8)
                    throw new Exception("پسورد باید حداقل 8 کاراکتر باشد");
                result = _userManager.ChangePasswordCurrentUser(domain.CurrentPassword, domain.Password);

                description = $" رمز عبور حساب کاربری « {_userContextDetector.UserContext.UserName} » تغییر کرد ";
            }
            catch
            {
                description = " عدم موفقیت در تغییر رمز عبور حساب کاربری  " + _userContextDetector.UserContext.UserName;
            }
            UserManager.RemoveRedisObjects(_userContextDetector.UserContext.UserName);
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
            return result;
        }
        [RateLimit]
        [Description("بازیابی رمز")]
        [HttpPost]
        [Route("api/member/RecoverPassword")]
        public Task<bool> RecoverPassword(RecoverPasswordModel model)
        {
            return _userManager.RecoverPassword(model.User, model.Password, model.Code);
        }
        [RateLimit]
        [Description("بروزرسانی اظلاعات شخصی")]
        [HttpPost]
        [Route("api/member/UpdatePersonalInfo")]
        [MachineAuthorize]
        public void UpdatePersonalInfo(MemberDto domain)
        {
            string description;
            try
            {
                _userManager.EditProfile(domain);
                description = $" ویرایش اطلاعات شخصی کاربر « {domain.UserName} » با موفقیت انجام گرفت  ";
                _redisCacheService.RemoveKey(RedisHelper.BuildKey(RedisKey.UserSession, (Guid)domain?.Id), RedisDbType.User);

                if (!string.IsNullOrEmpty(domain?.UserName))
                    UserManager.RemoveRedisObjects(domain.UserName);
            }
            catch
            {
                description = $" ویرایش اطلاعات شخصی کاربر « {domain.UserName} » با خطا مواجه شد ";
            }
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
        }
        [RateLimit]
        [Description("حذف ثیت درخواست عضویت")]
        [HttpPost]
        [Route("api/member/DeleteRegisterRequest")]
        [JwtAuthorizeAttribute(Permission = "SaveMember")]
        public void DeleteRegisterRequest(MemberDto member)
        {
            _userManager.DeleteRegisterRequest(member);
            var description = $" درخواست عضویت کاربر  « {member.FirstName}  {member.LastName} » - [ {member.UserName} ]  رد شد ";
            ObjectFactory.Current.GetInstance<ILogContext<ApiLogEntry>>().Current.Description += " -> " + description;
        }
        [Description("دریافت اطلاعات کاربر")]
        [HttpPost]
        [Route("api/member/GetWindowsUserInfo", Name = "WindowsUserInfo")]
        public object GetWindowsUserInfo()
        {
            var username = HttpContext.Current?.User?.Identity?.Name;

            if (username == null)
                return null;
            if (username.Contains("\\"))
                username = username.Split('\\')[1];

            return new
            {
                //user = _userManager.GetMember(username),
                permissions = UserManager.GetUserPermissions(_userManager.GetMember(username).Id)
            };
        }

        [HttpGet]
        [Route("api/member/GetMemberSessionInfo")]
        [JwtAuthorize]
        [CacheControl(10, true)]
        public MemberSessionInfoDto GetMemberSessionInfo()
        {
            var userSessionId = _userContextDetector.UserContext.MemberId;
            var userFromRedis = _redisCacheService.GetObject<MemberSessionInfoDto>(RedisHelper.BuildKey(RedisKey.UserSession, userSessionId), RedisDbType.User);
            if (userFromRedis == null)
            {
                var result = _userManager.GetMemberSessionInfoById(userSessionId);
                _redisCacheService.SetObject(RedisHelper.BuildKey(RedisKey.UserSession, result.Id), result, RedisDbType.User);
                return result;
            }

            return userFromRedis;
        }
    }
}