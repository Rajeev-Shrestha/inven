using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HrevertCRM.Data;
using Hrevert.Common.Enums;
using HrevertCRM.Common;
using HrevertCRM.Entities;
using Hrevert.Common;

namespace HrevertCRM.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HrevertCRM.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("AccountCashFlowType");

                    b.Property<string>("AccountCode")
                        .IsRequired();

                    b.Property<string>("AccountDescription");

                    b.Property<int>("AccountDetailType");

                    b.Property<byte>("AccountType");

                    b.Property<bool>("Active");

                    b.Property<bool>("BankAccount");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<decimal>("CurrentBalance");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte>("Level");

                    b.Property<bool>("MainAccount");

                    b.Property<int?>("ParentAccountId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ParentAccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<byte>("AddressType");

                    b.Property<string>("City");

                    b.Property<int>("CompanyId");

                    b.Property<int>("CountryId");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("DeliveryZoneId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Fax");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsDefault");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MiddleName");

                    b.Property<string>("MobilePhone")
                        .IsRequired();

                    b.Property<string>("State");

                    b.Property<int>("Suffix");

                    b.Property<string>("Telephone");

                    b.Property<int>("Title");

                    b.Property<string>("UserId");

                    b.Property<int?>("VendorId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.Property<string>("Website");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DeliveryZoneId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("UserId");

                    b.HasIndex("VendorId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CompanyName");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MiddleName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("UserType");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.BrandImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("FooterSettingId");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("FooterSettingId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("BrandImages");
                });

            modelBuilder.Entity("HrevertCRM.Entities.BugLogger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("BugAdded");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Message");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("BugLoggers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Carousel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("ImageId");

                    b.Property<string>("ImageUrl");

                    b.Property<int>("ItemId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte>("ProductOrCategory");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ImageId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Carousels");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Email");

                    b.Property<string>("FaxNo");

                    b.Property<byte>("FiscalYearFormat");

                    b.Property<string>("GpoBoxNumber");

                    b.Property<bool>("IsCompanyInitialized");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("MasterId");

                    b.Property<string>("Name");

                    b.Property<string>("PanRegistrationNo");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("VatRegistrationNo");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CompanyLogo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("ImageId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MediaUrl");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ImageId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("CompanyLogos");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CompanyWebSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("AllowGuest");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerSerialNo");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DeliveryMethodId");

                    b.Property<byte>("DiscountCalculationType");

                    b.Property<bool>("IsEstoreInitialized");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("PurchaseOrderCode");

                    b.Property<int>("SalesOrderCode");

                    b.Property<string>("SalesRepId");

                    b.Property<byte>("ShippingCalculationType");

                    b.Property<int>("VendorSerialNo");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("CompanyWebSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("BillingAddressId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CustomerCode");

                    b.Property<int?>("CustomerLevelId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("DisplayName");

                    b.Property<bool?>("IsCodEnabled");

                    b.Property<bool?>("IsPrepayEnabled");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Note");

                    b.Property<int?>("OnAccountId");

                    b.Property<decimal>("OpeningBalance");

                    b.Property<int>("PanNo");

                    b.Property<string>("Password");

                    b.Property<int?>("PaymentMethodId");

                    b.Property<int?>("PaymentTermId");

                    b.Property<string>("TaxRegNo");

                    b.Property<int>("VatNo");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerLevelId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentTermId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerContactGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("GroupName");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("CustomerContactGroups");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerInContactGroup", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("ContactGroupId");

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<byte[]>("Version");

                    b.HasKey("CustomerId", "ContactGroupId");

                    b.HasIndex("ContactGroupId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.ToTable("CustomerInContactGroups");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("CustomerLevels");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerLoginEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomerId");

                    b.Property<int>("FailedAttemptCount");

                    b.Property<int>("LockDuration");

                    b.Property<byte>("LockType");

                    b.Property<bool>("Locked");

                    b.Property<DateTime>("LockedTime");

                    b.Property<DateTime>("LoginTime");

                    b.HasKey("Id");

                    b.ToTable("CustomerLoginEvents");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("DeliveryCode")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DeliveryMethods");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("DeliveryMethodId");

                    b.Property<int?>("DeliveryZoneId");

                    b.Property<decimal?>("DocTotalFrom");

                    b.Property<decimal?>("DocTotalTo");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("MeasureUnitId");

                    b.Property<decimal?>("MinimumRate");

                    b.Property<int?>("ProductCategoryId");

                    b.Property<int?>("ProductId");

                    b.Property<decimal?>("Rate");

                    b.Property<int?>("UnitFrom");

                    b.Property<int?>("UnitTo");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.Property<decimal?>("WeightFrom");

                    b.Property<decimal?>("WeightTo");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeliveryMethodId");

                    b.HasIndex("DeliveryZoneId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("MeasureUnitId");

                    b.HasIndex("ProductCategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("DeliveryRates");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryZone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.Property<string>("ZoneCode")
                        .IsRequired();

                    b.Property<string>("ZoneName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DeliveryZones");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("CustomerId");

                    b.Property<int?>("CustomerLevelId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<DateTime>("DiscountEndDate");

                    b.Property<DateTime>("DiscountStartDate");

                    b.Property<byte>("DiscountType");

                    b.Property<double>("DiscountValue");

                    b.Property<int?>("ItemId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("MinimumQuantity");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerId");

                    b.HasIndex("CustomerLevelId");

                    b.HasIndex("ItemId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Distributor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<byte[]>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.ToTable("Distributor");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EcommerceSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<bool>("DecreaseQuantityOnOrder");

                    b.Property<bool>("DisplayOutOfStockItems");

                    b.Property<byte>("DisplayQuantity");

                    b.Property<int>("DueDatePeriod");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IncludeQuantityInSalesOrder");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("ProductPerCategory");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("EcommerceSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EmailSender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Cc");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("EmailNotSentCause");

                    b.Property<bool>("IsEmailSent");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MailFrom");

                    b.Property<string>("MailTo");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<byte[]>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EmailSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<byte>("EncryptionType");

                    b.Property<string>("Host")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Port");

                    b.Property<bool>("RequireAuthentication");

                    b.Property<string>("Sender")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("EmailSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountCashFlowTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("AccountCashFlowTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountDetailTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("AccountDetailTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountLevelTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("AccountLevelTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("AccountTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AddressTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("AddressTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DescriptionTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DescriptionTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DiscountCalculationTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DiscountCalculationTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DiscountTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DiscountTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DueDateTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DueDateTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DueTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("DueTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.EncryptionTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("EncryptionTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.EntryMethodTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("EntryMethodTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ImageTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("ImageTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.JournalTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("JournalTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.LockTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("LockTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.MediaTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("MediaTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PaymentDiscountTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("PaymentDiscountTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PurchaseOrdersStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("PurchaseOrdersStatus");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PurchaseOrderTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("PurchaseOrderTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SalesOrdersStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SalesOrdersStatus");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SalesOrderTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SalesOrderTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ShippingCalculationTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("ShippingCalculationTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ShippingStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("ShippingStatus");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SuffixTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SuffixTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TaxCalculationTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("TaxCalculationTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TermTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("TermTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TitleTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("TitleTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.UserTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FeaturedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<byte>("ImageType");

                    b.Property<int>("ItemId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("SortOrder");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("FeaturedItems");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FeaturedItemMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("FeaturedItemId");

                    b.Property<byte>("ImageType");

                    b.Property<int>("ItemId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte>("MediaType");

                    b.Property<string>("MediaUrl")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("FeaturedItemId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("FeaturedItemMetadatas");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FiscalPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("FiscalYearId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("FiscalYearId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("FiscalPeriods");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FiscalYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("FiscalYears");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FooterSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutStore");

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CopyrightText");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<bool>("EnableAbout");

                    b.Property<bool>("EnableBrands");

                    b.Property<bool>("EnableContact");

                    b.Property<bool>("EnableCopyright");

                    b.Property<bool>("EnableFaq");

                    b.Property<bool>("EnableFooterMenu");

                    b.Property<bool>("EnableNewsLetter");

                    b.Property<bool>("EnablePolicies");

                    b.Property<bool>("EnableStoreLocator");

                    b.Property<string>("FooterLogoUrl");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("ShowFooterLogo");

                    b.Property<bool>("ShowOrderHistoryLink");

                    b.Property<byte>("ShowTrendingOrLastest");

                    b.Property<bool>("ShowUserLoginLink");

                    b.Property<bool>("ShowWishlistLink");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("WhereToFindUs");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("FooterSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.GeneralSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<bool>("EnableGetInspired");

                    b.Property<bool>("EnableHotThisWeek");

                    b.Property<bool>("EnableLatestProducts");

                    b.Property<bool>("EnableSlides");

                    b.Property<bool>("EnableTopCategories");

                    b.Property<bool>("EnableTopTrendingProducts");

                    b.Property<string>("FaviconLogoUrl");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LogoUrl");

                    b.Property<string>("SelectedTheme");

                    b.Property<string>("StoreName");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("GeneralSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("GradeName")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("Rank");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("HrevertCRM.Entities.HeaderSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<bool>("EnableOfferOfTheDay");

                    b.Property<bool>("EnableSocialLinks");

                    b.Property<bool>("EnableStoreLocator");

                    b.Property<bool>("EnableWishlist");

                    b.Property<string>("FacebookUrl");

                    b.Property<string>("InstagramUrl");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LinkedInUrl");

                    b.Property<int>("NumberOfStores");

                    b.Property<string>("OfferOfTheDayUrl");

                    b.Property<string>("RssUrl");

                    b.Property<string>("TumblrUrl");

                    b.Property<string>("TwitterUrl");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("HeaderSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("ImageBase64");

                    b.Property<byte>("ImageSize");

                    b.Property<byte>("ImageType");

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("HrevertCRM.Entities.IndividualSlideSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("ColorCode");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<double>("DiscountPercentage");

                    b.Property<bool>("DiscountPercentageCheck");

                    b.Property<bool>("EnableFreeShipping");

                    b.Property<DateTime>("ExpireDate");

                    b.Property<string>("ExploreToLinkPage");

                    b.Property<bool>("IsExpires");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LimitedTimeOfferText");

                    b.Property<double>("OriginalPrice");

                    b.Property<bool>("OriginalPriceCheck");

                    b.Property<int>("ProductType");

                    b.Property<double>("SalesPrice");

                    b.Property<bool>("ShowAddToCartOption");

                    b.Property<bool>("ShowAddToListOption");

                    b.Property<byte>("SlideBackground");

                    b.Property<string>("SlideBackgroundImageUrl");

                    b.Property<string>("SlideImageUrl");

                    b.Property<int>("SlideSettingId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("SlideSettingId");

                    b.ToTable("IndividualSlideSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ItemCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("ItemId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<int>("QuantityOnInvoice");

                    b.Property<int>("QuantityOnOrder");

                    b.Property<byte[]>("Version");

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.ToTable("ItemCounts");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ItemMeasure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("MeasureUnitId");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("MeasureUnitId");

                    b.HasIndex("ProductId");

                    b.ToTable("ItemMeasures");
                });

            modelBuilder.Entity("HrevertCRM.Entities.JournalMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("Cancelled");

                    b.Property<bool>("Closed");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<decimal>("Credit");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<decimal>("Debit");

                    b.Property<string>("Description");

                    b.Property<int>("FiscalPeriodId");

                    b.Property<bool>("IsSystem");

                    b.Property<byte>("JournalType");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Note");

                    b.Property<bool>("Posted");

                    b.Property<DateTime>("PostedDate");

                    b.Property<bool>("Printed");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("FiscalPeriodId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("JournalMasters");
                });

            modelBuilder.Entity("HrevertCRM.Entities.LayoutSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<byte>("BackgroundImageOrColor");

                    b.Property<string>("BackgroundImageUrl");

                    b.Property<int>("CategoryFour");

                    b.Property<int>("CategoryOne");

                    b.Property<int>("CategoryThree");

                    b.Property<int>("CategoryTwo");

                    b.Property<string>("ColorCode");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<bool>("EnableSeparator");

                    b.Property<string>("HotThisWeekColor");

                    b.Property<string>("HotThisWeekImageUrl");

                    b.Property<string>("HotThisWeekSeparatorUrl");

                    b.Property<int>("HotThisWeekTitleStyle");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LatestProductsColor");

                    b.Property<string>("LatestProductsImageUrl");

                    b.Property<string>("LatestProductsSeparatorUrl");

                    b.Property<int>("LatestProductsTitleStyle");

                    b.Property<bool>("ShowHotThisWeekLayoutTitle");

                    b.Property<bool>("ShowLatestProductsLayoutTitle");

                    b.Property<bool>("ShowLayoutTitle");

                    b.Property<bool>("ShowTrendingItemsLayoutTitle");

                    b.Property<string>("TrendingItemsColor");

                    b.Property<string>("TrendingItemsImageUrl");

                    b.Property<string>("TrendingItemsSeparatorUrl");

                    b.Property<int>("TrendingItemsTitleStyle");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("LayoutSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.MeasureUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<byte>("EntryMethod");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Measure");

                    b.Property<string>("MeasureCode");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("MeasureUnits");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MethodCode")
                        .IsRequired();

                    b.Property<string>("MethodName")
                        .IsRequired();

                    b.Property<string>("ReceipentMemo");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PaymentTerm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<decimal>("DiscountDays");

                    b.Property<byte>("DiscountType");

                    b.Property<decimal>("DiscountValue");

                    b.Property<byte>("DueDateType");

                    b.Property<int>("DueDateValue");

                    b.Property<byte>("DueType");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("TermCode")
                        .IsRequired();

                    b.Property<string>("TermName")
                        .IsRequired();

                    b.Property<byte>("TermType");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("PaymentTerms");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PayMethodsInPayTerm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<int>("PayMethodId");

                    b.Property<int>("PayTermId");

                    b.Property<byte[]>("Version");

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.ToTable("PayMethodsInPayTerms");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PersonnelSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("LayoutSettingId");

                    b.Property<string>("PersonnelImageUrl");

                    b.Property<string>("RecommendationText");

                    b.Property<string>("RecommendingPersonAddress");

                    b.Property<string>("RecommendingPersonName");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("LayoutSettingId");

                    b.ToTable("PersonnelSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("AllowBackOrder");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<decimal?>("CommissionRate");

                    b.Property<bool?>("Commissionable");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LongDescription");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte>("ProductType");

                    b.Property<int>("QuantityOnHand");

                    b.Property<int>("QuantityOnOrder");

                    b.Property<string>("ShortDescription");

                    b.Property<double>("UnitPrice");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CategoryImageUrl");

                    b.Property<short>("CategoryRank");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ParentId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductInCategory", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("CategoryId");

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("ProductInCategories");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<byte>("ImageSize");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte>("MediaType");

                    b.Property<string>("MediaUrl")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductMetadatas");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductPriceRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<double?>("DiscountPercent");

                    b.Property<DateTime>("EndDate");

                    b.Property<double?>("FixedPrice");

                    b.Property<int?>("FreeQuantity");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("ProductId");

                    b.Property<int?>("Quantity");

                    b.Property<DateTime>("StartDate");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPriceRule");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<double>("CostPrice");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DistributerId");

                    b.Property<int?>("DistributorId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<int>("ProductId");

                    b.Property<double>("SalesPrice");

                    b.Property<byte[]>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DistributorId");

                    b.HasIndex("LastUserId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductRate");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductsRefByKitAndAssembledType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProductRefId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsRefByKitAndAssembledTypes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PurchaseOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("BillingAddressId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DeliveryMethodId");

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("FiscalPeriodId");

                    b.Property<bool>("FullyPaid");

                    b.Property<DateTime>("InvoicedDate");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<DateTime>("OrderDate");

                    b.Property<byte>("OrderType");

                    b.Property<decimal>("PaidAmount");

                    b.Property<DateTime>("PaymentDueOn");

                    b.Property<int>("PaymentTermId");

                    b.Property<string>("PurchaseOrderCode");

                    b.Property<string>("PurchaseRepId");

                    b.Property<string>("SalesOrderNumber");

                    b.Property<int>("ShippingAddressId");

                    b.Property<byte>("Status");

                    b.Property<decimal>("TotalAmount");

                    b.Property<int>("VendorId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeliveryMethodId");

                    b.HasIndex("FiscalPeriodId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("PaymentTermId");

                    b.HasIndex("PurchaseRepId");

                    b.HasIndex("VendorId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PurchaseOrderLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<byte?>("DescriptionType");

                    b.Property<decimal>("Discount");

                    b.Property<byte>("DiscountType");

                    b.Property<decimal>("ItemPrice");

                    b.Property<decimal>("ItemQuantity");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<short>("LineOrder");

                    b.Property<int>("ProductId");

                    b.Property<int>("PurchaseOrderId");

                    b.Property<bool>("Shipped");

                    b.Property<decimal>("ShippedQuantity");

                    b.Property<decimal>("TaxAmount");

                    b.Property<int?>("TaxId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseOrderId");

                    b.HasIndex("TaxId");

                    b.ToTable("PurchaseOrderLines");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ReasonClosed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Reason")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("ReasonClosed");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Retailer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DistibutorId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("RetailerId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Retailer");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("Level");

                    b.Property<string>("MiddleName");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SalesManagers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOpportunity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<decimal>("BusinessValue");

                    b.Property<DateTime>("ClosedDate");

                    b.Property<DateTime>("ClosingDate");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("GradeId");

                    b.Property<bool>("IsClosed");

                    b.Property<bool>("IsSucceeded");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("Priority");

                    b.Property<int>("Probability");

                    b.Property<int>("ReasonClosedId");

                    b.Property<string>("SalesRepresentative");

                    b.Property<int>("SourceId");

                    b.Property<int>("StageId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GradeId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ReasonClosedId");

                    b.HasIndex("SalesRepresentative");

                    b.HasIndex("SourceId");

                    b.HasIndex("StageId");

                    b.ToTable("SalesOpportunities");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("BillingAddressId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("DeliveryMethodId");

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("FiscalPeriodId");

                    b.Property<bool>("FullyPaid");

                    b.Property<DateTime>("InvoicedDate");

                    b.Property<bool>("IsWebOrder");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte>("OrderType");

                    b.Property<decimal>("PaidAmount");

                    b.Property<DateTime>("PaymentDueOn");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("PaymentTermId");

                    b.Property<string>("PurchaseOrderNumber");

                    b.Property<string>("SalesOrderCode");

                    b.Property<string>("SalesPolicy");

                    b.Property<string>("SalesRepId");

                    b.Property<int>("ShippingAddressId");

                    b.Property<decimal?>("ShippingCost");

                    b.Property<byte>("Status");

                    b.Property<decimal>("TotalAmount");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DeliveryMethodId");

                    b.HasIndex("FiscalPeriodId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentTermId");

                    b.HasIndex("SalesRepId");

                    b.ToTable("SalesOrders");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOrderLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<byte?>("DescriptionType");

                    b.Property<decimal>("Discount");

                    b.Property<byte>("DiscountType");

                    b.Property<decimal>("ItemPrice");

                    b.Property<decimal>("ItemQuantity");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<short>("LineOrder");

                    b.Property<int>("ProductId");

                    b.Property<int>("SalesOrderId");

                    b.Property<bool>("Shipped");

                    b.Property<decimal>("ShippedQuantity");

                    b.Property<decimal>("TaxAmount");

                    b.Property<int?>("TaxId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesOrderId");

                    b.HasIndex("TaxId");

                    b.ToTable("SalesOrderLines");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Security", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("SecurityCode");

                    b.Property<string>("SecurityDescription")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Securities");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("GroupDescription")
                        .IsRequired();

                    b.Property<string>("GroupName")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SecurityGroups");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityGroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("MemberId");

                    b.Property<int>("SecurityGroupId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("MemberId");

                    b.HasIndex("SecurityGroupId");

                    b.ToTable("SecurityGroupMembers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityRight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("Allowed");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("SecurityGroupId");

                    b.Property<int>("SecurityId");

                    b.Property<string>("UserId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("SecurityGroupId");

                    b.HasIndex("SecurityId");

                    b.HasIndex("UserId");

                    b.ToTable("SecurityRights");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<decimal>("Amount");

                    b.Property<int?>("BillingAddressId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("DeliveryMethodId");

                    b.Property<string>("HostIp");

                    b.Property<bool>("IsCheckedOut");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("PaymentTermId");

                    b.Property<int?>("ShippingAddressId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeliveryMethodId");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("PaymentTermId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ShoppingCartDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<decimal>("Discount");

                    b.Property<Guid?>("Guid");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<decimal>("ProductCost");

                    b.Property<int>("ProductId");

                    b.Property<byte>("ProductType");

                    b.Property<decimal>("Quantity");

                    b.Property<decimal?>("ShippingCost");

                    b.Property<int>("ShoppingCartId");

                    b.Property<DateTime>("ShoppingDateTime");

                    b.Property<decimal>("TaxAmount");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.Property<decimal>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartDetails");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SlideSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("NumberOfSlides");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("SlildeSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("SourceName")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Stage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("Probability");

                    b.Property<int>("Rank");

                    b.Property<string>("StageName")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("HrevertCRM.Entities.StoreLocator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Email");

                    b.Property<string>("Fax");

                    b.Property<int>("HeaderSettingId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("HeaderSettingId");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("StoreLocators");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TaskManager", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<int>("CompletePercentage");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("DocId");

                    b.Property<int>("DocType");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("Reminder");

                    b.Property<DateTime>("ReminderEndDateTime");

                    b.Property<DateTime>("ReminderStartDateTime");

                    b.Property<int>("Status");

                    b.Property<string>("TaskAssignedToUser")
                        .IsRequired();

                    b.Property<string>("TaskDescription")
                        .IsRequired();

                    b.Property<DateTime>("TaskEndDateTime");

                    b.Property<int>("TaskPriority");

                    b.Property<DateTime>("TaskStartDateTime");

                    b.Property<string>("TaskTitle")
                        .IsRequired();

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("TaskId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("TaskAssignedToUser");

                    b.ToTable("TaskManagers");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Tax", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description");

                    b.Property<bool>("IsRecoverable");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("RecoverableCalculationType");

                    b.Property<string>("TaxCode")
                        .IsRequired();

                    b.Property<decimal>("TaxRate");

                    b.Property<byte>("TaxType");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.ToTable("Taxes");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TaxesInProduct", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("TaxId");

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ProductId", "TaxId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("TaxId");

                    b.ToTable("TaxesInProducts");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TransactionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("ChangedItemId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("ItemType");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("NotificationProcessed");

                    b.Property<int>("SecurityId");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<string>("UserId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("SecurityId");

                    b.ToTable("TransactionLogs");
                });

            modelBuilder.Entity("HrevertCRM.Entities.UserSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("EntityId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LastUserId");

                    b.Property<int>("PageSize");

                    b.Property<string>("UserId");

                    b.Property<byte[]>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastUserId");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("BillingAddressId");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<int>("CompanyId");

                    b.Property<string>("ContactName");

                    b.Property<string>("CreatedBy");

                    b.Property<decimal>("Credit");

                    b.Property<decimal>("CreditLimit");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<decimal>("Debit");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int?>("PaymentMethodId");

                    b.Property<int?>("PaymentTermId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("WebActive");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentTermId");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Account", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AccountCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AccountModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Account", "ParentAccount")
                        .WithMany()
                        .HasForeignKey("ParentAccountId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Address", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("Addresses")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AddressCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Customer", "Customer")
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HrevertCRM.Entities.DeliveryZone", "DeliveryZone")
                        .WithMany("Addresses")
                        .HasForeignKey("DeliveryZoneId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AddressModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId");

                    b.HasOne("HrevertCRM.Entities.Vendor", "Vendor")
                        .WithMany("Addresses")
                        .HasForeignKey("VendorId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ApplicationUser", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("UsersCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("UsersModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.BrandImage", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("BrandImageCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.FooterSetting", "FooterSetting")
                        .WithMany("BrandImages")
                        .HasForeignKey("FooterSettingId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("BrandImageModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.BugLogger", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("BugLoggerCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("BugLoggerModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Carousel", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CarouselCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CarouselModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Company", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CompaniesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CompaniesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CompanyLogo", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CompanyLogoCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CompanyLogoModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CompanyWebSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CompanyWebSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CompanyWebSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Customer", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Address", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CustomerCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.CustomerLevel", "CustomerLevel")
                        .WithMany("Customers")
                        .HasForeignKey("CustomerLevelId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CustomerModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Customers")
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("HrevertCRM.Entities.PaymentTerm", "PaymentTerm")
                        .WithMany("Customers")
                        .HasForeignKey("PaymentTermId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerContactGroup", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CustomerContactGroupCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CustomerContactGroupModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerInContactGroup", b =>
                {
                    b.HasOne("HrevertCRM.Entities.CustomerContactGroup", "ContactGroup")
                        .WithMany("CustomerAndContactGroupList")
                        .HasForeignKey("ContactGroupId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.Customer", "Customer")
                        .WithMany("CustomerInContactGroups")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.CustomerLevel", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CustomerLevelCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CustomerLevelModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryMethod", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DeliveryMethodCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DeliveryMethodModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryRate", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DeliveryRateCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.DeliveryMethod", "DeliveryMethod")
                        .WithMany("DeliveryRates")
                        .HasForeignKey("DeliveryMethodId");

                    b.HasOne("HrevertCRM.Entities.DeliveryZone", "DeliveryZone")
                        .WithMany("DeliveryRates")
                        .HasForeignKey("DeliveryZoneId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DeliveryRateModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.MeasureUnit", "MeasureUnit")
                        .WithMany("DeliveryRates")
                        .HasForeignKey("MeasureUnitId");

                    b.HasOne("HrevertCRM.Entities.ProductCategory", "ProductCategory")
                        .WithMany("DeliveryRates")
                        .HasForeignKey("ProductCategoryId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("DeliveryRates")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.DeliveryZone", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DeliveryZoneCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DeliveryZoneModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Discount", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ProductCategory", "ProductCategory")
                        .WithMany("Discounts")
                        .HasForeignKey("CategoryId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DiscountCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Customer", "Customer")
                        .WithMany("Discounts")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HrevertCRM.Entities.CustomerLevel", "CustomerLevel")
                        .WithMany("Discounts")
                        .HasForeignKey("CustomerLevelId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("Discounts")
                        .HasForeignKey("ItemId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DiscountModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Distributor", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EcommerceSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("EcommerceSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("EcommerceSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EmailSender", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("EmailSenderCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("EmailSenderModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.EmailSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("EmailSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("EmailSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountCashFlowTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AccountCashFlowTypeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AccountCashFlowTypeModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountDetailTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AccountDetailTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AccountDetailTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountLevelTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AccountLevelTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AccountLevelTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AccountTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AccountTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AccountTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.AddressTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("AddressTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("AddressTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.Country", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("CountryCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("CountryModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DescriptionTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DescriptionTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DescriptionTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DiscountCalculationTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DiscountCalculationTypeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DiscountCalculationTypeModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DiscountTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DiscountTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DiscountTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DueDateTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DueDateTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DueDateTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.DueTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("DueTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("DueTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.EncryptionTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("EncryptionTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("EncryptionTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.EntryMethodTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("EntryMethodCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("EntryMethodModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ImageTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ImageTypeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ImageTypeModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.JournalTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("JournalTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("JournalTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.LockTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("LockTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("LockTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.MediaTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("MediaTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("MediaTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PaymentDiscountTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PaymentDiscountTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PaymentDiscountTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PurchaseOrdersStatus", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PurchaseOrdersStatusCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PurchaseOrdersStatusModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.PurchaseOrderTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PurchaseOrderTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PurchaseOrderTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SalesOrdersStatus", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SalesOrdersStatusCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SalesOrdersStatusModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SalesOrderTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SalesOrderTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SalesOrderTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ShippingCalculationTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ShippingCalculationTypeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ShippingCalculationTypeModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.ShippingStatus", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ShippingStatusCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ShippingStatusModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.SuffixTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SuffixTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SuffixTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TaxCalculationTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TaxCalculationTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TaxCalculationTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TermTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TermTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TermTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.TitleTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TitleTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TitleTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Enumerations.UserTypes", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("UserTypesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("UserTypesModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FeaturedItem", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("FeaturedItemCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("FeaturedItemModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FeaturedItemMetadata", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("FeaturedItemMetadataCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.FeaturedItem", "FeaturedItem")
                        .WithMany("FeaturedItemMetadatas")
                        .HasForeignKey("FeaturedItemId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("FeaturedItemMetadataModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FiscalPeriod", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("FiscalPeriods")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("FiscalPeriodCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.FiscalYear", "FiscalYear")
                        .WithMany("FiscalPeriods")
                        .HasForeignKey("FiscalYearId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("FiscalPeriodModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FiscalYear", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("FiscalYears")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("FiscalYearCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("FiscalYearModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.FooterSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("FooterSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("FooterSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.GeneralSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("GeneralSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("GeneralSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Grade", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("GradeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("GradeModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.HeaderSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("HeaderSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("HeaderSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.IndividualSlideSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("IndividualSlideSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("IndividualSlideSettingModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.SlideSetting", "SlideSetting")
                        .WithMany("IndividualSlideSettings")
                        .HasForeignKey("SlideSettingId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ItemCount", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ItemMeasure", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ItemMeasureCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ItemMeasureModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.MeasureUnit", "MeasureUnit")
                        .WithMany("ItemMeasures")
                        .HasForeignKey("MeasureUnitId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ItemMeasures")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.JournalMaster", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("JournalMasterCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.FiscalPeriod", "FiscalPeriod")
                        .WithMany("JournalMasters")
                        .HasForeignKey("FiscalPeriodId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("JournalMasterModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.LayoutSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("LayoutSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("LayoutSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.MeasureUnit", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("MeasureUnitCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("MeasureUnitModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PaymentMethod", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PaymentMethodCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PaymentMethodModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PaymentTerm", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PaymentTermCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PaymentTermModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PayMethodsInPayTerm", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PersonnelSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PersonnelSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PersonnelSettingModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.LayoutSetting", "LayoutSetting")
                        .WithMany("PersonnelSettings")
                        .HasForeignKey("LayoutSettingId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Product", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("Products")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductsCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductsModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductCategory", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductCategoriesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductCategoriesModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.ProductCategory", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductInCategory", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ProductCategory", "ProductCategory")
                        .WithMany("ProductInCategories")
                        .HasForeignKey("CategoryId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductInCategoriesCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductInCategoriesModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ProductInCategories")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductMetadata", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductMetadataCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductMetadataModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ProductMetadatas")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductPriceRule", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ProductCategory", "Category")
                        .WithMany("ProductPriceRules")
                        .HasForeignKey("CategoryId");

                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("ProductPriceRules")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductPriceRuleCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductPriceRuleModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ProductPriceRules")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductRate", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.Distributor", "Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ProductRates")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ProductsRefByKitAndAssembledType", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ProductsRefByKitAndAssembledTypeCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ProductsRefByKitAndAssembledTypeModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ProductsReferencedByKitAndAssembledTypes")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PurchaseOrder", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Address", "BillingAddress")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("BillingAddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("PurchaseOrderCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.DeliveryMethod", "DeliveryMethod")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("DeliveryMethodId");

                    b.HasOne("HrevertCRM.Entities.FiscalPeriod", "FiscalPeriod")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("FiscalPeriodId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("PurchaseOrderModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.PaymentTerm", "PaymentTerm")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("PaymentTermId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "PurchaseRep")
                        .WithMany("PurchaseOrderByPurchaseRep")
                        .HasForeignKey("PurchaseRepId");

                    b.HasOne("HrevertCRM.Entities.Vendor", "Vendor")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("VendorId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.PurchaseOrderLine", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("PurchaseOrderLines")
                        .HasForeignKey("ProductId");

                    b.HasOne("HrevertCRM.Entities.PurchaseOrder", "PurchaseOrder")
                        .WithMany("PurchaseOrderLines")
                        .HasForeignKey("PurchaseOrderId");

                    b.HasOne("HrevertCRM.Entities.Tax", "TaxType")
                        .WithMany("PurchaseOrderLines")
                        .HasForeignKey("TaxId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ReasonClosed", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ReasonClosedCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ReasonClosedModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Retailer", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("RetailerCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("RetailerModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesManager", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("SalesManagers")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SalesManagerCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SalesManagerModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOpportunity", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SalesOpportunityCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Customer", "Customer")
                        .WithMany("SalesOpportunity")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HrevertCRM.Entities.Grade", "Grade")
                        .WithMany("SalesOpportunities")
                        .HasForeignKey("GradeId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SalesOpportunityModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.ReasonClosed", "ReasonClosed")
                        .WithMany("SalesOpportunities")
                        .HasForeignKey("ReasonClosedId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("SalesOpportunity")
                        .HasForeignKey("SalesRepresentative");

                    b.HasOne("HrevertCRM.Entities.Source", "Source")
                        .WithMany("SalesOpportunities")
                        .HasForeignKey("SourceId");

                    b.HasOne("HrevertCRM.Entities.Stage", "Stage")
                        .WithMany("SalesOpportunities")
                        .HasForeignKey("StageId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOrder", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Address", "BillingAddress")
                        .WithMany("SalesOrders")
                        .HasForeignKey("BillingAddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany("SalesOrders")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SalesOrderCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.Customer", "Customer")
                        .WithMany("SalesOrders")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HrevertCRM.Entities.DeliveryMethod", "DeliveryMethod")
                        .WithMany("SalesOrders")
                        .HasForeignKey("DeliveryMethodId");

                    b.HasOne("HrevertCRM.Entities.FiscalPeriod", "FiscalPeriod")
                        .WithMany("SalesOrders")
                        .HasForeignKey("FiscalPeriodId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SalesOrderModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("SalesOrders")
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("HrevertCRM.Entities.PaymentTerm", "PaymentTerm")
                        .WithMany("SalesOrders")
                        .HasForeignKey("PaymentTermId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "SalesRep")
                        .WithMany("SalesOrdersBySalesRep")
                        .HasForeignKey("SalesRepId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SalesOrderLine", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("SalesOrderLines")
                        .HasForeignKey("ProductId");

                    b.HasOne("HrevertCRM.Entities.SalesOrder", "SalesOrder")
                        .WithMany("SalesOrderLines")
                        .HasForeignKey("SalesOrderId");

                    b.HasOne("HrevertCRM.Entities.Tax", "TaxType")
                        .WithMany("SalesOrderLines")
                        .HasForeignKey("TaxId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Security", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SecurityCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SecurityModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityGroup", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SecurityGroupCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SecurityGroupModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityGroupMember", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SecurityGroupMemberCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SecurityGroupMemberModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "MemberUser")
                        .WithMany("SecurityGroupMemberUsers")
                        .HasForeignKey("MemberId");

                    b.HasOne("HrevertCRM.Entities.SecurityGroup", "SecurityGroup")
                        .WithMany("Members")
                        .HasForeignKey("SecurityGroupId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SecurityRight", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SecurityRightCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SecurityRightModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.SecurityGroup", "SecurityGroup")
                        .WithMany("Rights")
                        .HasForeignKey("SecurityGroupId");

                    b.HasOne("HrevertCRM.Entities.Security", "Security")
                        .WithMany("SecurityRights")
                        .HasForeignKey("SecurityId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "MemberUser")
                        .WithMany("SecurityRightMemberUsers")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ShoppingCart", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Address", "BillingAddress")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("BillingAddressId");

                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ShoppingCartCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.DeliveryMethod", "DeliveryMethod")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("DeliveryMethodId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ShoppingCartModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.PaymentTerm", "PaymentTerm")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("PaymentTermId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.ShoppingCartDetail", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("ShoppingCartDetailCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("ShoppingCartDetailModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("ShoppingCartDetails")
                        .HasForeignKey("ProductId");

                    b.HasOne("HrevertCRM.Entities.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartDetails")
                        .HasForeignKey("ShoppingCartId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.SlideSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SlideSettingCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SlideSettingModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Source", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("SourceCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("SourceModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Stage", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("StageCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("StageModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.StoreLocator", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("StoreLocatorCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.HeaderSetting", "HeaderSetting")
                        .WithMany("StoreLocators")
                        .HasForeignKey("HeaderSettingId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("StoreLocatorModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TaskManager", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TaskManagerCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TaskManagerModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("TasksAssignedToUser")
                        .HasForeignKey("TaskAssignedToUser");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Tax", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TaxCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TaxModified")
                        .HasForeignKey("LastUpdatedBy");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TaxesInProduct", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TaxesInProductsCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TaxesInProductsModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Product", "Product")
                        .WithMany("TaxesInProducts")
                        .HasForeignKey("ProductId");

                    b.HasOne("HrevertCRM.Entities.Tax", "Tax")
                        .WithMany("TaxesInProducts")
                        .HasForeignKey("TaxId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.TransactionLog", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("TransactionLogCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("TransactionLogModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.Security", "Security")
                        .WithMany("TransactionLogs")
                        .HasForeignKey("SecurityId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.UserSetting", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");
                });

            modelBuilder.Entity("HrevertCRM.Entities.Vendor", b =>
                {
                    b.HasOne("HrevertCRM.Entities.Address", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "CreatedByUser")
                        .WithMany("VendorCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("HrevertCRM.Entities.ApplicationUser", "LastUser")
                        .WithMany("VendorModified")
                        .HasForeignKey("LastUpdatedBy");

                    b.HasOne("HrevertCRM.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Vendors")
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("HrevertCRM.Entities.PaymentTerm", "PaymentTerm")
                        .WithMany("Vendors")
                        .HasForeignKey("PaymentTermId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HrevertCRM.Entities.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HrevertCRM.Entities.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
