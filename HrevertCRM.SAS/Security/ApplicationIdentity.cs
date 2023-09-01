using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HrevertCRM.SAS.Security
{
    public static class ApplicationIdentity
    {
        public static void UseErpIdentity(this IApplicationBuilder app)
        {
            var applicationCookie = new CookieAuthenticationOptions
            {
                
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieName = "HrevertERP",
                LoginPath = new PathString("/login"),
                AccessDeniedPath = new PathString("/forbidden"),

            Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync,
                    //OnRedirectToLogin = ctx =>
                    //{
                    //    // If request coming from web api
                    //    // always return Unauthorized (401)
                    //    if (ctx.Request.Path.StartsWithSegments("/api") &&
                    //        ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    //    {
                    //        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    //    }
                    //    else
                    //    {
                    //        ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    //    }

                    //    return Task.FromResult(0);
                    //}
                },

                // Set to 1 hour
                ExpireTimeSpan = TimeSpan.FromHours(1),

                // Notice that if value = false, 
                // you can use angular-cookies ($cookies.get) to get value of Cookie
                CookieHttpOnly = true
            };

            IdentityOptions identityOptions = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
            identityOptions.Cookies.ApplicationCookie = applicationCookie;

            app.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
            app.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);
        }
    }
}
