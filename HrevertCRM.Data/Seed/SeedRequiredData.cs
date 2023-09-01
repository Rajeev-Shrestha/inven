using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Entities;
using HrevertCRM.Entities.Enumerations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HrevertCRM.Data.Seed
{
    public static class SeedRequiredData
    {
        public static async Task Seed(Company newCompany, ApplicationUser newUser, UserManager<ApplicationUser> userMgr, IServiceProvider serviceProvider, string envContentRootPath)
        {
            #region Seed Securities

            //Seed Security Rights, not dependant to companies and needs one time seed
            var secQueryProcessor = (SecurityQueryProcessor)serviceProvider.GetService(typeof(ISecurityQueryProcessor));
            if (!secQueryProcessor.GetActiveSecurities().Any())
            {
                var dataTexts = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\securities.json"));
                var securitiesList = JsonConvert.DeserializeObject<List<Security>>(dataTexts);
                secQueryProcessor.SaveAll(securitiesList);
            }

            #endregion

            #region Add Users to Security Groups

            var usersGroup = new List<ApplicationUser> { newUser };

            //Add Users to secuirty Group
            var securityGroupQueryProcessor = (SecurityGroupQueryProcessor)serviceProvider.GetService(typeof(ISecurityGroupQueryProcessor));
            var dataText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\securityGroups.json"));
            var securityGroups = JsonConvert.DeserializeObject<List<SecurityGroup>>(dataText);
            if (!securityGroupQueryProcessor.GetActiveSecurityGroups().Any(c => c.CompanyId == newCompany.Id))
            {
                securityGroups.ForEach(f => f.CompanyId = newCompany.Id);
                securityGroupQueryProcessor.SaveAll(securityGroups);
                //Add Users to secuirty groups
                var i = 0;
                var allGroupMembers = new List<SecurityGroupMember>();

                foreach (var user in usersGroup)
                {
                    var securityGroupMember = new SecurityGroupMember
                    {
                        CompanyId = newCompany.Id,
                        MemberId = user.Id,
                        SecurityGroupId = securityGroups[i].Id
                    };
                    allGroupMembers.Add(securityGroupMember);
                    i++;
                }
                var securityGroupMemberQueryProcessor = (SecurityGroupMemberQueryProcessor)serviceProvider.GetService(typeof(ISecurityGroupMemberQueryProcessor));
                securityGroupMemberQueryProcessor.SaveAllSeed(allGroupMembers);

            }

            #endregion

            #region Add All Respective Securities to Groups

            //Add all respective securities to groups
            var securityQueryProcessor = (SecurityQueryProcessor)serviceProvider.GetService(typeof(ISecurityQueryProcessor));
            var securities = securityQueryProcessor.GetActiveSecurities().ToList();
            const int adminGroupIndex = 0; //based on order in securityGroups.json
            var securityRightList = new List<SecurityRight>();

            try
            {
                securityRightList.AddRange(from securityIdPermisson in SecurityDefinition.SecurityDictionary
                    let secId = securities.FirstOrDefault(s => s.SecurityCode == (int) securityIdPermisson.Key).Id
                    where securityIdPermisson.Value.IsAdmin
                    select new SecurityRight
                    {
                        Allowed = true,
                        SecurityGroupId = securityGroups[adminGroupIndex].Id,
                        SecurityId = secId,
                        CompanyId = newCompany.Id,
                        UserId = newUser.Id
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Securities not seeded");
            }

            var securityRightQueryProcessor = (SecurityRightQueryProcessor)serviceProvider.GetService(typeof(ISecurityRightQueryProcessor));
            securityRightQueryProcessor.SaveAll(securityRightList);

            #endregion

            #region Seed Accounts

            //Seed Accounts
            var accountsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\accounts.json"));
            var accounts = JsonConvert.DeserializeObject<List<Account>>(accountsText);
            accounts.ForEach(cl => cl.CompanyId = newCompany.Id);
            var accountsQueryProcessor =
           (AccountQueryProcessor)serviceProvider.GetService(typeof(IAccountQueryProcessor));

            var rootAccounts = accounts.Where(a => a.ParentAccountId == null).ToList();
            accountsQueryProcessor.SaveAll(rootAccounts);
            foreach (var rootAccount in rootAccounts)
            {
                SaveAccountTree(rootAccount, accounts, accountsQueryProcessor);
            }

            #endregion

            #region Seed Taxes

            //Seed Taxes
            var taxesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\taxes.json"));
            var taxes = JsonConvert.DeserializeObject<List<Tax>>(taxesText);
            taxes.ForEach(cl => cl.CompanyId = newCompany.Id);
            var taxQueryProcessor =
            (TaxQueryProcessor)serviceProvider.GetService(typeof(ITaxQueryProcessor));
            taxQueryProcessor.SaveAll(taxes);

            #endregion

            #region Seed Email Setting

            //Seed Email Setting
            var emailSettingText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\emailSettings.json"));
            var emailSettings = JsonConvert.DeserializeObject<List<EmailSetting>>(emailSettingText);
            emailSettings.ForEach(es => es.CompanyId = newCompany.Id);

            var emailSettingQueryProcessor =
                (EmailSettingQueryProcessor)serviceProvider.GetService(typeof(IEmailSettingQueryProcessor));
            emailSettingQueryProcessor.SaveAllEmailSetting(emailSettings);

            #endregion

            #region Measure Unit Seeding

            //Seed Measure Units
            var measureUnitsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\measureUnits.json"));
            var measureUnits = JsonConvert.DeserializeObject<List<MeasureUnit>>(measureUnitsText);
            measureUnits.ForEach(cl => cl.CompanyId = newCompany.Id);
            var measureUnitQueryProcessor =
            (MeasureUnitQueryProcessor)serviceProvider.GetService(typeof(IMeasureUnitQueryProcessor));
            measureUnitQueryProcessor.SaveAll(measureUnits);

            #endregion

            #region Seed Delivery Methods

            //Seed Delivery Methods
            var deliveryMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\deliveryMethods.json"));
            var deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodsText);
            deliveryMethods.ForEach(cl => cl.CompanyId = newCompany.Id);

            var deliveryMethodQueryProcessor = (DeliveryMethodQueryProcessor)serviceProvider.GetService(typeof(IDeliveryMethodQueryProcessor));
            deliveryMethodQueryProcessor.SaveAll(deliveryMethods);

            #endregion

            #region Seed Payment Methods

            // Seed Payment Methods
            var paymentMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\paymentMethods.json"));
            var paymentMethods = JsonConvert.DeserializeObject<List<PaymentMethod>>(paymentMethodsText);
            paymentMethods.ForEach(cl => cl.CompanyId = newCompany.Id);

            var paymentMethodQueryProcessor = (PaymentMethodQueryProcessor)serviceProvider.GetService(typeof(IPaymentMethodQueryProcessor));
            paymentMethodQueryProcessor.SaveAll(paymentMethods);

            #endregion
            
            #region Seed Payment Terms

            // Seed Payment Terms
            var paymentTermsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\paymentTerms.json"));
            var paymentTerms = JsonConvert.DeserializeObject<List<PaymentTerm>>(paymentTermsText);
            paymentTerms.ForEach(cl => cl.CompanyId = newCompany.Id);

            var paymentTermQueryProcessor = (PaymentTermQueryProcessor)serviceProvider.GetService(typeof(IPaymentTermQueryProcessor));
            paymentTermQueryProcessor.SaveAll(paymentTerms);

            #endregion

            #region Delivery Zone Seeding

            //Seed Delivery Zones
            var deliveryZonesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\deliveryZones.json"));
            var deliveryZones = JsonConvert.DeserializeObject<List<DeliveryZone>>(deliveryZonesText);
            deliveryZones.ForEach(cl => cl.CompanyId = newCompany.Id);
            var deliveryZonQueryProcessor =
            (DeliveryZoneQueryProcessor)serviceProvider.GetService(typeof(IDeliveryZoneQueryProcessor));
            deliveryZonQueryProcessor.SaveAll(deliveryZones);

            #endregion

            #region Seed Customer Levels

            //Seed Customer Level
            var customerLevelText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\customerLevel.json"));
            var customerLevels = JsonConvert.DeserializeObject<List<CustomerLevel>>(customerLevelText);

            customerLevels.ForEach(cl => cl.CompanyId = newCompany.Id);
            var customerLevelQueryProcessor =
            (CustomerLevelQueryProcessor)serviceProvider.GetService(typeof(ICustomerLevelQueryProcessor));
            customerLevelQueryProcessor.SaveAll(customerLevels);

            #endregion

            #region Enumerations Seeding

            //Seed Title Types
            var titleTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\titleType.json"));
            var titleTypess = JsonConvert.DeserializeObject<List<TitleTypes>>(titleTypesText);
            titleTypess.ForEach(es => es.CompanyId = newCompany.Id);

            var titleTypesQueryProcessor =
                (TitleTypesQueryProcessor)serviceProvider.GetService(typeof(ITitleTypesQueryProcessor));
            titleTypesQueryProcessor.SaveAllTitleTypes(titleTypess);

            //Seed Suffix Types
            var suffixTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\suffixType.json"));
            var suffixTypess = JsonConvert.DeserializeObject<List<SuffixTypes>>(suffixTypesText);
            suffixTypess.ForEach(es => es.CompanyId = newCompany.Id);

            var suffixTypesQueryProcessor =
                (SuffixTypesQueryProcessor)serviceProvider.GetService(typeof(ISuffixTypesQueryProcessor));
            suffixTypesQueryProcessor.SaveAllSuffixTypes(suffixTypess);

            //Seed AccountCashFlow Types
            var accountCashFlowTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\accountCashFlowType.json"));
            var accountCashFlowTypes = JsonConvert.DeserializeObject<List<AccountCashFlowTypes>>(accountCashFlowTypesText);
            accountCashFlowTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var accountCashFlowTypesQueryProcessor =
                (AccountCashFlowTypesQueryProcessor)serviceProvider.GetService(typeof(IAccountCashFlowTypesQueryProcessor));
            accountCashFlowTypesQueryProcessor.SaveAllAccountCashFlowTypes(accountCashFlowTypes);

            //Seed Account Detail Types
            var accountDetailTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\accountDetailType.json"));
            var accountDetailTypes = JsonConvert.DeserializeObject<List<AccountDetailTypes>>(accountDetailTypesText);
            accountDetailTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var accountDetailTypesQueryProcessor =
                (AccountDetailTypesQueryProcessor)serviceProvider.GetService(typeof(IAccountDetailTypesQueryProcessor));
            accountDetailTypesQueryProcessor.SaveAllAccountDetailTypes(accountDetailTypes);

            //Seed Account Level Types
            var accountLevelTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\accountLevelType.json"));
            var accountLevelTypes = JsonConvert.DeserializeObject<List<AccountLevelTypes>>(accountLevelTypesText);
            accountLevelTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var accountLevelTypesQueryProcessor =
                (AccountLevelTypesQueryProcessor)serviceProvider.GetService(typeof(IAccountLevelTypesQueryProcessor));
            accountLevelTypesQueryProcessor.SaveAllAccountLevelTypes(accountLevelTypes);

            //Seed Account Types
            var accountTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\accountType.json"));
            var accountTypes = JsonConvert.DeserializeObject<List<AccountTypes>>(accountTypesText);
            accountTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var accountTypesQueryProcessor =
                (AccountTypesQueryProcessor)serviceProvider.GetService(typeof(IAccountTypesQueryProcessor));
            accountTypesQueryProcessor.SaveAllAccountTypes(accountTypes);

            //Seed Address Types
            var addressTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\addressType.json"));
            var addressTypes = JsonConvert.DeserializeObject<List<AddressTypes>>(addressTypesText);
            addressTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var addressTypesQueryProcessor =
                (AddressTypesQueryProcessor)serviceProvider.GetService(typeof(IAddressTypesQueryProcessor));
            addressTypesQueryProcessor.SaveAllAddressTypes(addressTypes);

            //Seed Description Types
            var descriptionTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\descriptionType.json"));
            var descriptionTypes = JsonConvert.DeserializeObject<List<DescriptionTypes>>(descriptionTypesText);
            descriptionTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var descriptionTypesQueryProcessor =
                (DescriptionTypesQueryProcessor)serviceProvider.GetService(typeof(IDescriptionTypesQueryProcessor));
            descriptionTypesQueryProcessor.SaveAllDescriptionTypes(descriptionTypes);

            //Seed Discount Types
            var discountTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\discountType.json"));
            var discountTypes = JsonConvert.DeserializeObject<List<DiscountTypes>>(discountTypesText);
            discountTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var discountTypesQueryProcessor =
                (DiscountTypesQueryProcessor)serviceProvider.GetService(typeof(IDiscountTypesQueryProcessor));
            discountTypesQueryProcessor.SaveAllDiscountTypes(discountTypes);

            //Seed Due Date Types
            var dueDateTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\dueDateType.json"));
            var dueDateTypes = JsonConvert.DeserializeObject<List<DueDateTypes>>(dueDateTypesText);
            dueDateTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var dueDateTypesQueryProcessor =
                (DueDateTypesQueryProcessor)serviceProvider.GetService(typeof(IDueDateTypesQueryProcessor));
            dueDateTypesQueryProcessor.SaveAllDueDateTypes(dueDateTypes);

            //Seed Due Types
            var dueTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\dueType.json"));
            var dueTypess = JsonConvert.DeserializeObject<List<DueTypes>>(dueTypesText);
            dueTypess.ForEach(es => es.CompanyId = newCompany.Id);

            var dueTypesQueryProcessor =
                (DueTypesQueryProcessor)serviceProvider.GetService(typeof(IDueTypesQueryProcessor));
            dueTypesQueryProcessor.SaveAllDueTypes(dueTypess);

            //Seed Encryption Types
            var encryptionTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\encryptionType.json"));
            var encryptionTypes = JsonConvert.DeserializeObject<List<EncryptionTypes>>(encryptionTypesText);
            encryptionTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var encryptionTypesQueryProcessor =
                (EncryptionTypesQueryProcessor)serviceProvider.GetService(typeof(IEncryptionTypesQueryProcessor));
            encryptionTypesQueryProcessor.SaveAllEncryptionTypes(encryptionTypes);

            //Seed Journal Types
            var journalTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\journalType.json"));
            var journalTypes = JsonConvert.DeserializeObject<List<JournalTypes>>(journalTypesText);
            journalTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var journalTypesQueryProcessor =
                (JournalTypesQueryProcessor)serviceProvider.GetService(typeof(IJournalTypesQueryProcessor));
            journalTypesQueryProcessor.SaveAllJournalTypes(journalTypes);

            //Seed Lock Types
            var lockTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\lockType.json"));
            var lockTypes = JsonConvert.DeserializeObject<List<LockTypes>>(lockTypesText);
            lockTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var lockTypesQueryProcessor =
                (LockTypesQueryProcessor)serviceProvider.GetService(typeof(ILockTypesQueryProcessor));
            lockTypesQueryProcessor.SaveAllLockTypes(lockTypes);

            //Seed Media Types
            var mediaTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\mediaType.json"));
            var mediaTypes = JsonConvert.DeserializeObject<List<MediaTypes>>(mediaTypesText);
            mediaTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var mediaTypesQueryProcessor =
                (MediaTypesQueryProcessor)serviceProvider.GetService(typeof(IMediaTypesQueryProcessor));
            mediaTypesQueryProcessor.SaveAllMediaTypes(mediaTypes);


            //Seed Payment Discount Types
            var paymentDiscountTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\paymentDiscountType.json"));
            var paymentDiscountTypes = JsonConvert.DeserializeObject<List<PaymentDiscountTypes>>(paymentDiscountTypesText);
            paymentDiscountTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var paymentDiscountTypesQueryProcessor =
                (PaymentDiscountTypesQueryProcessor)serviceProvider.GetService(typeof(IPaymentDiscountTypesQueryProcessor));
            paymentDiscountTypesQueryProcessor.SaveAllPaymentDiscountTypes(paymentDiscountTypes);

            //Seed PurchaseOrder Types
            var purchaseOrderTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\purchaseOrderType.json"));
            var purchaseOrderTypes = JsonConvert.DeserializeObject<List<PurchaseOrderTypes>>(purchaseOrderTypesText);
            purchaseOrderTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var purchaseOrderTypesQueryProcessor =
                (PurchaseOrderTypesQueryProcessor)serviceProvider.GetService(typeof(IPurchaseOrderTypesQueryProcessor));
            purchaseOrderTypesQueryProcessor.SaveAllPurchaseOrderTypes(purchaseOrderTypes);

            //Seed SalesOrder Types
            var salesOrderTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\salesOrderType.json"));
            var salesOrderTypess = JsonConvert.DeserializeObject<List<SalesOrderTypes>>(salesOrderTypesText);
            salesOrderTypess.ForEach(es => es.CompanyId = newCompany.Id);

            var salesOrderTypesQueryProcessor =
                (SalesOrderTypesQueryProcessor)serviceProvider.GetService(typeof(ISalesOrderTypesQueryProcessor));
            salesOrderTypesQueryProcessor.SaveAllSalesOrderTypes(salesOrderTypess);

            //Seed TaxCalculation Types
            var taxCalculationTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\taxCalculationType.json"));
            var taxCalculationTypes = JsonConvert.DeserializeObject<List<TaxCalculationTypes>>(taxCalculationTypesText);
            taxCalculationTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var taxCalculationTypesQueryProcessor =
                (TaxCalculationTypesQueryProcessor)serviceProvider.GetService(typeof(ITaxCalculationTypesQueryProcessor));
            taxCalculationTypesQueryProcessor.SaveAllTaxCalculationTypes(taxCalculationTypes);

            //Seed Term Types
            var termTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\termType.json"));
            var termTypes = JsonConvert.DeserializeObject<List<TermTypes>>(termTypesText);
            termTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var termTypesQueryProcessor =
                (TermTypesQueryProcessor)serviceProvider.GetService(typeof(ITermTypesQueryProcessor));
            termTypesQueryProcessor.SaveAllTermTypes(termTypes);

            //Seed User Types
            var userTypesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\userType.json"));
            var userTypes = JsonConvert.DeserializeObject<List<UserTypes>>(userTypesText);
            userTypes.ForEach(es => es.CompanyId = newCompany.Id);

            var userTypesQueryProcessor =
                (UserTypesQueryProcessor)serviceProvider.GetService(typeof(IUserTypesQueryProcessor));
            userTypesQueryProcessor.SaveAllUserTypes(userTypes);

            //Seed SalesOrderStatus 
            var salesOrderStatusText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\salesOrderStatus.json"));
            var salesOrderStatus = JsonConvert.DeserializeObject<List<SalesOrdersStatus>>(salesOrderStatusText);
            salesOrderStatus.ForEach(es => es.CompanyId = newCompany.Id);

            var salesOrderStatusQueryProcessor =
                (SalesOrdersStatusQueryProcessor)serviceProvider.GetService(typeof(ISalesOrdersStatusQueryProcessor));
            salesOrderStatusQueryProcessor.SaveAllSalesOrdersStatus(salesOrderStatus);

            //Seed SalesOrderStatus 
            var purchaseOrderStatusText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\purchaseOrderStatus.json"));
            var purchaseOrderStatus = JsonConvert.DeserializeObject<List<SalesOrdersStatus>>(purchaseOrderStatusText);
            purchaseOrderStatus.ForEach(es => es.CompanyId = newCompany.Id);

            var purchaseOrderStatusQueryProcessor =
                (SalesOrdersStatusQueryProcessor)serviceProvider.GetService(typeof(ISalesOrdersStatusQueryProcessor));
            purchaseOrderStatusQueryProcessor.SaveAllSalesOrdersStatus(purchaseOrderStatus);


            // Seed EntryMethodTypes

            var entryMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\entryMethodType.json"));
            var entryMethods = JsonConvert.DeserializeObject<List<EntryMethodTypes>>(entryMethodsText);
            entryMethods.ForEach(es => es.CompanyId = newCompany.Id);

            var entryMethodTypeQueryProcessor =
                (EntryMethodTypeQueryProcessor)serviceProvider.GetService(typeof(IEntryMethodTypeQueryProcessor));
            entryMethodTypeQueryProcessor.SaveAllEntryMethodTypes(entryMethods);

            // Seed shippingStatus
            var shippingStatusText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\shippingStatus.json"));
            var shippingStatus = JsonConvert.DeserializeObject<List<ShippingStatus>>(shippingStatusText);
            shippingStatus.ForEach(es => es.CompanyId = newCompany.Id);

            var shippingStatusQueryProcessor =
                (ShippingStatusQueryProcessor)serviceProvider.GetService(typeof(IShippingStatusQueryProcessor));
            shippingStatusQueryProcessor.SaveAllShippingStatus(shippingStatus);

            // Seed ShippingCalculationType
            var shippingCalcultationTypeText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\shippingCalculationType.json"));
            var shippingCalculationType = JsonConvert.DeserializeObject<List<ShippingCalculationTypes>>(shippingCalcultationTypeText);
            shippingCalculationType.ForEach(es => es.CompanyId = newCompany.Id);

            var shippingCalculationTypeQueryProcessor =
                (ShippingCalculationTypeQueryProcessor)serviceProvider.GetService(typeof(IShippingCalculationTypeQueryProcessor));
            shippingCalculationTypeQueryProcessor.SaveAllShippingCalculationType(shippingCalculationType);


            // Seed DiscountCalculationType
            var discountCalculationTypeText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\discountCalculationType.json"));
            var discountCalculationType = JsonConvert.DeserializeObject<List<DiscountCalculationTypes>>(discountCalculationTypeText);
            discountCalculationType.ForEach(es => es.CompanyId = newCompany.Id);

            var discountCalculationTypeQueryProcessor =
                (DiscountCalculationTypeQueryProcessor)serviceProvider.GetService(typeof(IDiscountCalculationTypeQueryProcessor));
            discountCalculationTypeQueryProcessor.SaveAllDiscountCalculationType(discountCalculationType);


            // Seed ImageType
            var imageTypeText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\imageType.json"));
            var imageType = JsonConvert.DeserializeObject<List<ImageTypes>>(imageTypeText);
            imageType.ForEach(es => es.CompanyId = newCompany.Id);

            var imageTypeQueryProcessor =
                (ImageTypeQueryProcessor)serviceProvider.GetService(typeof(IImageTypeQueryProcessor));
            imageTypeQueryProcessor.SaveAllImageTypes(imageType);


            #region Seed Country

            //Seed Country
            var countryLevelText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\country.json"));
            var countries = JsonConvert.DeserializeObject<List<Country>>(countryLevelText);

            countries.ForEach(cl => cl.CompanyId = newCompany.Id);
            var countryQueryProcessor =
                (CountryQueryProcessor)serviceProvider.GetService(typeof(ICountryQueryProcessor));
            countryQueryProcessor.SaveAll(countries);

            #endregion

            #endregion

            #region Seed Fiscal Years

            // Seed Fiscal Years
            var fiscalYearsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\fiscalYears.json"));
            var fiscalYears = JsonConvert.DeserializeObject<List<FiscalYear>>(fiscalYearsText);
            fiscalYears.ForEach(cl => cl.CompanyId = newCompany.Id);

            var fiscalYearQueryProcessor = (FiscalYearQueryProcessor)serviceProvider.GetService(typeof(IFiscalYearQueryProcessor));
            fiscalYearQueryProcessor.SaveAll(fiscalYears);

            #endregion

            #region Seed Fiscal Periods

            // Seed Fiscal Periods
            var fiscalPeriodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\fiscalPeriods.json"));
            var fiscalPeriods = JsonConvert.DeserializeObject<List<FiscalPeriod>>(fiscalPeriodsText);
            fiscalPeriods.ForEach(cl => cl.CompanyId = newCompany.Id);

            var fiscalPeriodQueryProcessor = (FiscalPeriodQueryProcessor)serviceProvider.GetService(typeof(IFiscalPeriodQueryProcessor));
            fiscalPeriodQueryProcessor.SaveAll(fiscalPeriods);

            #endregion

        }
        #region Save Account Tree

        private static void SaveAccountTree(Account rootAccount, List<Account> allAccounts, AccountQueryProcessor processor)
        {
            var childAccounts = allAccounts.Where(a => a.ParentAccountId.ToString() == rootAccount.AccountCode).ToList();
            childAccounts.ForEach(a => a.ParentAccountId = rootAccount.Id);
            processor.SaveAll(childAccounts);
            foreach (var child in childAccounts)
            {
                SaveAccountTree(child, allAccounts, processor);
            }
        }

        #endregion
    }
}
