using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using Kms.Security.Core;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Kms.Security.OpenId;
using Kms.Security.Util;
using LabXand.Core.ExceptionManagement;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Logging.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Kms.Security.Jwt
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAppJwtConfiguration _configuration;
        private readonly IApplicationUserManager _userManager;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ISecurityService _securityService;
        private readonly ILoginHistoryService _loginHistoryService;
        private readonly IUserProvider _userProvider;
        private readonly IClientIPAccessService _clientIPAccessService;
        private readonly ILogger _logger;
        private readonly ISecurityConfigurationContext _securityConfigurationContext;
        private readonly IRedisCacheService _cacheManager;

        public AppOAuthProvider(
              IUserProvider userProvider,
          IApplicationUserManager userManager,
           ITokenStoreService tokenStoreService,
              ISecurityService securityService,
          IAppJwtConfiguration configuration,
           ILoginHistoryService loginHistoryService,
           IClientIPAccessService clientIPAccessService,
           ILogger logger,
           ISecurityConfigurationContext securityConfigurationContext,
           IRedisCacheService cacheManager)
        {

            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentNull(nameof(_tokenStoreService));

            _securityService = securityService;
            _securityService.CheckArgumentNull(nameof(_securityService));

            _configuration = configuration;
            _configuration.CheckArgumentNull(nameof(_configuration));

            _userManager = userManager;
            _userProvider = userProvider;//for use other user manager service like kwp(abo bargh khozestan or sa'ante naft)
            _loginHistoryService = loginHistoryService;

            _clientIPAccessService = clientIPAccessService;
            _logger = logger;
            _securityConfigurationContext = securityConfigurationContext;
            _cacheManager = cacheManager;
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId != null)
            {
                context.Rejected();
                return Task.FromResult(0);
            }

            // Change authentication ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            var userId = new Guid(context.Ticket.Identity.FindFirst(LabxandClaimTypes.UserId).Value);
            //_usersService().UpdateUserLastActivityDate(userId);

            return Task.FromResult(0);
        }
        private ApiLogEntry CreateApiLogEntryWithRequestData(IOwinRequest request) =>
           new ApiLogEntry
           {
               Application = ConfigurationManager.AppSettings["ApplicationName"],
               ControllerName = "LoginController",
               ActionName = "Login",
               Machine = Environment.MachineName,
               RequestContentType = request.ContentType,
               RequestIpAddress = request.LocalIpAddress,
               RequestMethod = request.Method,
               RequestHeaders = SerializeHeaders(request.Headers),
               RequestTimestamp = DateTime.Now,
               RequestUri = request.Uri.ToString()
           };

        private string SerializeHeaders(IHeaderDictionary headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = string.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var form = await context.Request.ReadFormAsync();
                (string username, string password) = DecryptUsernameAndPassword(context, form);
                GuardAgainstLoginAttempts(username);
                GuardAgainstBlockedIp(username, context);
                CaptchaValidation(context, form);
                SetRipiToken(form);
                var user = _userProvider.GetUser(username);

                if (_userProvider.CheckInternalUser && user == null)
                    user = _userProvider.GetInternalUser(username, password);

                GuardAgainstInvalidUser(username, user);
                var fingerPrint = form["fingerPrint"];
                var securityConfiguration = _securityConfigurationContext.Instance;
                var result = _userProvider.VerifyPassword(password, user.PasswordHash);
                if (result != PasswordVerificationResult.Success)
                {
                    _loginHistoryService.Failed(user.Id, fingerPrint);
                    //----new -------
                    if (!user.IsSuperAdmin)
                    {
                        var maximumNumberOfFailedLogin = securityConfiguration.MaximumNumberOfFailedLogin;
                        var allowedTimeForReEntry = securityConfiguration.AllowedTimeForReEntry;
                        if (maximumNumberOfFailedLogin != 0 && allowedTimeForReEntry != 0)
                        {
                            //اینجا باید چک بشه اگر تایم زمانی گذشته از آخرین تلاش ناموفق اطلاعات ریست بشه
                            if (user.NumberOfFailedLogin != 0 && user.UserStatus != UserStatus.Block)
                            {
                                var diff = DateTime.Now.Subtract(new DateTime(user.LastTimeLoginFailed.Value.Year, user.LastTimeLoginFailed.Value.Month, user.LastTimeLoginFailed.Value.Day,
                                    user.LastTimeLoginFailed.Value.Hour, user.LastTimeLoginFailed.Value.Minute, user.LastTimeLoginFailed.Value.Second));
                                if (diff.TotalSeconds >= Convert.ToInt32(allowedTimeForReEntry))
                                {
                                    user.NumberOfFailedLogin = 0;
                                    user.LastTimeLoginFailed = null;
                                }
                            }

                            user.NumberOfFailedLogin++;
                            if (user.NumberOfFailedLogin < Convert.ToInt32(maximumNumberOfFailedLogin))
                                user.LastTimeLoginFailed = DateTime.Now;

                            //اگر دیگه برسه به اون تعداد دفعاتی که اشتباه زده کاربر غیر فعال میشه
                            else
                            {
                                user.UserStatus = UserStatus.Block;
                                _logger.Log(ApiLogEntryHandler.CreateApiLogEntry("Login",
                                    "GrantResourceOwnerCredentials",
                                    null,
                                    $" حساب کاربری [ {username} ] به علت تلاش غیرمجاز برای ورود به سامانه غیرفعال شد "));

                            }

                            using (var dbcontext = new ApplicationDbContext())
                            {
                                dbcontext.Set<ApplicationUser>().Attach(user);
                                dbcontext.Entry(user).State = EntityState.Modified;
                                dbcontext.SaveChanges();
                            }


                            var userMessage = string.Empty;
                            if ((Convert.ToInt32(maximumNumberOfFailedLogin) - user.NumberOfFailedLogin) != 0)
                                userMessage = "نام کاربری یا رمز ورود اشتباه است" + "(" + (Convert.ToInt32(maximumNumberOfFailedLogin) - user.NumberOfFailedLogin) + " مرتبه دیگر فرصت دارید " + ")";
                            else
                                userMessage = "حساب کاربری شما غیر فعال شد و تنها توسط ادمین سیستم قابلیت فعال سازی مجدد دارد. ";
                            throw new ViolationSecurityPolicyException(userMessage,
                                $"{user.NumberOfFailedLogin} امین تلاش ناموفق کاربر {username} برای ورود به سیستم");
                        }
                        else
                            throw new ViolationSecurityPolicyException("نام کاربری یا رمز ورود اشتباه است",
                                $" : تلاش ناموفق جهت ورود به سامانه به علت نام کاربری یا رمز عبور اشتباه با نام کاربری [ {username} ] ");
                    }
                    else
                        throw new ViolationSecurityPolicyException("نام کاربری یا رمز ورود اشتباه است",
                                $" : تلاش ناموفق جهت ورود به سامانه به علت نام کاربری یا رمز عبور اشتباه با نام کاربری [ {username} ] ");

                }
                else
                {
                    //اگر مقدار محاسبه توسط الگوریتم با مقدار موجود در دیتابیس یکی نباید یعنی پسورد از دیتابیس تغییر کرده است
                    if (ConfigurationManager.AppSettings["HashPasswordSHA256"] != null && PasswordHashSHA.CalculateLoginValue(user.PasswordHash) != user.LoginValue)
                    {
                        user.UserStatus = UserStatus.Block;
                        using (var dbcontext = new ApplicationDbContext())
                        {
                            dbcontext.Set<ApplicationUser>().Attach(user);
                            dbcontext.Entry(user).State = EntityState.Modified;
                            dbcontext.SaveChanges();
                        }
                        throw new ViolationSecurityPolicyException(
                            "رمز عبور کاربر به صورت غیرمجاز تغییر کرده است . این حساب کاربری تا زمان ریست شدن کلمه عبور توسط ادمین، قابل استفاده نمیباشد", "رمز عبور کاربر مذکور به شکل غیر سیستمی و غیرمجاز در پایگاه داده تغییر کرده است. این حساب تا زمان ریست شدن پسورد توسط ادمین به صورت غیرفعال خواهد بود. کاربر: " + username);
                    }
                }

                if (securityConfiguration.MaximumLoginAccount != 0 &&
                    !CheckMaximumLoginAccounts(user.Id).UserAllowed)
                    throw new ViolationSecurityPolicyException("در حال حاضر تعداد ورود به سیستم های شما از دستگاه های مختلف بیشتر از حد مجاز می باشد",
                            $"کاربر '{username}' به‌طور هم‌زمان در '{securityConfiguration.MaximumLoginAccount}' دستگاه وارد شده است. به‌همین دلیل تلاش او برای ورود جدید به سامانه با شکست مواجه شد ");
                _loginHistoryService.Success(user.Id, fingerPrint, securityConfiguration.ExpirationMinutes);
                var identity = SetClaimsIdentity(context, user);

                context.Validated(identity);

                _logger.Log(ApiLogEntryHandler.CreateApiLogEntry("Login",
                    "GrantResourceOwnerCredentials",
                    null,
                    $" {LoginType()} -> کاربر با نام کاربری {username} وارد سیستم شد "));

            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private static void GuardAgainstInvalidUser(string username, ApplicationUser user)
        {
            if (user == null)
                throw new ViolationSecurityPolicyException("نام کاربری یا رمز ورود اشتباه است.",
                    $" : تلاش ناموفق جهت ورود به سامانه به علت نام کاربری یا رمز عبور اشتباه با نام کاربری [ {username} ] ");

            if (user.UserStatus == UserStatus.RegisterRequest || user.UserStatus == UserStatus.Block)
                throw new ViolationSecurityPolicyException("حساب کاربری شما غیر فعال می باشد",
                 $" عدم موفقیت در ورود کاربر [ {username} ] به علت غیر فعال بودن حساب کاربری");
        }

        private void GuardAgainstBlockedIp(string username, OAuthGrantResourceOwnerCredentialsContext context)
        {
            var ip = context.Request.RemoteIpAddress.ToString();
            if (IsIpInBlackList(ip))
                throw new ViolationSecurityPolicyException("ای آپی آدرس شما مسدود شده است",
                     $" کاربر [ {username} ] با ای پی آدرس مسدود با مقدار [ {ip} ] اقدام به ورود کرد.");
        }

        private void GuardAgainstLoginAttempts(string username)
        {
            string cacheKey = RedisHelper.BuildKey(RedisKey.LoginAttempt, username);
            var lastLoginAttempt = _cacheManager.GetObject<LastLoginAttempt>(cacheKey, RedisDbType.LoginAttempts);

            if (lastLoginAttempt == null)
            {
                _cacheManager.SetObject(cacheKey, new LastLoginAttempt(username, DateTime.Now), RedisDbType.LoginAttempts, TimeSpan.FromSeconds(5));
            }
            else if ((DateTime.Now - lastLoginAttempt.LastAttemptDate).TotalSeconds < LoginAttemptConfig.LoginAttemptPeriod)
            {
                throw new ViolationSecurityPolicyException("فاصله زمانی درخواست ورود حداقل 5 ثانیه است.",
                     $" کاربر [ {username} ] با آهنگی سریعتر از مقدار تعیین شده، اقدام به ورود کرد.");
            }
            else
            {
                _cacheManager.RemoveKey(cacheKey, RedisDbType.LoginAttempts);
                _cacheManager.SetObject(cacheKey, new LastLoginAttempt(username, DateTime.Now), RedisDbType.LoginAttempts, TimeSpan.FromSeconds(5));
            }
        }

        private (string username, string password) DecryptUsernameAndPassword(OAuthGrantResourceOwnerCredentialsContext context, IFormCollection form)
        {
            var keyId = form["keyId"];
            if (string.IsNullOrEmpty(keyId) || string.IsNullOrWhiteSpace(keyId))
                ThrowInvalidKeyException();

            var privateKey = _cacheManager.GetObject<string>(RedisHelper.BuildKey(RedisKey.PrivateKey, keyId), RedisDbType.User);
            if (string.IsNullOrEmpty(privateKey) ||
                string.IsNullOrWhiteSpace(privateKey))
                ThrowInvalidKeyException();


            var username = RSAHelper.Decrypt(context.UserName, privateKey);
            var password = RSAHelper.Decrypt(context.Password, privateKey);
            return (username, password);
        }

        public void ThrowInvalidKeyException() => throw new ViolationSecurityPolicyException("مشکلی در پردازش فرآیند لاگین به‌وجود آمده است. لطفاً صفحه را رفرش کرده و دوباره امتحان کنید",
            " کلیدها از سمت کلاینت به درستی ارسال نشده است");


        public string LoginType()
            => _userProvider.GetType().Name == "UserProvider" ? "  ورود عادی" : " ورود از طریق اکتیودایرکتوری ";
        private void HandleException(OAuthGrantResourceOwnerCredentialsContext context, Exception ex)
        {
            IExceptionHandler handler = ExceptionHandlerFactory.GetSuitableHandler(ex);
            string message = handler.GetUserMessage(ex);
            string logDescription = ex is ViolationSecurityPolicyException ? ex.Message : message;
            var logEntry = ApiLogEntryHandler.CreateApiLogEntry("Login",
                                "GrantResourceOwnerCredentials",
                                null,
                                 logDescription, (int)handler.HttpCode);
            _logger.Log(logEntry);
            logEntry.ExceptionOccured(ex.Message, ExceptionInformation.CreateFromException(logEntry.Id, ex));
            context.SetError(message);
            context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });
        }

        public (bool UserAllowed, int NumberOfCuncurrentLogin) CheckMaximumLoginAccounts(Guid userId)
        {
            var securityConfiguration = _securityConfigurationContext.Instance;
            var result = _loginHistoryService.GetAll().Where(t => t.UserId == userId && t.IsSuccess == true &&
            DateTime.UtcNow < t.ExpireDateToken && t.LogOutDate == null).Count();

            var config = Convert.ToInt32(securityConfiguration.MaximumLoginAccount);
            if (result < config)
                return (true, result);
            else
                return (false, result);
        }
        public bool IsIpInBlackList(string ipAddress) => _clientIPAccessService.GetWithIP(ipAddress) != null;
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
                context.Validated();

            context.Validated();
            return Task.FromResult(0);
        }
        private ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {

            var identity = new ClaimsIdentity(authenticationType: KmsCookieOptions.AUTHENTICATION_TYPE);
            identity.AddClaim(new Claim(LabxandClaimTypes.FullName, user.FirstName + " " + user.LastName));
            identity.AddClaim(new Claim(LabxandClaimTypes.FirstName, user.FirstName));
            identity.AddClaim(new Claim(LabxandClaimTypes.LastName, user.LastName));
            identity.AddClaim(new Claim(LabxandClaimTypes.UserName, user.UserName));
            identity.AddClaim(new Claim(LabxandClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(LabxandClaimTypes.UserId, user.Id.ToString()));
            identity.AddClaim(new Claim(LabxandClaimTypes.EnterprisePositionId, user.EnterprisePositionId != null ? user.EnterprisePositionId.ToString() : ""));
            //identity.AddClaim(new Claim(LabxandClaimTypes.EnterprisePositionTitle, user.EnterprisePosition != null ? user.EnterprisePosition.Name : ""));
            identity.AddClaim(new Claim(LabxandClaimTypes.OrganizationId, user.OrganizationId != null ? user.OrganizationId.ToString() : ""));
            //identity.AddClaim(new Claim(LabxandClaimTypes.OrganizationTitle, user.Organization != null ? user.Organization.Name : ""));
            identity.AddClaim(new Claim(LabxandClaimTypes.UserStatus, ((int)user.UserStatus).ToString()));
            identity.AddClaim(new Claim(LabxandClaimTypes.IsSuperAdmin, user.IsSuperAdmin.ToString()));
            identity.AddClaim(new Claim(LabxandClaimTypes.RequirePasswordChangeStatus, ((int)user.RequirePasswordChangeStatus).ToString()));
            //var enterprisePositionPosts = user.EnterprisePositionPosts.Select(t => t.Id).ToList();
            //identity.AddClaim(new Claim(LabxandClaimTypes.EnterprisePositionPosts, JsonConvert.SerializeObject(enterprisePositionPosts)));
            identity.AddClaim(new Claim(LabxandClaimTypes.SerialNumber, user.SerialNumber));

            return identity;
        }
        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {

            var userId = new Guid(context.Identity.FindFirst(LabxandClaimTypes.UserId).Value.ToString());
            _tokenStoreService.UpdateUserToken(
               userId: userId,
               accessTokenHash: _securityService.GetSha256Hash(context.AccessToken)
               );

            var expireDuration = AuthenticationHelper.CalculateTokenExpireDuration(context.Properties.ExpiresUtc, context.Properties.IssuedUtc);
            _cacheManager.SetObject(RedisHelper.BuildKey(RedisKey.Token, userId), context.AccessToken, RedisDbType.User, expireDuration);

            //var userInfo = _userManager.GetMember(userId);
            //userInfo.IsSuperAdmin = bool.Parse(context.Identity.FindFirst(LabxandClaimTypes.IsSuperAdmin).Value.ToString());
            //context.AdditionalResponseParameters.Add("UserInfo", new JavaScriptSerializer().Serialize(userInfo));
            context.AdditionalResponseParameters.Add("IsSuperAdmin", 
                new JavaScriptSerializer().Serialize(context.Identity.FindFirst(LabxandClaimTypes.IsSuperAdmin).Value.ToString()));
            context.AdditionalResponseParameters.Add("RequirePasswordChangeStatus", 
                new JavaScriptSerializer().Serialize(context.Identity.FindFirst(LabxandClaimTypes.RequirePasswordChangeStatus).Value.ToString()));
            var permissions = UserManager.GetUserPermissions(userId);
            context.AdditionalResponseParameters.Add("Permissions", new JavaScriptSerializer().Serialize(permissions));

            context.Identity.AddClaim(new Claim(LabxandClaimTypes.AccessToken, context.AccessToken));

            context.OwinContext.Authentication.SignIn(new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = false,

            }, context.Identity);

            return base.TokenEndpointResponse(context);
        }

        private void CaptchaValidation(OAuthGrantResourceOwnerCredentialsContext context, IFormCollection form)
        {
            if (ConfigurationManager.AppSettings["ShowCaptcha"] == "false")
                return;

            var enteredCaptcha = form.FirstOrDefault(t => t.Key == "captchaText").Value?.FirstOrDefault();
            var captchaId = form.FirstOrDefault(t => t.Key == "captchaId").Value?.FirstOrDefault();
            if (enteredCaptcha.IsNullOrEmpty() || captchaId.IsNullOrEmpty())
                throw new ViolationSecurityPolicyException("مقدار کپچا صحیح نیست", $"ارسال مقدار کپچای ناصحیح با نام کاربری: [ {context.UserName} ]");

            var trueCaptchaText = _cacheManager.GetObject<string>(RedisKey.Captcha + captchaId, RedisDbType.LoginAttempts);
            _cacheManager.RemoveKey(RedisKey.Captcha + captchaId, RedisDbType.LoginAttempts);

            if (enteredCaptcha.ToLower() != trueCaptchaText.ToLower())
                throw new ViolationSecurityPolicyException("مقدار کپچا صحیح نیست", $"ارسال مقدار کپچای ناصحیح با نام کاربری: [ {context.UserName} ]");
        }
        private void SetRipiToken(IFormCollection form)
        {
            if (ConfigurationManager.AppSettings["CustomerName"] != "Ripi")
                return;

            _userProvider.RipiToken = form.Where(t => t.Key == "Token").FirstOrDefault().Value[0].ToString();
        }

    }
}
