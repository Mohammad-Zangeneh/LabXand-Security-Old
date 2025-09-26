using Kms.Security.Common.DataContract;
using Kms.Security.Core;
using Kms.Security.WebApi;
using LabXand.Logging.Core;
using LabXand.Logging.LogService;
using LabXand.Security.Core;
using LabXand.Infrastructure.Data.Redis;
using StructureMap;
using Kms.Security.Identity.Service.Contracts;
using Kms.Security.Identity.Service;
using Kms.Security.Util;

namespace Kms.Security.Api.App_Start.Ioc
{
    public class GlobalIocRegistry : Registry
    {
        public GlobalIocRegistry()
        {
            For<ILogger>().Use<ElasticLogger<KmsUserContext>>();
            For<ILogContext>().Use<LogContext<ApiLogEntry>>();
            Forward<ILogContext, ILogContext<ApiLogEntry>>();
            For<IUserContextDetector<KmsUserContext>>().Use<KmsApiUserContextDetector>();
            For<IRedisCacheService>().Use<RedisCacheService>();
            For<IRedisStore>().Use<RedisStore>();
            For<ISecurityConfigurationReader>().Use<SecurityConfigurationRedisReader>();
            For<ISecurityConfigurationContext>().Use<SecurityConfigurationContext>().Singleton();
            For<IZarService>().Use<ZarService>();
            For<IPhoneNumberValidator>().Use<PhoneNumberValidator>();
            For<IEmailValidator>().Use<EmailValidator>();
            For<IPersonnelNumberValidator>().Use<PersonnelNumberValidator>();
            For<IUsernameValidator>().Use<UsernameValidator>();
        }
    }
}