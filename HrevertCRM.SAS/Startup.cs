using HrevertCRM.SAS.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.SAS
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton<ApiConfigurationService>();
            services.AddSingleton<IConfiguration>(_ => Configuration);
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookiesHCRMStore",
                CookieName = "CookiesHCRMStore",
                LoginPath = new PathString("/login"),
                AccessDeniedPath = new PathString("/forbidden"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true

            });

            //var options = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    LoginPath = new PathString("/login"),
            //    LogoutPath = new PathString("/logout"), 
            //    AuthenticationScheme = options.Cookies.ApplicationCookieAuthenticationScheme,
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    Events = new CookieAuthenticationEvents
            //    {
            //        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            //    }
            //});
            //app.UseStatusCodePagesWithReExecute("/error/{0}");
            //app.UseHttpException();
            app.UseErpIdentity();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseMvc(config =>
            {

                config.MapRoute("actionroute", "{action}", new { controller = "Home", action = "Index" }
                );

                config.MapRoute(
                    name: "Default",
                     template: "{controller=Home}/{action=Index}/{id?}"
                );


            });


        }
        //public static class ApplicationBuilderExtensions
        //{
        //    public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        //    {
        //        return application.UseMiddleware<HttpExceptionMiddleware>();
        //    }
        //}
    }
}
