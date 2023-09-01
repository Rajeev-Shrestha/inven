using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using HrevertCRM.Common;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Entities
{
    public class ApplicationUser : IdentityUser, IWebItem
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public UserType UserType { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; } = true;
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public ApplicationUser LastUser { get; set; }
        public ApplicationUser CreatedByUser { get; set; }

        #region Created and Modified Collection of Entities

        public virtual ICollection<Product> ProductsModified { get; set; }
        public virtual ICollection<Product> ProductsCreated { get; set; }
        public virtual ICollection<ApplicationUser> UsersModified { get; set; }
        public virtual ICollection<ApplicationUser> UsersCreated { get; set; }
        public virtual ICollection<Company> CompaniesModified { get; set; }
        public virtual ICollection<Company> CompaniesCreated { get; set; }
        public virtual ICollection<ProductCategory> ProductCategoriesModified { get; set; }
        public virtual ICollection<ProductCategory> ProductCategoriesCreated { get; set; }
        public virtual ICollection<FiscalYear> FiscalYearModified { get; set; }
        public virtual ICollection<FiscalYear> FiscalYearCreated { get; set; }
        public virtual ICollection<SalesOrder> SalesOrderModified { get; set; }
        public virtual ICollection<SalesOrder> SalesOrderCreated { get; set; }
        public virtual ICollection<SalesOrder> SalesOrdersBySalesRep { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrderModified { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrderCreated { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrderByPurchaseRep { get; set; }
        public virtual ICollection<SalesManager> SalesManagerModified { get; set; }
        public virtual ICollection<SalesManager> SalesManagerCreated { get; set; }
        public virtual ICollection<SecurityRight> SecurityRightCreated { get; set; } //user has these security rights
        public virtual ICollection<SecurityRight> SecurityRightModified { get; set; } //user has these security rights
        public virtual ICollection<SecurityRight> SecurityRightMemberUsers { get; set; }
        public virtual ICollection<Security> SecurityCreated { get; set; }
        public virtual ICollection<Security> SecurityModified { get; set; }
        public virtual ICollection<SecurityGroup> SecurityGroupCreated { get; set; }
        public virtual ICollection<SecurityGroup> SecurityGroupModified { get; set; }
        public virtual ICollection<SecurityGroupMember> SecurityGroupMemberCreated { get; set; }
        public virtual ICollection<SecurityGroupMember> SecurityGroupMemberModified { get; set; }
        public virtual ICollection<SecurityGroupMember> SecurityGroupMemberUsers { get; set; }
        public virtual ICollection<TransactionLog> TransactionLogCreated { get; set; }
        public virtual ICollection<TransactionLog> TransactionLogModified { get; set; }
        public virtual ICollection<ProductMetadata> ProductMetadataCreated { get; set; }
        public virtual ICollection<ProductMetadata> ProductMetadataModified { get; set; }
        public virtual ICollection<Address> AddressCreated { get; set; }
        public virtual ICollection<Address> AddressModified { get; set; }
        public virtual ICollection<Address> UserAddresses { get; set; }
        public virtual ICollection<FiscalPeriod> FiscalPeriodModified { get; set; }
        public virtual ICollection<FiscalPeriod> FiscalPeriodCreated { get; set; }
        public virtual ICollection<Customer> CustomerModified { get; set; }
        public virtual ICollection<Customer> CustomerCreated { get; set; }
        public virtual ICollection<CustomerLevel> CustomerLevelModified { get; set; }
        public virtual ICollection<CustomerLevel> CustomerLevelCreated { get; set; }
        public virtual ICollection<DeliveryMethod> DeliveryMethodModified { get; set; }
        public virtual ICollection<DeliveryMethod> DeliveryMethodCreated { get; set; }
        public virtual ICollection<PaymentTerm> PaymentTermModified { get; set; }
        public virtual ICollection<PaymentTerm> PaymentTermCreated { get; set; }
        public virtual ICollection<CustomerContactGroup> CustomerContactGroupCreated { get; set; }
        public virtual ICollection<CustomerContactGroup> CustomerContactGroupModified { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethodCreated { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethodModified { get; set; }
        public virtual ICollection<Account> AccountCreated { get; set; }
        public virtual ICollection<Account> AccountModified { get; set; }
        public virtual ICollection<Tax> TaxCreated { get; set; }
        public virtual ICollection<Tax> TaxModified { get; set; }
        public virtual ICollection<Vendor> VendorCreated { get; set; }
        public virtual ICollection<Vendor> VendorModified { get; set; }
        public virtual ICollection<EmailSetting> EmailSettingCreated { get; set; }
        public virtual ICollection<EmailSetting> EmailSettingModified { get; set; }
        public virtual ICollection<JournalMaster> JournalMasterCreated { get; set; }
        public virtual ICollection<JournalMaster> JournalMasterModified { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCartCreated { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCartModified { get; set; }
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetailCreated { get; set; }
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetailModified { get; set; }
        public virtual ICollection<EmailSender> EmailSenderCreated { get; set; }
        public virtual ICollection<EmailSender> EmailSenderModified { get; set; }
        public virtual ICollection<EcommerceSetting> EcommerceSettingCreated { get; set; }
        public virtual ICollection<EcommerceSetting> EcommerceSettingModified { get; set; }
        public virtual ICollection<Carousel> CarouselCreated { get; set; }
        public virtual ICollection<Carousel> CarouselModified { get; set; }
        public virtual ICollection<DeliveryZone> DeliveryZoneCreated { get; set; }
        public virtual ICollection<DeliveryZone> DeliveryZoneModified { get; set; }
        public virtual ICollection<ItemMeasure> ItemMeasureCreated { get; set; }
        public virtual ICollection<ItemMeasure> ItemMeasureModified { get; set; }
        public virtual ICollection<MeasureUnit> MeasureUnitCreated { get; set; }
        public virtual ICollection<MeasureUnit> MeasureUnitModified { get; set; }
        public virtual ICollection<DeliveryRate> DeliveryRateCreated { get; set; }
        public virtual ICollection<DeliveryRate> DeliveryRateModified { get; set; }
        public virtual ICollection<ProductPriceRule> ProductPriceRuleCreated { get; set; }
        public virtual ICollection<ProductPriceRule> ProductPriceRuleModified { get; set; }
        public virtual ICollection<CompanyWebSetting> CompanyWebSettingCreated { get; set; } 
        public virtual ICollection<CompanyWebSetting> CompanyWebSettingModified { get; set; }
        public virtual ICollection<Discount> DiscountCreated { get; set; }
        public virtual ICollection<Discount> DiscountModified { get; set; }
        public virtual ICollection<FeaturedItem> FeaturedItemCreated { get; set; }
        public virtual ICollection<FeaturedItem> FeaturedItemModified { get; set; }
        public virtual ICollection<FeaturedItemMetadata> FeaturedItemMetadataCreated { get; set; }
        public virtual ICollection<FeaturedItemMetadata> FeaturedItemMetadataModified { get; set; }
        public virtual ICollection<ProductsRefByKitAndAssembledType> ProductsRefByKitAndAssembledTypeCreated { get; set; }
        public virtual ICollection<ProductsRefByKitAndAssembledType> ProductsRefByKitAndAssembledTypeModified { get; set; }
        public virtual ICollection<Dashboard> DashboardCreated { get; set; }
        public virtual ICollection<Dashboard> DashboardModified { get; set; }
        public virtual ICollection<BugLogger> BugLoggerCreated { get; set; }
        public virtual ICollection<BugLogger> BugLoggerModified { get; set; }
        public virtual ICollection<TaxesInProduct> TaxesInProductsCreated { get; set; }
        public virtual ICollection<TaxesInProduct> TaxesInProductsModified { get; set; }
        public virtual ICollection<ProductInCategory> ProductInCategoriesCreated { get; set; }
        public virtual ICollection<ProductInCategory> ProductInCategoriesModified { get; set; }
        public virtual ICollection<CompanyLogo> CompanyLogoCreated { get; set; }
        public virtual ICollection<CompanyLogo> CompanyLogoModified { get; set; }
        public virtual ICollection<TaskManager> TaskManagerCreated { get; set; }
        public virtual ICollection<TaskManager> TaskManagerModified { get; set; }
        public virtual ICollection<TaskManager> TasksAssignedToUser { get; set; }
        public virtual ICollection<Retailer> RetailerCreated { get; set; }
        public virtual ICollection<Retailer> RetailerModified { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunityCreated { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunityModified { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunity { get; set;}
        public virtual ICollection<Stage> StageCreated { get; set; }
        public virtual ICollection<Stage> StageModified { get; set; }
        public virtual ICollection<Source> SourceCreated { get; set; }
        public virtual ICollection<Source> SourceModified { get; set; }
        public virtual ICollection<ReasonClosed> ReasonClosedCreated { get; set; }
        public virtual ICollection<ReasonClosed> ReasonClosedModified { get; set; }
        public virtual ICollection<Grade> GradeCreated { get; set; }
        public virtual ICollection<Grade> GradeModified { get; set; }
        public virtual ICollection<FooterSetting> FooterSettingCreated { get; set; }
        public virtual ICollection<FooterSetting> FooterSettingModified { get; set; }
        public virtual ICollection<GeneralSetting> GeneralSettingCreated { get; set; }
        public virtual ICollection<GeneralSetting> GeneralSettingModified { get; set; }
        public virtual ICollection<HeaderSetting> HeaderSettingCreated { get; set; }
        public virtual ICollection<HeaderSetting> HeaderSettingModified { get; set; }
        public virtual ICollection<IndividualSlideSetting> IndividualSlideSettingCreated { get; set; }
        public virtual ICollection<IndividualSlideSetting> IndividualSlideSettingModified { get; set; }
        public virtual ICollection<LayoutSetting> LayoutSettingCreated { get; set; }
        public virtual ICollection<LayoutSetting> LayoutSettingModified { get; set; }
        public virtual ICollection<SlideSetting> SlideSettingCreated { get; set; }
        public virtual ICollection<SlideSetting> SlideSettingModified { get; set; }
        public virtual ICollection<StoreLocator> StoreLocatorCreated { get; set; }
        public virtual ICollection<StoreLocator> StoreLocatorModified { get; set; }
        public virtual ICollection<BrandImage> BrandImageCreated { get; set; }
        public virtual ICollection<BrandImage> BrandImageModified { get; set; }
        public virtual ICollection<PersonnelSetting> PersonnelSettingCreated { get; set; }
        public virtual ICollection<PersonnelSetting> PersonnelSettingModified { get; set; }

        #region Enumerations Collections

        public virtual ICollection<AccountCashFlowTypes> AccountCashFlowTypeCreated { get; set; }
        public virtual ICollection<AccountCashFlowTypes> AccountCashFlowTypeModified { get; set; }
        public virtual ICollection<AccountDetailTypes> AccountDetailTypesCreated { get; set; }
        public virtual ICollection<AccountDetailTypes> AccountDetailTypesModified { get; set; }
        public virtual ICollection<AccountLevelTypes> AccountLevelTypesCreated { get; set; }
        public virtual ICollection<AccountLevelTypes> AccountLevelTypesModified { get; set; }
        public virtual ICollection<AccountTypes> AccountTypesCreated { get; set; }
        public virtual ICollection<AccountTypes> AccountTypesModified { get; set; }
        public virtual ICollection<AddressTypes> AddressTypesCreated { get; set; }
        public virtual ICollection<AddressTypes> AddressTypesModified { get; set; }
        public virtual ICollection<DescriptionTypes> DescriptionTypesCreated { get; set; }
        public virtual ICollection<DescriptionTypes> DescriptionTypesModified { get; set; }
        public virtual ICollection<DiscountTypes> DiscountTypesCreated { get; set; }
        public virtual ICollection<DiscountTypes> DiscountTypesModified { get; set; }
        public virtual ICollection<DueDateTypes> DueDateTypesCreated { get; set; }
        public virtual ICollection<DueDateTypes> DueDateTypesModified { get; set; }
        public virtual ICollection<DueTypes> DueTypesCreated { get; set; }
        public virtual ICollection<DueTypes> DueTypesModified { get; set; }
        public virtual ICollection<EncryptionTypes> EncryptionTypesCreated { get; set; }
        public virtual ICollection<EncryptionTypes> EncryptionTypesModified { get; set; }
        public virtual ICollection<JournalTypes> JournalTypesCreated { get; set; }
        public virtual ICollection<JournalTypes> JournalTypesModified { get; set; }
        public virtual ICollection<LockTypes> LockTypesCreated { get; set; }
        public virtual ICollection<LockTypes> LockTypesModified { get; set; }
        public virtual ICollection<MediaTypes> MediaTypesCreated { get; set; }
        public virtual ICollection<MediaTypes> MediaTypesModified { get; set; }
        public virtual ICollection<PaymentDiscountTypes> PaymentDiscountTypesCreated { get; set; }
        public virtual ICollection<PaymentDiscountTypes> PaymentDiscountTypesModified { get; set; }
        public virtual ICollection<PurchaseOrdersStatus> PurchaseOrdersStatusCreated { get; set; }
        public virtual ICollection<PurchaseOrdersStatus> PurchaseOrdersStatusModified { get; set; }
        public virtual ICollection<PurchaseOrderTypes> PurchaseOrderTypesCreated { get; set; }
        public virtual ICollection<PurchaseOrderTypes> PurchaseOrderTypesModified { get; set; }
        public virtual ICollection<SalesOrdersStatus> SalesOrdersStatusCreated { get; set; }
        public virtual ICollection<SalesOrdersStatus> SalesOrdersStatusModified { get; set; }
        public virtual ICollection<SalesOrderTypes> SalesOrderTypesCreated { get; set; }
        public virtual ICollection<SalesOrderTypes> SalesOrderTypesModified { get; set; }
        public virtual ICollection<SuffixTypes> SuffixTypesCreated { get; set; }
        public virtual ICollection<SuffixTypes> SuffixTypesModified { get; set; }
        public virtual ICollection<TaxCalculationTypes> TaxCalculationTypesCreated { get; set; }
        public virtual ICollection<TaxCalculationTypes> TaxCalculationTypesModified { get; set; }
        public virtual ICollection<TermTypes> TermTypesCreated { get; set; }
        public virtual ICollection<TermTypes> TermTypesModified { get; set; }
        public virtual ICollection<TitleTypes> TitleTypesCreated { get; set; }
        public virtual ICollection<TitleTypes> TitleTypesModified { get; set; }
        public virtual ICollection<UserTypes> UserTypesCreated { get; set; }
        public virtual ICollection<UserTypes> UserTypesModified { get; set; }
        public virtual ICollection<EntryMethodTypes> EntryMethodCreated { get; set; }
        public virtual ICollection<EntryMethodTypes> EntryMethodModified { get; set; }

        public virtual ICollection<ShippingStatus> ShippingStatusCreated { get; set; }
        public virtual ICollection<ShippingStatus> ShippingStatusModified { get; set; }

        public virtual ICollection<ShippingCalculationTypes> ShippingCalculationTypeCreated { get; set; }
        public virtual ICollection<ShippingCalculationTypes> ShippingCalculationTypeModified { get; set; }

        public virtual ICollection<DiscountCalculationTypes> DiscountCalculationTypeCreated { get; set; }
        public virtual ICollection<DiscountCalculationTypes> DiscountCalculationTypeModified { get; set; }

        public virtual ICollection<ImageTypes> ImageTypeCreated { get; set; }
        public virtual ICollection<ImageTypes> ImageTypeModified { get; set; }

        public virtual ICollection<Country> CountryCreated { get; set; }
        public virtual ICollection<Country> CountryModified { get; set; }

        #endregion

        #endregion
    }
}
