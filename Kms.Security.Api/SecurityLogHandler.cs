using Kms.Security.Api.App_Start.Ioc;
using Kms.Security.Core;
using LabXand.DistributedServices.WebApi;
using LabXand.Logging.Core;
using LabXand.Security.Core;

namespace Kms.Security.Api
{
    public class SecurityLogHandler : ApiLogHandler
    {
        public SecurityLogHandler(ILogger logger, ILogContext<ApiLogEntry> logContext) : base(logger, logContext)
        {

        }

        protected override void CustomizeLogEntryInit(ref ApiLogEntry apiLogEntry)
        {
            var userContextDetector = ObjectFactory.Current.GetInstance<IUserContextDetector<KmsUserContext>>();
            if (userContextDetector.UserContext == null)
            {
                apiLogEntry.User = "Unknown";
                return;
            };
            apiLogEntry.User = userContextDetector.UserContext.UserName;
            apiLogEntry.SupplementaryUserInformation = userContextDetector.UserContext.ToString();
        }
    }
}