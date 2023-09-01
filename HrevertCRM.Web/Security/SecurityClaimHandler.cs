using System.Threading.Tasks;
using Hrevert.Common.Helpers;
using HrevertCRM.Data.QueryProcessors;
using Microsoft.AspNetCore.Authorization;

namespace HrevertCRM.Web.Security
{
    public class SecurityClaimHandler : AuthorizationHandler<SecurityRequirement>
    {
        private readonly ISecurityQueryProcessor _securityQueryProcessor;

        public SecurityClaimHandler(ISecurityQueryProcessor securityQueryProcessor)
        {
            _securityQueryProcessor = securityQueryProcessor;
        }
       
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext actionContext, SecurityRequirement requirement)
        {
            var principal = HttpHelper.HttpContext.User;
          //  var userId = User.Identity
            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Fail();
                return Task.FromResult<object>(null);
            }

            if (!_securityQueryProcessor.VerifyUserHasRight(requirement.RequiredSecurity))
            {
                //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, $"Action has been denied. Required security with Id of {SecurityId} ");
                actionContext.Fail();

                return Task.FromResult<object>(null);
            }
            actionContext.Succeed(requirement);
            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}