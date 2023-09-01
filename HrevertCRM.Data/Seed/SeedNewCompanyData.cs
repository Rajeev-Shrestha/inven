using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HrevertCRM.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace HrevertCRM.Data.Seed
{
    public static class SeedNewCompanyData
    {
        public static async Task Seed(Company newCompany, UserManager<ApplicationUser> userMgr, IServiceProvider serviceProvider, string envContentRootPath)
        {

            #region Create Admin, Sales, Purchase, Distributor Users            

            var demoUser = "demo";
            var demo = new ApplicationUser()
            {
                Address = "N/A",
                Email = demoUser,
                FirstName = "Demo",
                LastName = "User",
                Gender = 1,
                UserType = UserType.CompanyAdmin,
                UserName = demoUser,
                CompanyId = newCompany.Id,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(demo, "demo12345");

            var domain = "@" + newCompany.Name.Split(' ').FirstOrDefault();
            var adminEmail = "admin" + domain;
            var admin = new ApplicationUser()
            {
                Address = "N/A",
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                Gender = 1,
                UserType = UserType.CompanyAdmin,
                UserName = adminEmail,
                CompanyId = newCompany.Id,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(admin, "p@77w0rd");


            var salesEmail = "sale" + domain;
            var salesUser = new ApplicationUser()
            {
                Address = "N/A",
                Email = salesEmail,
                FirstName = "Sales",
                LastName = "Manager",
                Gender = 1,
                UserType = UserType.CompanyUsers,
                UserName = salesEmail,
                CompanyId = newCompany.Id,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(salesUser, "p@77w0rd");
            var purchaseEmail = "purchase" + domain;
            var purchaseUser = new ApplicationUser()
            {
                Address = "N/A",
                Email = purchaseEmail,
                FirstName = "Purchase",
                LastName = "Manager",
                Gender = 1,
                UserType = UserType.CompanyUsers,
                UserName = purchaseEmail,
                CompanyId = newCompany.Id,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(purchaseUser, "p@77w0rd");

            var distributorEmail = "distributor" + domain;
            var distributorUser = new ApplicationUser()
            {
                Address = "N/A",
                Email = distributorEmail,
                FirstName = "Distributor",
                LastName = "Manager",
                Gender = 1,
                UserType = UserType.CompanyUsers,
                UserName = distributorEmail,
                CompanyId = newCompany.Id,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(distributorUser, "p@77w0rd");

            #endregion

            #region Add Users to Security Groups

            var usersGroup = new List<ApplicationUser> { admin, salesUser, purchaseUser, distributorUser };

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
            const int salesGroupIndex = 1;
            const int purchaseGroupIndex = 2;
            const int distributorGroupIndex = 3;
            var securityRightList = new List<SecurityRight>();

            try
            {
                foreach (var securityIdPermisson in SecurityDefinition.SecurityDictionary)
                {
                    var secId = securities.FirstOrDefault(s => s.SecurityCode == (int)securityIdPermisson.Key).Id;
                    if (securityIdPermisson.Value.IsAdmin)
                    {
                        var securityRight = new SecurityRight
                        {
                            Allowed = true,
                            SecurityGroupId = securityGroups[adminGroupIndex].Id,
                            SecurityId = secId,
                            CompanyId = newCompany.Id,
                            UserId = admin.Id
                        };
                        securityRightList.Add(securityRight);
                    }
                    if (securityIdPermisson.Value.IsSales)
                    {
                        var securityRight = new SecurityRight
                        {
                            Allowed = true,
                            SecurityGroupId = securityGroups[salesGroupIndex].Id,
                            SecurityId = secId,
                            CompanyId = newCompany.Id,
                            UserId = salesUser.Id
                        };
                        securityRightList.Add(securityRight);
                    }
                    if (securityIdPermisson.Value.IsDistributor)
                    {
                        var securityRight = new SecurityRight
                        {
                            Allowed = true,
                            SecurityGroupId = securityGroups[distributorGroupIndex].Id,
                            SecurityId = secId,
                            CompanyId = newCompany.Id,
                            UserId = purchaseUser.Id
                        };
                        securityRightList.Add(securityRight);
                    }
                    if (!securityIdPermisson.Value.IsPurchase) continue;
                    {
                        var securityRight = new SecurityRight
                        {
                            Allowed = true,
                            SecurityGroupId = securityGroups[purchaseGroupIndex].Id,
                            SecurityId = secId,
                            CompanyId = newCompany.Id,
                            UserId = distributorUser.Id
                        };
                        securityRightList.Add(securityRight);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var securityRightQueryProcessor = (SecurityRightQueryProcessor)serviceProvider.GetService(typeof(ISecurityRightQueryProcessor));
            securityRightQueryProcessor.SaveAll(securityRightList);

            #endregion

           // #region Seed Accounts

           // //Seed Accounts
           // var accountsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\accounts.json"));
           // var accounts = JsonConvert.DeserializeObject<List<Account>>(accountsText);
           // accounts.ForEach(cl => cl.CompanyId = newCompany.Id);
           // var accountsQueryProcessor =
           //(AccountQueryProcessor)serviceProvider.GetService(typeof(IAccountQueryProcessor));

           // var rootAccounts = accounts.Where(a => a.ParentAccountId == null).ToList();
           // accountsQueryProcessor.SaveAll(rootAccounts);
           // foreach (var rootAccount in rootAccounts)
           // {
           //     SaveAccountTree(rootAccount, accounts, accountsQueryProcessor);
           // }

           // #endregion

           // #region Seed Taxes

           // //Seed Taxes
           // var taxesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\taxes.json"));
           // var taxes = JsonConvert.DeserializeObject<List<Tax>>(taxesText);
           // taxes.ForEach(cl => cl.CompanyId = newCompany.Id);
           // var taxQueryProcessor =
           // (TaxQueryProcessor)serviceProvider.GetService(typeof(ITaxQueryProcessor));
           // taxQueryProcessor.SaveAll(taxes);

           // #endregion

           // #region Seed Email Setting

           // //Seed Email Setting
           // var emailSettingText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\emailSettings.json"));
           // var emailSettings = JsonConvert.DeserializeObject<List<EmailSetting>>(emailSettingText);
           // emailSettings.ForEach(es => es.CompanyId = newCompany.Id);

           // var emailSettingQueryProcessor =
           //     (EmailSettingQueryProcessor)serviceProvider.GetService(typeof(IEmailSettingQueryProcessor));
           // emailSettingQueryProcessor.SaveAllEmailSetting(emailSettings);

           // #endregion

           // #region Measure Unit Seeding

           // //Seed Measure Units
           // var measureUnitsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\measureUnits.json"));
           // var measureUnits = JsonConvert.DeserializeObject<List<MeasureUnit>>(measureUnitsText);
           // measureUnits.ForEach(cl => cl.CompanyId = newCompany.Id);
           // var measureUnitQueryProcessor =
           // (MeasureUnitQueryProcessor)serviceProvider.GetService(typeof(IMeasureUnitQueryProcessor));
           // measureUnitQueryProcessor.SaveAll(measureUnits);

           // #endregion

           // #region Seed Delivery Methods

           // //Seed Delivery Methods
           // var deliveryMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\deliveryMethods.json"));
           // var deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodsText);
           // deliveryMethods.ForEach(cl => cl.CompanyId = newCompany.Id);

           // var deliveryMethodQueryProcessor = (DeliveryMethodQueryProcessor)serviceProvider.GetService(typeof(IDeliveryMethodQueryProcessor));
           // deliveryMethodQueryProcessor.SaveAll(deliveryMethods);

           // #endregion

           // #region Seed Payment Methods

           // // Seed Payment Methods
           // var paymentMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\paymentMethods.json"));
           // var paymentMethods = JsonConvert.DeserializeObject<List<PaymentMethod>>(paymentMethodsText);
           // paymentMethods.ForEach(cl => cl.CompanyId = newCompany.Id);

           // var paymentMethodQueryProcessor = (PaymentMethodQueryProcessor)serviceProvider.GetService(typeof(IPaymentMethodQueryProcessor));
           // paymentMethodQueryProcessor.SaveAll(paymentMethods);

           // #endregion

           // #region Seed Payment Terms

           // // Seed Payment Terms
           // var paymentTermsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\paymentTerms.json"));
           // var paymentTerms = JsonConvert.DeserializeObject<List<PaymentTerm>>(paymentTermsText);
           // paymentTerms.ForEach(cl => cl.CompanyId = newCompany.Id);

           // var paymentTermQueryProcessor = (PaymentTermQueryProcessor)serviceProvider.GetService(typeof(IPaymentTermQueryProcessor));
           // paymentTermQueryProcessor.SaveAll(paymentTerms);

           // #endregion

           // #region Delivery Zone Seeding

           // //Seed Delivery Zones
           // var deliveryZonesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\deliveryZones.json"));
           // var deliveryZones = JsonConvert.DeserializeObject<List<DeliveryZone>>(deliveryZonesText);
           // deliveryZones.ForEach(cl => cl.CompanyId = newCompany.Id);
           // var deliveryZonQueryProcessor =
           // (DeliveryZoneQueryProcessor)serviceProvider.GetService(typeof(IDeliveryZoneQueryProcessor));
           // deliveryZonQueryProcessor.SaveAll(deliveryZones);

           // #endregion

           // #region Seed Customer Levels

           // //Seed Customer Level
           // var customerLevelText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\customerLevel.json"));
           // var customerLevels = JsonConvert.DeserializeObject<List<CustomerLevel>>(customerLevelText);

           // customerLevels.ForEach(cl => cl.CompanyId = newCompany.Id);
           // var customerLevelQueryProcessor =
           // (CustomerLevelQueryProcessor)serviceProvider.GetService(typeof(ICustomerLevelQueryProcessor));
           // customerLevelQueryProcessor.SaveAll(customerLevels);

           // #endregion

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
