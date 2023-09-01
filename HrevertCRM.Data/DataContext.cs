#region Using Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Mappings;
using HrevertCRM.Data.Mappings.ECommerce;
using HrevertCRM.Data.Mappings.Enumerations;
using HrevertCRM.Data.Mappings.ThemeSettings;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.Seed;
using HrevertCRM.Entities;
using HrevertCRM.Entities.Enumerations;
using HrevertCRM.Web.SeedValues;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShippingStatus = HrevertCRM.Entities.Enumerations.ShippingStatus;
using UserType = HrevertCRM.Common.UserType;

#endregion

namespace HrevertCRM.Data
{
    public class DataContext : BaseContext, IDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IDbContext _context;

        public DataContext(DbContextOptions<DataContext> options, IServiceProvider serviceProvider,
            IUserSession userSession)
            : base(userSession, serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region DbSet Codes

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<SalesManager> SalesManagers { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityRight> SecurityRights { get; set; }
        public DbSet<SecurityGroupMember> SecurityGroupMembers { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<FiscalPeriod> FiscalPeriods { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLevel> CustomerLevels { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<CustomerInContactGroup> CustomerInContactGroups { get; set; }
        public DbSet<CustomerContactGroup> CustomerContactGroups { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<CustomerLoginEvent> CustomerLoginEvents { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<JournalMaster> JournalMasters { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetail> ShoppingCartDetails { get; set; }
        public DbSet<CompanyWebSetting> CompanyWebSettings { get; set; }
        public DbSet<ProductMetadata> ProductMetadatas { get; set; }
        public DbSet<EmailSender> Emails { get; set; }
        public DbSet<EcommerceSetting> EcommerceSettings { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<ItemCount> ItemCounts { get; set; }
        public DbSet<DeliveryRate> DeliveryRates { get; set; }
        public DbSet<ItemMeasure> ItemMeasures { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<DeliveryZone> DeliveryZones { get; set; }
        public DbSet<PayMethodsInPayTerm> PayMethodsInPayTerms { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<FeaturedItem> FeaturedItems { get; set; }
        public DbSet<FeaturedItemMetadata> FeaturedItemMetadatas { get; set; }
        public DbSet<ProductsRefByKitAndAssembledType> ProductsRefByKitAndAssembledTypes { get; set; }
        public DbSet<BugLogger> BugLoggers { get; set; }
        public DbSet<TaxesInProduct> TaxesInProducts { get; set; }
        public DbSet<CompanyLogo> CompanyLogos { get; set; }
        public DbSet<TaskManager> TaskManagers { get; set; }
        public DbSet<SalesOpportunity> SalesOpportunities { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<ReasonClosed> ReasonClosed { get; set; }
        public DbSet<BrandImage> BrandImages { get; set; }
        public DbSet<FooterSetting> FooterSettings { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<HeaderSetting> HeaderSettings { get; set; }
        public DbSet<IndividualSlideSetting> IndividualSlideSettings { get; set; }
        public DbSet<LayoutSetting> LayoutSettings { get; set; }
        public DbSet<PersonnelSetting> PersonnelSettings { get; set; }
        public DbSet<SlideSetting> SlildeSettings { get; set; }
        public DbSet<StoreLocator> StoreLocators { get; set; }


        #region Enum DbSets

        public DbSet<AccountCashFlowTypes> AccountCashFlowTypes { get; set; }
        public DbSet<AccountDetailTypes> AccountDetailTypes { get; set; }
        public DbSet<AccountLevelTypes> AccountLevelTypes { get; set; }
        public DbSet<AccountTypes> AccountTypes { get; set; }
        public DbSet<AddressTypes> AddressTypes { get; set; }
        public DbSet<DescriptionTypes> DescriptionTypes { get; set; }
        public DbSet<DiscountTypes> DiscountTypes { get; set; }
        public DbSet<DueDateTypes> DueDateTypes { get; set; }
        public DbSet<DueTypes> DueTypes { get; set; }
        public DbSet<EncryptionTypes> EncryptionTypes { get; set; }
        public DbSet<JournalTypes> JournalTypes { get; set; }
        public DbSet<LockTypes> LockTypes { get; set; }
        public DbSet<MediaTypes> MediaTypes { get; set; }
        public DbSet<PaymentDiscountTypes> PaymentDiscountTypes { get; set; }
        public DbSet<PurchaseOrdersStatus> PurchaseOrdersStatus { get; set; }
        public DbSet<PurchaseOrderTypes> PurchaseOrderTypes { get; set; }
        public DbSet<SalesOrdersStatus> SalesOrdersStatus { get; set; }
        public DbSet<SalesOrderTypes> SalesOrderTypes { get; set; }
        public DbSet<SuffixTypes> SuffixTypes { get; set; }
        public DbSet<TaxCalculationTypes> TaxCalculationTypes { get; set; }
        public DbSet<TermTypes> TermTypes { get; set; }
        public DbSet<TitleTypes> TitleTypes { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<EntryMethodTypes> EntryMethodTypes { get; set; }
        public DbSet<ShippingStatus> ShippingStatus { get; set; }
        public DbSet<ShippingCalculationTypes> ShippingCalculationTypes { get; set; }
        public DbSet<DiscountCalculationTypes> DiscountCalculationTypes { get; set; }
        public DbSet<Country> Countries { get; set; }

        #endregion


        #endregion,

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(
                "Server=RAJEEV\\SERVER; MultipleActiveResultSets=true; Initial Catalog=HrevertCRM.Data; User Id=sa;Password=software",
                //"Data Source=SQL5035.SmarterASP.NET;Initial Catalog=DB_A0A20A_hREVER;User Id=DB_A0A20A_hREVER_admin;Password=p@77w0rd;",
                //"Server=DESKTOP-T2LKBQK; MultipleActiveResultSets=true; Initial Catalog=HrevertCRM.Data; Integrated Security = True",
                //"Server=tfs; MultipleActiveResultSets=true; Initial Catalog=HrevertCRM.Data; user id=sa; password=p@77w0rd",
                // "Server=SESHAT; MultipleActiveResultSets=true;Initial Catalog=HrevertCRM.Data;  user id=sa; password=p@77w0rd",
                //@"Server=(localdb)\MSSQLLocalDB; MultipleActiveResultSets=true; Initial Catalog=HrevertCRM.Data; user id=sa; password=p@77w0rd",
                b => b.MigrationsAssembly("HrevertCRM.Web"));
        }
                                                            
        protected override void OnModelCreating(ModelBuilder builder)   
        {
            base.OnModelCreating(builder);
            new CompanyMapping(builder.Entity<Company>());
            new ProductMapping(builder.Entity<Product>());
            new UserMapping(builder.Entity<ApplicationUser>());
            new ProductCategoryMapping(builder.Entity<ProductCategory>());
            new ProductInCategoryMapping(builder.Entity<ProductInCategory>());
            new FiscalYearMapping(builder.Entity<FiscalYear>());
            new SalesOrderMapping(builder.Entity<SalesOrder>());
            new PurchaseOrderMapping(builder.Entity<PurchaseOrder>());
            new SalesManagerMapping(builder.Entity<SalesManager>());
            new SecurityMapping(builder.Entity<Security>());
            new SecurityGroupMapping(builder.Entity<SecurityGroup>());
            new SecurityRightMapping(builder.Entity<SecurityRight>());
            new SecurityGroupMemberMapping(builder.Entity<SecurityGroupMember>());
            new TransactionLogMapping(builder.Entity<TransactionLog>());
            new ProductMetadataMapping(builder.Entity<ProductMetadata>());
            new AddressMapping(builder.Entity<Address>());
            new FiscalPeriodMapping(builder.Entity<FiscalPeriod>());
            new CustomerInContactGroupMapping(builder.Entity<CustomerInContactGroup>());
            new CustomerContactGroupMapping(builder.Entity<CustomerContactGroup>());
            new CustomerMapping(builder.Entity<Customer>());
            new CustomerLevelMapping(builder.Entity<CustomerLevel>());
            new PaymentTermMapping(builder.Entity<PaymentTerm>());
            new DeliveryMethodMapping(builder.Entity<DeliveryMethod>());
            new PaymentMethodMapping(builder.Entity<PaymentMethod>());
            new SalesOrderLineMapping(builder.Entity<SalesOrderLine>());
            new PurchaseOrderLineMapping(builder.Entity<PurchaseOrderLine>());
            new AccountMapping(builder.Entity<Account>());
            new TaxMapping(builder.Entity<Tax>());
            new VendorMapping(builder.Entity<Vendor>());
            new EmailSettingMapping(builder.Entity<EmailSetting>());
            new JournalMasterMapping(builder.Entity<JournalMaster>());
            new ShoppingCartMapping(builder.Entity<ShoppingCart>());
            new ShoppingCartDetailMapping(builder.Entity<ShoppingCartDetail>());
            new ProductMetadataMapping(builder.Entity<ProductMetadata>());
            new EmailSenderMapping(builder.Entity<EmailSender>());
            new EcommerceSettingMapping(builder.Entity<EcommerceSetting>());
            new CarouselMapping(builder.Entity<Carousel>());
            new DeliveryZoneMapping(builder.Entity<DeliveryZone>());
            new DeliveryRateMapping(builder.Entity<DeliveryRate>());
            new ItemMeasureMapping(builder.Entity<ItemMeasure>());
            new MeasureUnitMapping(builder.Entity<MeasureUnit>());
            new ProductPriceRuleMapping(builder.Entity<ProductPriceRule>());
            new CompanyWebSettingMapping(builder.Entity<CompanyWebSetting>());
            new DiscountMapping(builder.Entity<Discount>());
            new FeaturedItemMapping(builder.Entity<FeaturedItem>());
            new FeaturedItemMetadataMapping(builder.Entity<FeaturedItemMetadata>());
            new ProductsRefByKitAndAssembledTypeMapping(builder.Entity<ProductsRefByKitAndAssembledType>());
            new BugLoggerMapping(builder.Entity<BugLogger>());
            new TaxesInProductMapping(builder.Entity<TaxesInProduct>());
            new CompanyLogoMapping(builder.Entity<CompanyLogo>());
            new TaskManagerMapping(builder.Entity<TaskManager>());
            new RetailerMapping(builder.Entity<Retailer>());
            new SalesOpportunityMapping(builder.Entity<SalesOpportunity>());
            new GradeMapping(builder.Entity<Grade>());
            new StageMapping(builder.Entity<Stage>());
            new SourceMapping(builder.Entity<Source>());
            new ReasonClosedMapping(builder.Entity<ReasonClosed>());
            new FooterSettingMapping(builder.Entity<FooterSetting>());
            new GeneralSettingMapping(builder.Entity<GeneralSetting>());
            new HeaderSettingMapping(builder.Entity<HeaderSetting>());
            new IndividualSlideSettingMapping(builder.Entity<IndividualSlideSetting>());
            new LayoutSettingMapping(builder.Entity<LayoutSetting>());
            new SlideSettingMapping(builder.Entity<SlideSetting>());
            new StoreLocatorMapping(builder.Entity<StoreLocator>());
            new BrandImageMapping(builder.Entity<BrandImage>());
            new PersonnelSettingMapping(builder.Entity<PersonnelSetting>());

            #region Enumerations Mapping

            new AccountCashFlowTypeMapping(builder.Entity<AccountCashFlowTypes>());
            new AccountDetailTypeMapping(builder.Entity<AccountDetailTypes>());
            new AccountLevelTypeMapping(builder.Entity<AccountLevelTypes>());
            new AccountTypeMapping(builder.Entity<AccountTypes>());
            new AddressTypeMapping(builder.Entity<AddressTypes>());
            new DescriptionTypeMapping(builder.Entity<DescriptionTypes>());
            new DiscountTypeMapping(builder.Entity<DiscountTypes>());
            new DueDateTypeMapping(builder.Entity<DueDateTypes>());
            new DueTypeMapping(builder.Entity<DueTypes>());
            new EncryptionTypeMapping(builder.Entity<EncryptionTypes>());
            new JournalTypeMapping(builder.Entity<JournalTypes>());
            new LockTypeMapping(builder.Entity<LockTypes>());
            new MediaTypeMapping(builder.Entity<MediaTypes>());
            new PaymentDiscountTypeMapping(builder.Entity<PaymentDiscountTypes>());
            new PurchaseOrderTypeMapping(builder.Entity<PurchaseOrderTypes>());
            new PurchaseOrderStatusMapping(builder.Entity<PurchaseOrdersStatus>());
            new SalesOrderTypeMapping(builder.Entity<SalesOrderTypes>());
            new SalesOrdersStatusMapping(builder.Entity<SalesOrdersStatus>());
            new SuffixTypeMapping(builder.Entity<SuffixTypes>());
            new TaxCalculationTypeMapping(builder.Entity<TaxCalculationTypes>());
            new TermTypeMapping(builder.Entity<TermTypes>());
            new TitleTypeMapping(builder.Entity<TitleTypes>());
            new UserTypeMapping(builder.Entity<UserTypes>());
            new EntryMethodMapping(builder.Entity<EntryMethodTypes>());
            new ShippingCalculationTypeMapping(builder.Entity<ShippingCalculationTypes>());
            new DiscountCalculationTypeMapping(builder.Entity<DiscountCalculationTypes>());
            new ShippingStatusMapping(builder.Entity<ShippingStatus>());
            new ImageTypeMapping(builder.Entity<ImageTypes>());
            new CountryMapping(builder.Entity<Country>());

            #endregion
        }
        public async void EnsureSeedData(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr, 
            SignInManager<ApplicationUser> signMgr, string envContentRootPath)
        {
            return;
            Database.EnsureDeleted();
            Database.EnsureCreated();

            //seed companies with super admin account
            //seed admin user for the company

            //Create super admin with user type 1, this user manages all companies, not company's user. It will be SAS admin
            const string adminEmail = "admin@hrevertcrm.com";
            var adminUser = await userMgr.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser()
                {
                    Address = "N/A",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Manager",
                    UserType = UserType.SuperAdmin,
                    Gender = 1,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                await userMgr.CreateAsync(adminUser, "p@77w0rd");
            }
            //Seed Security Rights, not dependant to companies and needs one time seed
            var secQueryProcessor = (SecurityQueryProcessor)_serviceProvider.GetService(typeof(ISecurityQueryProcessor));
            if (!secQueryProcessor.GetActiveSecurities().Any())
            {
                var dataText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\securities.json"));
                var securities = JsonConvert.DeserializeObject<List<Security>>(dataText);
                secQueryProcessor.SaveAll(securities);
            }
         
            //Optional Seeds
            //Create 2 companies
            var companyQueryProcessor = (CompanyQueryProcessor)_serviceProvider.GetService(typeof(ICompanyQueryProcessor));
            const string company1Name = "Hrevert Technologies";
            const string company1Address = "Sinamangal, Kathmandu";
            const string company1Email = "contactus@hrevert.com";
            const string company1Phone = "011456987";
            const string company1WebsiteUrl = "www.Hrevert.com";

            var company1 = companyQueryProcessor.GetCompanies(c => c.Name == company1Name).FirstOrDefault();
            if (company1 == null)
            {
                company1 = new Company
                {
                    Name = company1Name,
                    Address = company1Address,
                    Email = company1Email,
                    PhoneNumber = company1Phone,
                    WebsiteUrl = company1WebsiteUrl,
                    CreatedBy = adminUser.Id,
                    LastUpdatedBy = adminUser.Id,
                    IsCompanyInitialized = true,
                    FiscalYearFormat = FiscalYearFormat.WithPrefix
                };
                companyQueryProcessor.Save(company1);
                await SeedNewCompanyData.Seed(company1,  userMgr, _serviceProvider, envContentRootPath);
                await SeedingTrivialDatas.Seed(company1, userMgr, _serviceProvider, envContentRootPath);
            }
            const string company2Name = "Sample Technologies";
            const string company2Address = "Gaushala, Kathmandu";
            const string company2Email = "contactus@sample.com";
            const string company2Phone = "011236547";
            const string company2WebsiteUrl = "www.SampleTech.com";
            
            var company2 = companyQueryProcessor.GetCompanies(c => c.Name == company2Name).FirstOrDefault();
            if (company2 != null) return;
            company2 = new Company
            {
                Name = company2Name,
                Address = company2Address,
                Email = company2Email,
                PhoneNumber = company2Phone,
                WebsiteUrl = company2WebsiteUrl,
                CreatedBy = adminUser.Id,
                LastUpdatedBy = adminUser.Id,
                IsCompanyInitialized = true,
                FiscalYearFormat = FiscalYearFormat.WithPrefix
            };
            companyQueryProcessor.Save(company2);
            await SeedNewCompanyData.Seed(company2, userMgr, _serviceProvider, envContentRootPath);
            await SeedingTrivialDatas.Seed(company2, userMgr, _serviceProvider, envContentRootPath);
        }
    }
}