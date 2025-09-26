using LabXand.Security.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace Kms.Security.WebApi
{
    public class KmsClaimAttribute : ClaimAttribute
    {
        protected override void CustomeAuthetication(HttpActionContext actionContext)
        {
            IEnumerable<string> headerValues = null;
            if (actionContext.Request.Headers.TryGetValues("authenticationToken", out headerValues))
            {
                string authenticationToken = Convert.ToString(headerValues.FirstOrDefault());
                SecurityHelper.LoginByToken(authenticationToken, true);
            }
            else
            {
                SecurityHelper.GetUserInfo(actionContext.RequestContext.Principal.Identity.Name);
            }
        }
    }
}
