using Microsoft.Extensions.DependencyInjection;

namespace HrevertCRM.Web.Security
{
    public static class Authorization
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(SecurityDefinition.SecurityDictionary[SecurityId.ViewProducts],
                //policy => policy.Requirements.Add(new SecurityRequirement((int)SecurityId.ViewProducts)));
            });
        }

     
    }
}
