using LabXand.Core.ExceptionManagement;
using System;
using System.Net;

namespace Kms.Security.Jwt
{
    public class ViolationSecurityPolicyException : Exception
    {
        public ViolationSecurityPolicyException(string userMessage, string message) : base(message)
        {
            UserMessage = userMessage;
        }
        public string UserMessage { get; set; }
    }

    public class ViolationSecurityPolicyExceptionHandler : CustomExceptionHandlerBase<ViolationSecurityPolicyException>
    {
        public override HttpStatusCode HttpCode => HttpStatusCode.PreconditionFailed;
        public override string GetUserMessage(Exception exception) =>
            ((ViolationSecurityPolicyException)exception).UserMessage;
    }
}
