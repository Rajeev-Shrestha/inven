using Microsoft.AspNetCore.Authorization;

namespace HrevertCRM.Web.Security
{
    public class SecurityRequirement : IAuthorizationRequirement
    {
        public SecurityRequirement(int security)
        {
            RequiredSecurity = security;
        }

        public int RequiredSecurity { get; set; }
    }
}