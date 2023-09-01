using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Hrevert.Common.Helpers;
using Hrevert.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Entities;
using HrevertCRM.Web.Security;
using HrevertCRM.Web.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HrevertCRM.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

       public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            Authorization.Configure(services);
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 6;

                config.Cookies.ApplicationCookie.Events =
           new CookieAuthenticationEvents
           {
               OnRedirectToLogin = context =>
               {
                   if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK || 
                        (context.Request.Path.StartsWithSegments("/api") 
                        && context.Response.StatusCode == (int)HttpStatusCode.Unauthorized))
                       context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                   else
                       context.Response.Redirect(context.RedirectUri);

                   return Task.CompletedTask;
               },
               OnRedirectToAccessDenied = context =>
               {
                   if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK ||
                        (context.Request.Path.StartsWithSegments("/api") 
                        && context.Response.StatusCode == (int)HttpStatusCode.Unauthorized))
                       context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                   else
                       context.Response.Redirect(context.RedirectUri);

                   return Task.CompletedTask;
               }
           };
            })
               .AddEntityFrameworkStores<DataContext>()
               .AddDefaultTokenProviders();
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSingleton<IPagedDataRequestFactory, PagedDataRequestFactory>();
            services.AddScoped<IDbContext, DataContext>();
            services.AddScoped<IProductQueryProcessor, ProductQueryProcessor>();
            services.AddScoped<ICompanyQueryProcessor, CompanyQueryProcessor>();
            services.AddScoped<IProductCategoryQueryProcessor, ProductCategoryQueryProcessor>();
            services.AddScoped<IUserQueryProcessor, UserQueryProcessor>();
            services.AddScoped<IProductInCategoryQueryProcessor, ProductInCategoryQueryProcessor>();
            services.AddScoped<IFiscalYearQueryProcessor, FiscalYearQueryProcessor>();
            services.AddScoped<ISecurityQueryProcessor, SecurityQueryProcessor>();
            services.AddSingleton<IAuthorizationHandler, SecurityClaimHandler>();
            services.AddSingleton<IUserSession, UserSession>();
            services.AddScoped<ISecurityGroupQueryProcessor, SecurityGroupQueryProcessor>();
            services.AddScoped<ISecurityGroupMemberQueryProcessor, SecurityGroupMemberQueryProcessor>();
            services.AddScoped<ISecurityRightQueryProcessor, SecurityRightQueryProcessor>();
            services.AddScoped<IAddressQueryProcessor, AddressQueryProcessor>();
            services.AddScoped<IFiscalPeriodQueryProcessor, FiscalPeriodQueryProcessor>();
            services.AddScoped<ICustomerQueryProcessor, CustomerQueryProcessor>();
            services.AddScoped<ICustomerLevelQueryProcessor, CustomerLevelQueryProcessor>();
            services.AddScoped<IPaymentTermQueryProcessor, PaymentTermQueryProcessor>();
            services.AddScoped<IDeliveryMethodQueryProcessor, DeliveryMethodQueryProcessor>();
            services.AddScoped<ICustomerContactGroupQueryProcessor, CustomerContactGroupQueryProcessor>();
            services.AddScoped<ICustomerInContactGroupQueryProcessor, CustomerInContactGroupQueryProcessor>();
            services.AddScoped<IPaymentMethodQueryProcessor, PaymentMethodQueryProcessor>();
            services.AddScoped<IAccountQueryProcessor, AccountQueryProcessor>();
            services.AddScoped<ISalesOrderQueryProcessor, SalesOrderQueryProcessor>();
            services.AddScoped<IPurchaseOrderQueryProcessor, PurchaseOrderQueryProcessor>();
            services.AddScoped<ITaxQueryProcessor, TaxQueryProcessor>();
            services.AddScoped<ISalesOrderLineQueryProcessor, SalesOrderLineQueryProcessor>();
            services.AddScoped<IPurchaseOrderLineQueryProcessor, PurchaseOrderLineQueryProcessor>();
            services.AddScoped<IVendorQueryProcessor, VendorQueryProcessor>();
            services.AddScoped<IEmailSettingQueryProcessor, EmailSettingQueryProcessor>();
            services.AddScoped<IJournalMasterQueryProcessor, JournalMasterQueryProcessor>();
            services.AddScoped<IAccountQueryProcessor, AccountQueryProcessor>();
            services.AddScoped<IUserSettingQueryProcessor, UserSettingQueryProcessor>();
            services.AddScoped<IShoppingCartQueryProcessor, ShoppingCartQueryProcessor>();
            services.AddScoped<IShoppingCartDetailQueryProcessor, ShoppingCartDetailQueryProcessor>();
            services.AddScoped<IProductMetadataQueryProcessor, ProductMetadataQueryProcessor>();
            services.AddScoped<IEmailSenderQueryProcessor, EmailSenderQueryProcessor>();
            services.AddScoped<ICustomerLoginEventQueryProcessor, CustomerLoginEventQueryProcessor>();
            services.AddScoped<IEcommerceSettingQueryProcessor, EcommerceSettingQueryProcessor>();
            services.AddScoped<ICarouselQueryProcessor, CarouselQueryProcessor>();
            services.AddScoped<IDeliveryRateQueryProcessor, DeliveryRateQueryProcessor>();
            services.AddScoped<IDeliveryZoneQueryProcessor, DeliveryZoneQueryProcessor>();
            services.AddScoped<IItemMeasureQueryProcessor, ItemMeasureQueryProcessor>();
            services.AddScoped<IMeasureUnitQueryProcessor, MeasureUnitQueryProcessor>();
            services.AddScoped<IProductPriceRuleQueryProcessor, ProductPriceRuleQueryProcessor>();
            services.AddScoped<ICompanyWebSettingQueryProcessor, CompanyWebSettingQueryProcessor>();
            services.AddScoped<IDiscountQueryProcessor, DiscountQueryProcessor>();
            services.AddScoped<IFeaturedItemQueryProcessor, FeaturedItemQueryProcessor>();
            services.AddScoped<IFeaturedItemMetadataQueryProcessor, FeaturedItemMetadataQueryProcessor>();
            services.AddScoped<IDashboardQueryProcessor, DashboardQueryProcessor>();
            services.AddScoped<IBugLoggerQueryProcessor, BugLoggerQueryProcessor>();
            services.AddScoped<ITaxesInProductQueryProcessor, TaxesInProductQueryProcessor>();
            services.AddScoped<ICompanyLogoQueryProcessor, CompanyLogoQueryProcessor>();
            services.AddScoped<ITaskManagerQueryProcessor, TaskManagerQueryProcessor>();
            services.AddScoped<ISalesOpportunityQueryProcessor, SalesOpportunityQueryProcessor>();
            services.AddScoped<IGradeQueryProcessor, GradeQueryProcessor>();
            services.AddScoped<ISourceQueryProcessor, SourceQueryProcessor>();
            services.AddScoped<IReasonClosedQueryProcessor, ReasonClosedQueryProcessor>();
            services.AddScoped<IStageQueryProcessor, StageQueryProcessor>();
            services.AddScoped<IHeaderSettingQueryProcessor, HeaderSettingQueryProcessor>();
            services.AddScoped<IGeneralSettingQueryProcessor, GeneralSettingQueryProcessor>();
            services.AddScoped<IStoreLocatorQueryProcessor, StoreLocatorQueryProcessor>();
            services.AddScoped<IBrandImageQueryProcessor, BrandImageQueryProcessor>();
            services.AddScoped<IFooterSettingQueryProcessor, FooterSettingQueryProcessor>();
            services.AddScoped<ISlideSettingQueryProcessor, SlideSettingQueryProcessor>();
            services.AddScoped<IIndividualSlideSettingQueryProcessor, IndividualSlideSettingQueryProcessor>();
            services.AddScoped<ILayoutSettingQueryProcessor, LayoutSettingQueryProcessor>();

            #region Enumeration Services

            services.AddScoped<IAccountCashFlowTypesQueryProcessor, AccountCashFlowTypesQueryProcessor>();
            services.AddScoped<IAccountDetailTypesQueryProcessor, AccountDetailTypesQueryProcessor>();
            services.AddScoped<IAccountLevelTypesQueryProcessor, AccountLevelTypesQueryProcessor>();
            services.AddScoped<IAccountTypesQueryProcessor, AccountTypesQueryProcessor>();
            services.AddScoped<IAddressTypesQueryProcessor, AddressTypesQueryProcessor>();
            services.AddScoped<IDescriptionTypesQueryProcessor, DescriptionTypesQueryProcessor>();
            services.AddScoped<IDiscountTypesQueryProcessor, DiscountTypesQueryProcessor>();
            services.AddScoped<IDueDateTypesQueryProcessor, DueDateTypesQueryProcessor>();
            services.AddScoped<IDueTypesQueryProcessor, DueTypesQueryProcessor>();
            services.AddScoped<IEncryptionTypesQueryProcessor, EncryptionTypesQueryProcessor>();
            services.AddScoped<IJournalTypesQueryProcessor, JournalTypesQueryProcessor>();
            services.AddScoped<ILockTypesQueryProcessor, LockTypesQueryProcessor>();
            services.AddScoped<IMediaTypesQueryProcessor, MediaTypesQueryProcessor>();
            services.AddScoped<IPaymentDiscountTypesQueryProcessor, PaymentDiscountTypesQueryProcessor>();
            services.AddScoped<IPurchaseOrdersStatusQueryProcessor, PurchaseOrdersStatusQueryProcessor>();
            services.AddScoped<IPurchaseOrderTypesQueryProcessor, PurchaseOrderTypesQueryProcessor>();
            services.AddScoped<ISalesOrdersStatusQueryProcessor, SalesOrdersStatusQueryProcessor>();
            services.AddScoped<ISalesOrderTypesQueryProcessor, SalesOrderTypesQueryProcessor>();
            services.AddScoped<ISuffixTypesQueryProcessor, SuffixTypesQueryProcessor>();
            services.AddScoped<ITaxCalculationTypesQueryProcessor, TaxCalculationTypesQueryProcessor>();
            services.AddScoped<ITermTypesQueryProcessor, TermTypesQueryProcessor>();
            services.AddScoped<ITitleTypesQueryProcessor, TitleTypesQueryProcessor>();
            services.AddScoped<IUserTypesQueryProcessor, UserTypesQueryProcessor>();
            services.AddScoped<IEntryMethodTypeQueryProcessor, EntryMethodTypeQueryProcessor>();
            services.AddScoped<IDiscountCalculationTypeQueryProcessor, DiscountCalculationTypeQueryProcessor>();
            services.AddScoped<IShippingCalculationTypeQueryProcessor, ShippingCalculationTypeQueryProcessor>();
            services.AddScoped<IShippingStatusQueryProcessor, ShippingStatusQueryProcessor>();
            services.AddScoped<IImageTypeQueryProcessor, ImageTypeQueryProcessor>();
            services.AddScoped<ICountryQueryProcessor, CountryQueryProcessor>();
            #endregion


            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            //services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
            //services.AddScoped<IWebContextFactory, WebContextFactory>();
            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc(cofigure =>
            {
               // cofigure.Filters.AddService(typeof(UnitOfWorkActionFilterAttribute));
            });

            //services.AddAutoMapper();
            //services.AddTransient<IProfileService, IdentityWithAdditionalClaimsProfileService>();
            services.AddIdentityServer()
               .AddTemporarySigningCredential()
               .AddInMemoryPersistedGrants()
               .AddInMemoryScopes(Config.GetScopes())
               .AddInMemoryClients(Config.GetClients())
               .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<IdentityWithAdditionalClaimsProfileService>();
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();
           app.UseIdentity();
            //   app.UseIdentityServer();
            // app.UseErpIdentity();
            var HOST_URL = Configuration.GetSection("APISettings")["HOST_URL"];
            app.UseIdentityServer();
             JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions
            {
                Authority = HOST_URL + "/",
                ScopeName = "apis",
                ScopeSecret = "secret",
                AutomaticAuthenticate = true,
                SupportedTokens = SupportedTokens.Both,
                // TokenRetriever = _tokenRetriever,
                // required if you want to return a 403 and not a 401 for forbidden responses
                AutomaticChallenge = true, 
                RequireHttpsMetadata = false
            };

            app.UseIdentityServerAuthentication(identityServerValidationOptions);
            app.UseMvc(config =>
                {
                    config.MapRoute("actionroute", "{action}",
                        new { controller = "App", action = "Index" }
                        );

                    config.MapRoute(
                        name: "Default",
                         template: "{controller=App}/{action=Index}/{id?}"
                    );
                   

                });
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    //serviceScope.ServiceProvider.GetService<DataContext>()
                        //.Database.Migrate();

                    var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                    var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();
                    var signInManager = app.ApplicationServices.GetService<SignInManager<ApplicationUser>>();

                    serviceScope.ServiceProvider.GetService<DataContext>().EnsureSeedData(userManager, roleManager, signInManager, env.ContentRootPath);
                }
            }
            catch(Exception)
            {
                //return (ex.Message);
            }
          //  app.ApplicationServices.GetRequiredService<DataContext>().Seed(userManager);
        }
    }
}
