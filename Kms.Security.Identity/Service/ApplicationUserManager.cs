using Kms.Security.Common.DataContract.Member;
using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Util;
using LabXand.Core;
using LabXand.DistributedServices;
using LabXand.DomainLayer;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.EF;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Logging.Core;
using LabXand.Security.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Kms.Security.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>, IApplicationUserManager, IKmsApplicationUserManager
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IUserContextDetector<KmsUserContext> _userContextDetector;
        private readonly IIdentityUnitOfWork _uow;
        private readonly IDbSet<ApplicationUser> _users;
        private readonly Lazy<Func<IIdentity>> _identity;
        private ApplicationUser _user;
        private readonly IEntityMapper<ApplicationUser, MemberDto> _memberMapper;
        private readonly IEntityMapper<LabxandRole, RoleDto> _roleMapper;
        private readonly ITokenStoreService _userTokenService;
        private readonly IOrganizationService _organizationService;
        private readonly IPhoneNumberValidator phoneNumberValidator;
        private readonly IEmailValidator emailValidator;
        private readonly IPersonnelNumberValidator personnelNumberValidator;
        private readonly IUsernameValidator usernameValidator;
        private readonly IRedisCacheService cacheManager;
        private readonly ISecurityConfigurationService _securityConfigurationService;
        public ApplicationUserManager(
            ICustomUserStore store,
            IUserContextDetector<KmsUserContext> userContextDetector,
            IIdentityUnitOfWork uow,
            IEntityMapper<ApplicationUser, MemberDto> memberMapper,
            IOrganizationService organizationService,
            IEntityMapper<LabxandRole, RoleDto> roleMapper,
            IPhoneNumberValidator phoneNumberValidator,
            IEmailValidator emailValidator,
            IPersonnelNumberValidator personnelNumberValidator,
            IUsernameValidator usernameValidator,
            IRedisCacheService cacheManager,
            ISecurityConfigurationService securityConfigurationService
            )

            : base((IUserStore<ApplicationUser, Guid>)store)
        {
            _memberMapper = memberMapper;
            _userContextDetector = userContextDetector;
            _uow = uow;
            _users = _uow.Set<ApplicationUser>();
            CreateApplicationUserManager();
            _userTokenService = new TokenStoreService(null, null, _uow, this);
            _organizationService = organizationService;
            _roleMapper = roleMapper;
            this.phoneNumberValidator = phoneNumberValidator;
            this.emailValidator = emailValidator;
            this.personnelNumberValidator = personnelNumberValidator;
            this.usernameValidator = usernameValidator;
            this.cacheManager = cacheManager;
            _securityConfigurationService = securityConfigurationService;
        }

        public ApplicationUser FindById(Guid userId)
        {
            return _users.Find(userId);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser applicationUser)
        {
            var userIdentity = await CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie).ConfigureAwait(false);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("user-email", applicationUser.Email));
            return userIdentity;
        }

        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return this.Users.ToListAsync();
        }
        public Task<List<ApplicationUser>> GetAllUsersAsyncWithRoles()
        {
            return this.Users.Include(x => x.LabxandRoles).ToListAsync();
        }
        public List<ApplicationUser> GetUsersAsyncWithRoles(Guid id)
        {
            return Users.Include(x => x.LabxandRoles).Where(t => t.Id == id).ToList();
        }
        public List<ApplicationUser> GetUsersAsyncWithRoles(int userStatus, Guid organizationId)
        {
            return Users.Include(x => x.LabxandRoles).Where(t => t.OrganizationId == organizationId && t.UserStatus == (UserStatus)userStatus).ToList();
        }
        public Task<List<ApplicationUser>> GetUsersAsyncWithRoles(int userStatus)
        {
            return Users.Include(x => x.LabxandRoles).Where(t => t.UserStatus == (UserStatus)userStatus).ToListAsync();
        }
        public ApplicationUser GetCurrentUser()
        {
            return _user ?? (_user = this.FindById(GetCurrentUserId()));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _user ?? (_user = await this.FindByIdAsync(GetCurrentUserId()).ConfigureAwait(false));
        }

        public Guid GetCurrentUserId()
        {
            return _identity.Value().GetUserId().ToGuid();
        }
        public async Task<bool> HasPassword(Guid userId)
        {
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            return user != null && user.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumber(Guid userId)
        {
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            return user != null && user.PhoneNumber != null;
        }

        public Func<CookieValidateIdentityContext, Task> OnValidateIdentity()
        {
            return SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                         validateInterval: TimeSpan.FromSeconds(0),
                         regenerateIdentityCallback: (manager, user) => manager.GenerateUserIdentityAsync(user),
                         getUserIdCallback: claimsIdentity => claimsIdentity.GetUserId().ToGuid());
        }

        private void CreateApplicationUserManager()
        {
            this.UserValidator = new UserValidator<ApplicationUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            this.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, Guid>
            {
                MessageFormat = "Your security code is: {0}"
            });
            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, Guid>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            if (_dataProtectionProvider != null)
            {
                var dataProtector = _dataProtectionProvider.Create("ASP.NET Identity");
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtector);
            }
        }

        Task<Guid> IApplicationUserManager.GetAccessFailedCountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }


        public async Task<MemberDto> CreateMemberAsync(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false)
        {
            user.PasswordHash = PasswordHasher.HashPassword(password);
            if (!isSuperAdminChanged)
                ValidateData(user);

            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();
            user.LastUserPasswordChange = null;
            user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToUserRegistration;
            var result = await base.CreateAsync(user, password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }
        public async Task<MemberDto> RegisterRequest(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false)
        {
            if (!isSuperAdminChanged)
                ValidateDataWithoutPassword(user);

            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();
            user.LastUserPasswordChange = null;
            user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToUserRegistration;
            var result = await base.CreateAsync(user, password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }

        public async Task<MemberDto> ChangeSupperAdmin(ApplicationUser user, string secretKey, bool isSuperAdminChanged = false)
        {
            if (!isSuperAdminChanged)
                ValidateDataWithoutPassword(user);

            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();

            var result = await base.CreateAsync(user, null).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }

        public async Task<MemberDto> EditMemberAsync(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false)
        {
            if (!isSuperAdminChanged)
                ValidateDataWithoutPassword(user);

            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();
            user.LastUserPasswordChange = null;
            user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToUserRegistration;
            var result = await base.CreateAsync(user, password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }

        public async Task<MemberDto> AcceptRegisterRequestByAdmin(ApplicationUser user, string password, string secretKey, bool isSuperAdminChanged = false)
        {
            if (!isSuperAdminChanged)
                ValidateDataWithoutPassword(user);

            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();
            user.LastUserPasswordChange = null;
            user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToUserRegistration;
            var result = await base.CreateAsync(user, password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }

        public async Task<MemberDto> ChangeUserStatus(ApplicationUser user, string secretKey, bool isSuperAdminChanged = false)
        {
            var isUserExist = this.FindById(user.Id) != null;
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            else if (isUserExist)
            {
                EditUser(user, isSuperAdminChanged, secretKey);
                return null;
            }
            if (user.EnterprisePositionPosts != null)
            {
                var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
                user.EnterprisePositionPosts = _uow.Set<EnterprisePositionPost>().Where(t => enterprisePositionPosts.Contains(t.Id)).ToList();
            }

            if (user.LabxandRoles != null)
            {
                var roles = user.LabxandRoles.Select(t => t.Id).ToList();
                user.LabxandRoles = _uow.Set<LabxandRole>().Where(t => roles.Contains(t.Id)).ToList();
            }

            if (user.UserStatus == 0)
                user.UserStatus = UserStatus.Active;

            user.SerialNumber = Guid.NewGuid().ToString();

            var result = await base.CreateAsync(user, null).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var findedUserById = base.FindByIdAsync(user.Id);
                SaveUserInMemberOfKmsApi(user, secretKey);
                return _memberMapper.MapTo(findedUserById.Result);
            }
            else
            {
                string error = "";
                foreach (var item in result.Errors)
                {
                    error += item + Environment.NewLine;
                }
                throw new Exception(error);
            }

        }

        private void SaveUserInMemberOfKmsApi(ApplicationUser user, string accessToken)
        {
            var link = WebConfigurationManager.AppSettings["ServiceRoot"] + "/api/Member12/Save";
            try
            {
                if (string.IsNullOrEmpty(link) || string.IsNullOrWhiteSpace(link))
                    throw new InvalidOperationException("Invalid Create User Link");

                HttpUtil.PerformHttpPost(link,
                    JsonConvert.SerializeObject(user.Id.ToString()), "Application/Json",
                    new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>(AuthenticationHelper.Authorization, accessToken) ,
                        new KeyValuePair<string, string>("Username", string.IsNullOrEmpty(_userContextDetector.UserContext.UserName) ? user.UserName : _userContextDetector.UserContext.UserName),
                    });
            }
            catch (Exception ex)
            {
                new FileLogger().Log("on Performing user Save in api -->" + user.UserName + " \n" + ex.Message + "\n\n");
            }

        }
        public List<MemberDto> GetAll()
        {
            var list = this.GetAllUsersAsync();
            return list.Result.Select(p => _memberMapper.MapTo(p)).ToList();
        }

        public List<MemberDto> GetAllWithRoles()
        {
            var list = this.GetAllUsersAsyncWithRoles();
            return list.Result.Select(p => _memberMapper.MapTo(p)).ToList();
        }

        public Paginated<MemberDto> GetOnePageOfMemberList(Criteria criteria, int page, int pageSize, List<SortItem> sortItems)
        {
            var claimsIdentity = (System.Web.HttpContext.Current.User.Identity as ClaimsIdentity);
            var userOrganizationId = _userContextDetector.UserContext.OrganizationId;
            var allChildren = new List<Guid>();
            if (_userContextDetector.UserContext.IsSuperAdmin)
                allChildren = _organizationService.GetAllChild(userOrganizationId).Select(p => p.Id).ToList();
            allChildren.Add(userOrganizationId);
            Expression<Func<ApplicationUser, bool>> expression = p => true;

            if (criteria != null)
            {
                expression = ExpressionHelper.CreateFromCriteria<ApplicationUser>(criteria);
            }
            using (var db = new ApplicationDbContext())
            {
                var result = db.Set<ApplicationUser>().Include(e => e.EnterprisePosition).Include(p => p.Organization).Include(p => p.LabxandRoles).Include(q => q.EnterprisePositionPosts).Where(expression).Where(p => allChildren.Contains(p.OrganizationId.Value));
                var sortItemQuery = SortItemHelper.GetQueryable(sortItems, result);
                var pageListQuery = new PagedList<ApplicationUser>(sortItemQuery, page, pageSize);
                PagedList<MemberDto> pMember = new PagedList<MemberDto>(pageListQuery.Select(p => _memberMapper.MapTo(p)).ToList(), page, pageSize, pageListQuery.TotalCount);
                return new Paginated<MemberDto>(pMember);
            }

        }

        public ApplicationUser GetCurrentKmsUser()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            return _user ?? (_user = this.FindById(new Guid(userId)));
        }
        public Guid GetJwtUserId()
        {
            var claimsIdentity = (System.Web.HttpContext.Current.User.Identity as ClaimsIdentity);
            return new Guid(claimsIdentity.FindFirst(LabxandClaimTypes.UserId).Value);
        }
        public Guid GetMemberIdFromHeader() => HttpContext.Current.Request.Headers["MemberId"].ToGuid();
        private string _trackEntity;
        public void EditUser(ApplicationUser user, bool changeIsAdmin, string secretKey)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                if (changeIsAdmin && !user.IsSuperAdmin)
                {
                    var supperAdmin = dbContext.Users.FirstOrDefault(p => p.IsSuperAdmin == true && p.Id != user.Id) ??
                        throw new ViolationServiceRuleException("سوپر ادمین تنها یک کاربر می باشد و امکان حذف یا غیرفعالسازی وجود ندارد");
                }

                if (user.IsSuperAdmin && (user.UserStatus == UserStatus.Block || user.UserStatus == UserStatus.Disable))
                    throw new ViolationServiceRuleException("امکان غیر فعال سازی سوپر ادمین وجود ندارد");

                var temp = dbContext.Set<ApplicationUser>().Include(p => p.LabxandRoles).Include(p => p.EnterprisePositionPosts).FirstOrDefault(p => p.Id == user.Id);
                if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Keycloak)
                    temp.UserName = user.UserName;

                if (temp.LabxandRoles.Any(r => r.Name == "admin") && (user.UserStatus != UserStatus.Active || !user.LabxandRoles.Any(lr => lr.Name == "admin")))
                {
                    var admins = dbContext.Users.Where(u => u.LabxandRoles.Any(r => r.Name == "admin")).ToList();
                    if (admins.Count < 2)
                        throw new ViolationServiceRuleException("ادمین تنها یک کاربر می باشد و امکان حذف یا غیرفعالسازی وجود ندارد");
                }

                temp.LastName = user.LastName;
                temp.FirstName = user.FirstName;
                temp.UserStatus = user.UserStatus;
                temp.LastDeactivationDate = user.LastDeactivationDate;
                temp.PhoneNumber = user.PhoneNumber;
                temp.PersonnelNumber = user.PersonnelNumber;
                temp.Email = user.Email;
                if (changeIsAdmin)
                    temp.IsSuperAdmin = user.IsSuperAdmin;
                var labxandRoleId = user.LabxandRoles.Select(t => t.Id).ToList();
                if (temp.LabxandRoles != null)
                    temp.LabxandRoles.Clear();

                if (labxandRoleId.Count > 0)
                    temp.LabxandRoles = dbContext.Set<LabxandRole>().Where(t => labxandRoleId.Any(y => y.Equals(t.Id))).ToList();

                var ids = user.EnterprisePositionPosts?.Select(y => y.Id).ToList();
                if (temp.EnterprisePositionPosts != null)
                    temp.EnterprisePositionPosts.Clear();

                if (ids != null && ids.Count > 0)
                {
                    temp.EnterprisePositionPosts = dbContext.Set<EnterprisePositionPost>().Where(t => ids.Any(y => y.Equals(t.Id))).ToList();
                    //foreach (var item in temp.EnterprisePositionPosts)
                    //{
                    //    dbContext.Entry<EnterprisePositionPost>(item).State = EntityState.Unchanged;
                    //}
                }
                temp.OrganizationId = user.OrganizationId;
                temp.EnterprisePositionId = user.EnterprisePositionId;
                //dbContext.Users.Attach(temp);
                _trackEntity = dbContext.StringifyDbContextChanges();
                dbContext.SaveAllChanges();

                if (AuthenticationHelper.GetAuthenticationMode() == Util.AuthenticationMode.Kms)
                    _userTokenService.InvalidateUserTokens(user.Id);

                SaveUserInMemberOfKmsApi(user, secretKey);
            }
        }

        public string GetTrackEntity()
        {
            return _trackEntity ?? null;
        }

        public void EditProfile(MemberDto member)
        {
            var memberId = GetMemberIdFromHeader();
            using (var dbContext = new ApplicationDbContext())
            {
                var user = dbContext.Users.Where(t => t.Id == memberId).FirstOrDefault();
                user.FirstName = member.FirstName;
                user.LastName = member.LastName;
                user.Email = member.Email;
                user.PhoneNumber = member.CellphoneNumber;
                user.PersonnelNumber = member.PersonnelNumber;
                dbContext.SaveAllChanges();
            }
        }

        public async Task<bool> ResetPasswordAsync(string newPassword, Guid userId)
        {
            var user = this._uow.Set<ApplicationUser>()
                .Include(p => p.LabxandRoles)
                .Include(p => p.EnterprisePositionPosts)
                .FirstOrDefault(p => p.Id == userId);

            var validate = await PasswordValidator.ValidateAsync(newPassword);
            if (validate.Succeeded)
            {
                // Create a SHA256      
                if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null)
                {
                    user.PasswordHash = PasswordHashSHA.HashPassword(newPassword);
                    user.LoginValue = PasswordHashSHA.CalculateLoginValue(user.PasswordHash);
                }
                //old
                else
                    user.PasswordHash = PasswordHasher.HashPassword(newPassword);
                user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToAdminChange;
                var result = await UpdateAsync(user);
                //if (!result.Succeeded)
                //{
                //    //throw new Exception("خطا در تغییر پسورد");
                //}
            }
            else
                throw new Exception("\n پسورد معتبر نیست" + validate.Errors.FirstOrDefault());

            _userTokenService.InvalidateUserTokens(userId);
            return true;
        }

        public async Task<bool> ChangePasswordCurrentUser(string password, string newPassword)
        {
            var userId = GetJwtUserId();

            var user = _uow.Set<ApplicationUser>()
                .Include(p => p.LabxandRoles)
                .Include(p => p.EnterprisePositionPosts)
                .FirstOrDefault(p => p.Id == userId);

            ValidateCurrentPassword(password, user);
            ValidatePasswordDifference(password, newPassword);
            var validate = await PasswordValidator.ValidateAsync(newPassword);
            if (validate.Succeeded)
            {
                // Create a SHA256    
                if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null)
                {
                    user.PasswordHash = PasswordHashSHA.HashPassword(newPassword);
                    user.LoginValue = PasswordHashSHA.CalculateLoginValue(user.PasswordHash);
                }
                //old
                else
                    user.PasswordHash = PasswordHasher.HashPassword(newPassword);
                user.RequirePasswordChangeStatus = RequirePasswordChangeStatus.NotRequired;
                user.LastUserPasswordChange = DateTime.Now;
                var result = await UpdateAsync(user);
                //if (!result.Succeeded)
                //{
                //    //throw new Exception("خطا در تغییر پسورد");
                //}
            }
            else
                throw new ViolationServiceRuleException("\n پسورد معتبر نیست" + validate.Errors.FirstOrDefault());

            _userTokenService.InvalidateUserTokens(user.Id);
            return true;
        }

        private void ValidateCurrentPassword(string currentPassword, ApplicationUser user)
        {
            var isCurrentPasswordValid = ConfigurationManager.AppSettings["HashPasswordSHA256"] != null ?
                PasswordHashSHA.VerifyPassword(user.PasswordHash, currentPassword) :
                PasswordHasher.VerifyHashedPassword(user.PasswordHash, currentPassword);

            if (isCurrentPasswordValid != PasswordVerificationResult.Success)
                throw new ViolationServiceRuleException("رمز عبور فعلی صحیح نیست");

        }
        private void ValidatePasswordDifference(string currentPassword, string newPassword)
        {
            if (string.Equals(currentPassword, newPassword, StringComparison.Ordinal))
                throw new ViolationServiceRuleException("رمز عبور جدید و فعلی برابر هستند");
        }

        public async Task<bool> RecoverPassword(string user, string password, string code)
        {
            if (!IsCodeValid(user, code))
                return false;

            cacheManager.RemoveKey(RedisKey.RecoverCode + user, RedisDbType.RecoverPassword);

            try
            {
                var member = _uow.Set<ApplicationUser>().Include(p => p.LabxandRoles).FirstOrDefault(p => p.PersonnelNumber == user) ?? _uow.Set<ApplicationUser>().Include(p => p.LabxandRoles).FirstOrDefault(p => p.UserName == user);
                var validate = PasswordValidator.ValidateAsync(password);
                if (validate.Result.Succeeded)
                {
                    // Create a SHA256    
                    if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null)
                    {
                        member.PasswordHash = PasswordHashSHA.HashPassword(password);
                        member.LoginValue = PasswordHashSHA.CalculateLoginValue(member.PasswordHash);
                    }
                    else
                        member.PasswordHash = PasswordHasher.HashPassword(password);
                    member.LastUserPasswordChange = DateTime.Now;
                    member.RequirePasswordChangeStatus = RequirePasswordChangeStatus.NotRequired;
                    var result = await UpdateAsync(member);
                    if (!result.Succeeded)
                    {
                        throw new Exception("خطا در تغییر پسورد");
                    }
                }
                else
                    throw new Exception("\n پسورد معتبر نیست" + validate.Result.Errors.FirstOrDefault());

                cacheManager.RemoveKey(RedisKey.Token + member.Id, RedisDbType.User);
                cacheManager.RemoveKey(RedisKey.UserSession + member.Id, RedisDbType.User);
                cacheManager.RemoveKey(RedisKey.ApplicationUser + member.UserName, RedisDbType.User);
                cacheManager.RemoveKey(RedisKey.ApplicationUserWithDetail + member.UserName, RedisDbType.User);

                _userTokenService.InvalidateUserTokens(member.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsCodeValid(string user, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            var sendedCode = cacheManager.GetObject<string>(RedisKey.RecoverCode + user, RedisDbType.RecoverPassword);
            if (string.IsNullOrWhiteSpace(sendedCode))
                return false;

            if (sendedCode == code)
                return true;
            else
                return false;
        }

        public IList<ApplicationUser> GetUserWithRoleId(Guid roleId)
        {
            return _uow.Set<ApplicationUser>().Include(p => p.LabxandRoles).Where(x => x.LabxandRoles.Any(t => t.Id == roleId)).ToList();
        }
        public IList<ApplicationUser> GetUserWithRoleName(string roleName)
        {
            return _uow.Set<ApplicationUser>().Include(p => p.LabxandRoles).Where(x => x.LabxandRoles.Any(t => t.Name.Equals(roleName))).ToList();
        }
        public IQueryable<ApplicationUser> GetUser()
        {
            return _uow.Set<ApplicationUser>().Include(p => p.LabxandRoles);
        }

        public MemberDto GetMember(Expression<Func<ApplicationUser, bool>> predict)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var usersQuery = dbcontext.Users
                    .Include(p => p.EnterprisePosition)
                    .Include(p => p.EnterprisePositionPosts)
                    .Include(p => p.Organization)
                    .Include(p => p.LabxandRoles);
                if (predict != null)
                    usersQuery = usersQuery.Where(predict);
                return _memberMapper.MapTo(usersQuery.FirstOrDefault());
            }
        }

        public MemberDto GetMemberWithRole(Expression<Func<ApplicationUser, bool>> predict)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var usersQuery = dbcontext.Users
                    .Include(p => p.LabxandRoles);
                if (predict != null)
                    usersQuery = usersQuery.Where(predict);
                return _memberMapper.MapTo(usersQuery.FirstOrDefault());
            }
        }
        public MemberDto GetMemberWithRole(string username)
        {
            return GetMemberWithRole(t => t.UserName == username);
        }

        public MemberDto GetMember(Guid userId)
        {
            return GetMember(p => p.Id == userId);
        }
        public MemberDto GetMember(string username)
        {
            return GetMember(p => p.UserName == username);
        }

        public ApplicationUser GetUserWithUsernameForWebApi(string userName)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                ApplicationUser user = dbcontext.Users
                    .Include(p => p.EnterprisePosition)
                    .Include(p => p.EnterprisePositionPosts)
                    .Include(p => p.Organization)
                    .Include(p => p.LabxandRoles)
                    .FirstOrDefault(p => p.UserName == userName);
                return user;
            }
        }

        public void DeleteRegisterRequest(MemberDto member)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var user = dbcontext.Users.FirstOrDefault(t => t.Id == member.Id);
                if (user == null)
                    throw new Exception("کاربر مورد نظر وجود ندارد");
                if (user.UserStatus != UserStatus.RegisterRequest)
                    throw new Exception("امکان حذف کاربر مورد نظر وجود ندارد");
                dbcontext.Users.Remove(user);
                dbcontext.SaveAllChanges();
            }
        }

        public MemberDto CreateMember(MemberDto member, string accessToekn)
        {
            var user = _memberMapper.CreateFrom(member);
            var result = this.CreateMemberAsync(user, member.Password, accessToekn);
            return result.Result;
        }

        public string GeneratePasswordResetToken(Guid userId)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var user = dbcontext.Users.FirstOrDefault(t => t.Id == userId);
                if (user == null)
                    throw new Exception("خطا در انجام عملیات");
                user.SerialNumber = Guid.NewGuid().ToString();
                var token = CryptoService.Encrypt(userId + "" + Guid.NewGuid());
                dbcontext.SaveAllChanges();
                return token;
            }
        }
        public void ResetPassword(Guid userId, string token, string newPassword)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var deToken = CryptoService.Decrypt(token);

                var user = dbContext.Users.FirstOrDefault(p => p.Id == userId);
                if (user == null)
                    throw new Exception("خطا در انجام عملیات");
                if (user.Id + user.SerialNumber != deToken)
                    throw new Exception("خطا در انجام عملیات");
                // Create a SHA256    
                if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null)
                {
                    user.PasswordHash = PasswordHashSHA.HashPassword(newPassword);
                    user.LoginValue = PasswordHashSHA.CalculateLoginValue(user.PasswordHash);
                }
                //old
                else
                    user.PasswordHash = PasswordHasher.HashPassword(newPassword);
                user.SerialNumber = Guid.NewGuid().ToString();
                dbContext.SaveAllChanges();
            }
        }

        public Claim[] CasUserProccess(string userName)
        {
            var user = GetUser().FirstOrDefault(x => x.UserName == userName);

            return SetClaims(_memberMapper.MapTo(user));
        }

        public Claim[] KeycloakUserProccess(MemberDto keycloakUser, string secretKey)
        {
            var user = GetUser().FirstOrDefault(x => x.Id == keycloakUser.Id);
            MemberDto result;
            if (user == null)
            {
                //TOOD 
                //Where TO get Organization
                keycloakUser.OrganizationId = _organizationService.GetRoot().Id;
                keycloakUser.RegisterationDate = DateTime.Now;
                keycloakUser.Password = keycloakUser.UserName + "@123456";
                //var role = _roleService.GetAll().FirstOrDefault(t => t.Name == "Daneshakr" || t.Title == "دانشکار");
                keycloakUser.Roles = new List<RoleDto>
                {
                    GetInitialRole()
                };
                result = CreateMember(keycloakUser, secretKey);
            }
            else
            {
                if (IsKeycloakUserChanged(keycloakUser, user))
                {
                    user.UserName = keycloakUser.UserName;
                    user.FirstName = keycloakUser.FirstName;
                    user.LastName = keycloakUser.LastName;
                    user.Email = keycloakUser.Email;
                    EditUser(user, false, secretKey);
                }
                result = _memberMapper.MapTo(user);
            }

            return SetClaims(result);
        }
        private bool IsKeycloakUserChanged(MemberDto keycloakUser, ApplicationUser dbUser)
        {
            if (string.Compare(keycloakUser.UserName, dbUser.UserName, true) != 0 ||
                string.Compare(keycloakUser.FirstName, dbUser.FirstName, true) != 0 ||
                string.Compare(keycloakUser.LastName, dbUser.LastName, true) != 0 ||
                string.Compare(keycloakUser.Email, dbUser.Email, true) != 0)
                return true;
            return false;

        }
        private Claim[] SetClaims(MemberDto user)
        {
            //var identity = new ClaimsIdentity(authenticationType: "JWT");
            var enterprisePositionPosts = user.EnterprisePositionPosts?.Select(t => t.Id).ToList();
            return new[]
            {
                    new Claim(LabxandClaimTypes.UserName, user.UserName),
                    new Claim(LabxandClaimTypes.FullName, user.FullName),
                    new Claim(LabxandClaimTypes.FirstName, user.FirstName),
                    new Claim(LabxandClaimTypes.LastName, user.LastName),
                    new Claim(LabxandClaimTypes.Email, user.Email),
                    new Claim(LabxandClaimTypes.UserId, user.Id.ToString()),
                    new Claim(LabxandClaimTypes.EnterprisePositionId, user.EnterprisePositionId != null ? user.EnterprisePositionId.ToString() : ""),
                    new Claim(LabxandClaimTypes.OrganizationId, user.OrganizationId != null ? user.OrganizationId.ToString() : ""),
                    new Claim(LabxandClaimTypes.UserStatus, ((int)user.UserStatus).ToString()),
                    new Claim(LabxandClaimTypes.IsSuperAdmin, user.IsSuperAdmin.ToString()),
                    new Claim(LabxandClaimTypes.EnterprisePositionPosts, JsonConvert.SerializeObject(enterprisePositionPosts))

            };
        }
        // due to : [on injecting role service ]
        // Bi-directional dependency relationship detected 
        private RoleDto GetInitialRole()
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var role = dbcontext.LabxandRoles.FirstOrDefault(t => t.Name == "Daneshakr" || t.Title == "دانشکار");
                return _roleMapper.MapTo(role);
            }
        }

        public List<MemberDto> GetAllWithEntAndOrg()
        {
            using (var db = new ApplicationDbContext())
            {
                var result = db.Set<ApplicationUser>().Include(t => t.EnterprisePosition).Include(t => t.Organization).Include(t => t.LabxandRoles);
                return result.ToList().Select(t => _memberMapper.MapTo(t)).ToList();
            }
        }

        public void ValidateData(ApplicationUser applicationUser)
        {
            if (string.IsNullOrWhiteSpace(applicationUser.FirstName) || string.IsNullOrWhiteSpace(applicationUser.LastName))
                throw new ArgumentNullException("نام و نام خانوادگی اجباری است.");

            if (string.IsNullOrWhiteSpace(applicationUser.PasswordHash))
                throw new ArgumentNullException("وارد کردن رمز عبور اجباری است");

            phoneNumberValidator.ValidatePhoneNumber(applicationUser.PhoneNumber);
            emailValidator.ValidateEmailAddress(applicationUser.Email);
            personnelNumberValidator.ValidatePersonnelNumber(applicationUser.PersonnelNumber);
            usernameValidator.ValidateUsername(applicationUser.UserName);
            CheckRepetition(applicationUser);
        }

        public void ValidateDataWithoutPassword(ApplicationUser applicationUser)
        {
            if (string.IsNullOrWhiteSpace(applicationUser.FirstName) || string.IsNullOrWhiteSpace(applicationUser.LastName))
                throw new ArgumentNullException("نام و نام خانوادگی اجباری است.");

            phoneNumberValidator.ValidatePhoneNumber(applicationUser.PhoneNumber);
            emailValidator.ValidateEmailAddress(applicationUser.Email);
            personnelNumberValidator.ValidatePersonnelNumber(applicationUser.PersonnelNumber);
            usernameValidator.ValidateUsername(applicationUser.UserName);
            CheckRepetition(applicationUser);
        }

        public void CheckRepetition(ApplicationUser user)
        {
            Expression<Func<ApplicationUser, bool>> expression;
            if (ConfigurationManager.AppSettings["CustomerName"] == "Behdasht")
                expression = u => (u.Id != user.Id) && (
            u.PhoneNumber == user.PhoneNumber ||
            u.Email == user.Email ||
            u.UserName == user.UserName);
            else
                expression = u => (u.Id != user.Id) && (
            u.PhoneNumber == user.PhoneNumber ||
            u.Email == user.Email ||
            u.PersonnelNumber == user.PersonnelNumber ||
            u.UserName == user.UserName);

            var usersWithSameData = Users.Where(expression).ToList();

            if (usersWithSameData.Count == 0)
                return;

            if (usersWithSameData.Where(u => u.PhoneNumber == user.PhoneNumber).Count() > 0)
                throw new SecurityException("این شماره تلفن برای کاربر دیگری ثبت شده است.");

            if (usersWithSameData.Where(u => u.Email == user.Email).Count() > 0)
                throw new SecurityException("این ایمیل برای کاربر دیگری ثبت شده است.");

            if (usersWithSameData.Where(u => u.PersonnelNumber == user.PersonnelNumber).Count() > 0 && ConfigurationManager.AppSettings["CustomerName"] != "Behdasht")
                throw new SecurityException("این شماره پرسنلی برای کاربر دیگری ثبت شده است.");

            if (usersWithSameData.Where(u => u.UserName == user.UserName).Count() > 0)
                throw new SecurityException("این نام کاربری برای کاربر دیگری ثبت شده است.");
        }

        public MemberSessionInfoDto GetMemberSessionInfoById(Guid id)
        {
            using (var dbcontext = new ApplicationDbContext())
            {
                var user = dbcontext.Users
                    .Include(x => x.LabxandRoles)
                    .FirstOrDefault(x => x.Id == id);
                var mapper = (MemberMapper)_memberMapper;
                return mapper.MapToMemberSessionInfoDto(user);
            }
        }

        public void ProcessPasswordExpiry()
        {
            var securityConfiguration = _securityConfigurationService.GetSecurityConfiguration();
            if (securityConfiguration.PasswordChangeDaysPeriod < 1)
                return;
            var the30DaysEarlier = DateTime.Now.AddDays(-securityConfiguration.PasswordChangeDaysPeriod);
            using (var dbcontext = new ApplicationDbContext())
            {
                var usersWithExpirationPassword = dbcontext.Users
                      .Where(x => x.LastUserPasswordChange.HasValue &&
                                  x.LastUserPasswordChange < the30DaysEarlier &&
                                  x.RequirePasswordChangeStatus == RequirePasswordChangeStatus.NotRequired)
                      .ToList();

                if (usersWithExpirationPassword.Any())
                {
                    usersWithExpirationPassword.ForEach(x => x.RequirePasswordChangeStatus = RequirePasswordChangeStatus.DueToExpiration30Days);
                    dbcontext.SaveChanges();
                }

            };
        }
    }
}