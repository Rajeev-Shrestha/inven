using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hrevert.Common.Enums;
using HrevertCRM.Data;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Entities;
using HrevertCRM.Entities.Enumerations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ShippingStatus = HrevertCRM.Entities.Enumerations.ShippingStatus;
using Task = System.Threading.Tasks.Task;

namespace HrevertCRM.Web.SeedValues
{
    public static class SeedingTrivialDatas
    {
        public static async Task Seed(Company newCompany, UserManager<ApplicationUser> userMgr,
            IServiceProvider serviceProvider, string envContentRootPath)
        {
            #region Seed Product and Product in Categories

            var productCategoryQueryProcessor =
                (ProductCategoryQueryProcessor)serviceProvider.GetService(typeof(IProductCategoryQueryProcessor));
            var dataText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\productsCategories.json"));
            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(dataText);
            if (!productCategoryQueryProcessor.GetAllProductCategories().Any(p => p.CompanyId == newCompany.Id))
            {
                categories.ForEach(f => f.CompanyId = newCompany.Id);
                categories.ForEach(f => f.ParentCategory.CompanyId = newCompany.Id); 
                productCategoryQueryProcessor.SaveAll(categories);
            }

            var productQueryProcessor =
                (ProductQueryProcessor)serviceProvider.GetService(typeof(IProductQueryProcessor));
            var dataText1 = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\products.json"));
            var products = JsonConvert.DeserializeObject<List<Product>>(dataText1);
            if (productQueryProcessor.GetProducts().All(p => p.CompanyId != newCompany.Id))
            {
                try
                {
                    products.ForEach(p => p.WebActive = true);
                    products.ForEach(p => p.ProductType = ProductType.Regular);
                    products.ForEach(f => f.CompanyId = newCompany.Id);
                    productQueryProcessor.SaveAllProduct(products);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // Add product in category
            var productIncategories = new List<ProductInCategory>();
            for (var i = 0; i < 3; i++)
            {

                foreach (var product in products)
                {
                    var productInCategory = new ProductInCategory
                    {
                        CategoryId = categories[i].Id,
                        ProductId = product.Id,
                        CompanyId = newCompany.Id

                    };
                    productIncategories.Add(productInCategory);
                }
            }
            var productInCategoryProcessor =
              (ProductInCategoryQueryProcessor)serviceProvider.GetService(typeof(IProductInCategoryQueryProcessor));
            productInCategoryProcessor.SaveAll(productIncategories);

            #endregion

            #region Seed Product Metadatas

            //Seed Product Metadatas
            var productMetadataText =
                File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\productMetadatas.json"));
            var productMetadatas = JsonConvert.DeserializeObject<List<ProductMetadata>>(productMetadataText);
            productMetadatas.ForEach(pm => pm.CompanyId = newCompany.Id);

            foreach (var metadata in productMetadatas)
            {
                if (metadata.MediaUrl.Contains("product-01"))
                {
                    metadata.ProductId = 1;
                }
                else if (metadata.MediaUrl.Contains("product-02"))
                {
                    metadata.ProductId = 2;
                }

                else if (metadata.MediaUrl.Contains("product-3"))
                {
                    metadata.ProductId = 3;
                }
                else if (metadata.MediaUrl.Contains("product-4"))
                {
                    metadata.ProductId = 4;
                }
                else if (metadata.MediaUrl.Contains("product-5"))
                {
                    metadata.ProductId = 5;
                }
                else if (metadata.MediaUrl.Contains("product-6"))
                {
                    metadata.ProductId = 6;
                }
                else if (metadata.MediaUrl.Contains("product-7"))
                {
                    metadata.ProductId = 7;
                }
                else if (metadata.MediaUrl.Contains("product-8"))
                {
                    metadata.ProductId = 8;
                }
                else if (metadata.MediaUrl.Contains("product-9"))
                {
                    metadata.ProductId = 9;
                }
                else if (metadata.MediaUrl.Contains("product-10"))
                {
                    metadata.ProductId = 10;
                }

                else if (metadata.MediaUrl.Contains("product-11"))
                {
                    metadata.ProductId = 11;
                }
                
                else
                {
                    metadata.ProductId = 12;
                }
            }
            var productMetadataQueryProcessor =
                (ProductMetadataQueryProcessor)serviceProvider.GetService(typeof(IProductMetadataQueryProcessor));
            productMetadataQueryProcessor.SaveAllProductMetadata(productMetadatas);

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

            #region Seed Customers

            // Seed Customers
            var customersText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\customers.json"));
            var customers = JsonConvert.DeserializeObject<List<Customer>>(customersText);
            customers.ForEach(cl => cl.CompanyId = newCompany.Id);
            var assignLevelId = 0;
            var assignPaymentTermId = 0;
            var assignPaymentMethodId = 0;

            foreach (var customer in customers)
            {
                customer.CustomerLevelId = customerLevels[assignLevelId].Id;
                assignLevelId++;
                if (assignLevelId > customerLevels.Count - 1) assignLevelId = 0;
                customer.PaymentTermId = paymentTerms[assignPaymentTermId].Id;
                assignPaymentTermId++;
                if (assignPaymentTermId > paymentTerms.Count - 1) assignPaymentTermId = 0;
                customer.PaymentMethodId = paymentMethods[assignPaymentMethodId].Id;
                assignPaymentMethodId++;
                if (assignPaymentMethodId > paymentMethods.Count - 1) assignPaymentMethodId = 0;

            }
            var customerQueryProcessor =
           (CustomerQueryProcessor)serviceProvider.GetService(typeof(ICustomerQueryProcessor));
            customerQueryProcessor.SaveAll(customers);

            #endregion

            #region Seed Customer Groups With Members

            //Seed  Customer Groups with members
            var customerGroupText =
                File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\customerGroups.json"));
            var customerGroups = JsonConvert.DeserializeObject<List<CustomerContactGroup>>(customerGroupText);
            customerGroups.ForEach(cg => cg.CompanyId = newCompany.Id);

            var assignCustomerGroupId = 0;
            var customerInContactGroups = new List<CustomerInContactGroup>();
            var customerGroupQueryProcessor =
            (CustomerContactGroupQueryProcessor)serviceProvider.GetService(typeof(ICustomerContactGroupQueryProcessor));
            customerGroupQueryProcessor.SaveAll(customerGroups);

            foreach (var customer in customers)
            {
                customerInContactGroups.Add(new CustomerInContactGroup
                {
                    CompanyId = customer.CompanyId,
                    ContactGroupId = customerGroups[assignCustomerGroupId].Id,
                    CustomerId = customer.Id
                });
                assignCustomerGroupId++;
                if (assignCustomerGroupId > customerGroups.Count - 1) assignCustomerGroupId = 0;
            }


            var customerInGroupQueryProcessor =
            (CustomerInContactGroupQueryProcessor)serviceProvider.GetService(typeof(ICustomerInContactGroupQueryProcessor));
            customerInGroupQueryProcessor.SaveAll(customerInContactGroups);

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
            
            #region Seed Delivery Methods

            //Seed Delivery Methods
            var deliveryMethodsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\deliveryMethods.json"));
            var deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodsText);
            deliveryMethods.ForEach(cl => cl.CompanyId = newCompany.Id);

            var deliveryMethodQueryProcessor = (DeliveryMethodQueryProcessor)serviceProvider.GetService(typeof(IDeliveryMethodQueryProcessor));
            deliveryMethodQueryProcessor.SaveAll(deliveryMethods);

            #endregion

            #region Seed Vendors

            // Seed Vendors
            var vendorsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Vendors.json"));
            var vendors = JsonConvert.DeserializeObject<List<Vendor>>(vendorsText);
            vendors.ForEach(cl => cl.CompanyId = newCompany.Id);

            var vendorQueryProcessor =
            (VendorQueryProcessor)serviceProvider.GetService(typeof(IVendorQueryProcessor));
            vendorQueryProcessor.SaveAll(vendors);

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

            #region Seed Addresses

            // Seed Addresses
            var addressesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\addresses.json"));
            var addresses = JsonConvert.DeserializeObject<List<Address>>(addressesText);
            addresses.ForEach(cl => cl.CompanyId = newCompany.Id);

            //save customer address
            var addressQueryProcessor = (AddressQueryProcessor)serviceProvider.GetService(typeof(IAddressQueryProcessor));
            var customerAddresses = addresses.Where(a => a.VendorId == null).ToList();
            var addressCount = 0;
            var assignDeliveryZoneId = 0;

            foreach (var customer in customers)
            {
                customerAddresses[addressCount].CustomerId = customer.Id;
                customerAddresses[addressCount + 1].CustomerId = customer.Id;
                addressCount = addressCount + 2;
            }

            foreach (var address in customerAddresses)
            {
                address.DeliveryZoneId = deliveryZones[assignDeliveryZoneId].Id;
                assignDeliveryZoneId ++;
                if (assignDeliveryZoneId > deliveryZones.Count - 1) assignDeliveryZoneId = 0;
            }

            addressQueryProcessor.SaveAllAddresses(customerAddresses);
            

            //save vendor address
            var vendorAddresses = addresses.Where(a => a.CustomerId == null).ToList();
            addressCount = 0;
            foreach (var vendor in vendors)
            {
                vendorAddresses[addressCount].VendorId = vendor.Id;
                vendorAddresses[addressCount + 1].VendorId = vendor.Id;
                addressCount = addressCount + 2;
            }
            addressQueryProcessor.SaveAllAddresses(vendorAddresses);
    
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

            #region Seed Sales Orders

            //Seed SalesOrder
            var salesOrdersText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\salesOrders.json"));
            var salesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(salesOrdersText);
            salesOrders.ForEach(cl => cl.CompanyId = newCompany.Id);
            assignPaymentTermId = 0;
            var assignFiscalPeriodId = 0;
            var assignDeliveryMethodId = 0;
            assignPaymentMethodId = 0;
            var assignSalesRepId = 0;
            var assignCustomerId = 0;
            var assignBillingAddressId = 0;
            var assignShippingAddressId = 0;
            var inc = 1;
            foreach (var salesOrder in salesOrders)
            {
                salesOrder.SalesOrderCode = "SI-201718-0000000" + inc++;
                salesOrder.PaymentMethodId = paymentMethods[assignPaymentMethodId].Id;
                assignPaymentMethodId++;
                if (assignPaymentMethodId > paymentMethods.Count - 1) assignPaymentMethodId = 0;
                salesOrder.PaymentTermId = paymentTerms[assignPaymentTermId].Id;
                assignPaymentTermId++;
                if (assignPaymentTermId > paymentTerms.Count - 1) assignPaymentTermId = 0;
                salesOrder.FiscalPeriodId = fiscalPeriods[assignFiscalPeriodId].Id;
                assignFiscalPeriodId++;
                if (assignFiscalPeriodId > fiscalPeriods.Count - 1) assignFiscalPeriodId = 0;
                salesOrder.DeliveryMethodId = deliveryMethods[assignDeliveryMethodId].Id;
                assignDeliveryMethodId++;
                if (assignDeliveryMethodId > deliveryMethods.Count - 1) assignDeliveryMethodId = 0;
                salesOrder.SalesRepId = userMgr.Users.ToList()[assignSalesRepId].Id;
                assignSalesRepId++;
                if (assignSalesRepId > userMgr.Users.ToList().Count - 1) assignSalesRepId = 0;
                salesOrder.CustomerId = customers[assignCustomerId].Id;
                assignCustomerId++;
                if (assignCustomerId > customers.Count - 1) assignCustomerId = 0;
                assignBillingAddressId++;
                salesOrder.BillingAddressId = customerAddresses[assignBillingAddressId].Id;
                if (assignBillingAddressId > customerAddresses.Count - 1) assignBillingAddressId = 0;
                salesOrder.ShippingAddressId = customerAddresses[assignShippingAddressId].Id;
                assignShippingAddressId++;
                if (assignShippingAddressId > customerAddresses.Count - 1) assignShippingAddressId = 0;
            }

            var salesOrderQueryProcessor =
           (SalesOrderQueryProcessor)serviceProvider.GetService(typeof(ISalesOrderQueryProcessor));
            salesOrderQueryProcessor.SaveAll(salesOrders);

            #endregion

            #region Seed Sales Order Lines

            //Seed SalesOrder Lines
            var salesOrderLinesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\salesOrderLines.json"));
            var salesOrderLines = JsonConvert.DeserializeObject<List<SalesOrderLine>>(salesOrderLinesText);
            salesOrderLines.ForEach(cl => cl.CompanyId = newCompany.Id);
            var assignTaxId = 0;
            var assignSalesOrderId = 0;
            var assignProductId = 0;

            foreach (var salesOrderLine in salesOrderLines)
            {
                salesOrderLine.TaxId = taxes[assignTaxId].Id;
                assignTaxId++;
                if (assignTaxId > taxes.Count - 1) assignTaxId = 0;
                salesOrderLine.SalesOrderId = salesOrders[assignSalesOrderId].Id;
                //assignSalesOrderId++;
                if (assignSalesOrderId > salesOrders.Count - 1) assignSalesOrderId = 0;
                salesOrderLine.ProductId = products[assignProductId].Id;
                assignProductId++;
                if (assignProductId > products.Count - 1) assignProductId = 0;
            }

            var salesOrderLineQueryProcessor =
           (SalesOrderLineQueryProcessor)serviceProvider.GetService(typeof(ISalesOrderLineQueryProcessor));
            salesOrderLineQueryProcessor.SaveAllSeed(salesOrderLines);

            #endregion

            #region Seed Company Web Setting

            //Seed Company Web Setting
            var companyWebSettingText =
                File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\companyWebSettings.json"));
            var companyWebSettings = JsonConvert.DeserializeObject<List<CompanyWebSetting>>(companyWebSettingText);
            companyWebSettings.ForEach(x => x.CompanyId = newCompany.Id);

            foreach (var companyWebSetting in companyWebSettings)
            {
                companyWebSetting.SalesRepId = userMgr.Users.FirstOrDefault(x => x.CompanyId == newCompany.Id).Id;
            }

            var companyWebSettingQueryProcessor =
                (CompanyWebSettingQueryProcessor)serviceProvider.GetService(typeof(ICompanyWebSettingQueryProcessor));
            companyWebSettingQueryProcessor.SaveAll(companyWebSettings);

            #endregion

            #region Seed Shopping Cart

            //Seed Shopping Cart
            var shoppingCartText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\shoppingCarts.json"));
            var shoppingCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(shoppingCartText);
            shoppingCarts.ForEach(cart => cart.CompanyId = newCompany.Id);
            assignCustomerId = 0;
            assignBillingAddressId = 0;
            assignShippingAddressId = 0;
            assignPaymentTermId = 0;
            assignDeliveryMethodId = 0;

            foreach (var cart in shoppingCarts)
            {
                cart.CustomerId = customers[assignCustomerId].Id;
                assignCustomerId++;
                if (assignCustomerId > customers.Count - 1) assignCustomerId = 0;
                cart.BillingAddressId = customerAddresses[assignBillingAddressId].Id;
                assignBillingAddressId++;
                if (assignBillingAddressId > customerAddresses.Count - 1) assignBillingAddressId = 0;
                cart.ShippingAddressId = customerAddresses[assignShippingAddressId].Id;
                assignShippingAddressId++;
                if (assignShippingAddressId > customerAddresses.Count - 1) assignShippingAddressId = 0;
                cart.PaymentTermId = paymentTerms[assignPaymentTermId].Id;
                assignPaymentTermId++;
                if (assignPaymentTermId > paymentTerms.Count - 1) assignPaymentTermId = 0;
                cart.DeliveryMethodId = deliveryMethods[assignDeliveryMethodId].Id;
                assignDeliveryMethodId++;
                if (assignDeliveryMethodId > deliveryMethods.Count - 1) assignDeliveryMethodId = 0;
            }
            var shoppingCartQueryProcessor =
                (ShoppingCartQueryProcessor)serviceProvider.GetService(typeof(IShoppingCartQueryProcessor));
            shoppingCartQueryProcessor.SaveAllShoppingCart(shoppingCarts);

            #endregion

            #region Seed Shopping Cart Detail

            //Seed Shopping Cart Detail
            var shoppingCartDetailText =
                File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\shoppingCartDetails.json"));
            var shoppingCartDetails = JsonConvert.DeserializeObject<List<ShoppingCartDetail>>(shoppingCartDetailText);
            shoppingCartDetails.ForEach(scd => scd.CompanyId = newCompany.Id);
            var assignShoppingCartId = 0;
            assignProductId = 0;

            foreach (var cartDetail in shoppingCartDetails)
            {
                cartDetail.ShoppingCartId = shoppingCarts[assignShoppingCartId].Id;
                if (assignShoppingCartId > shoppingCarts.Count - 1) assignShoppingCartId = 0;
                cartDetail.ProductId = products[assignProductId].Id;
                assignProductId++;
                if (assignProductId > products.Count - 1) assignProductId = 0;
                cartDetail.ShoppingDateTime = DateTime.Now;
            }
            var shoppingCartDetailQueryProcessor =
                (ShoppingCartDetailQueryProcessor)serviceProvider.GetService(typeof(IShoppingCartDetailQueryProcessor));
            shoppingCartDetailQueryProcessor.SaveAllShoppingCartDetail(shoppingCartDetails);

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

            #region Seed Journal Master
            //Seed Journal Master
            var journalMasterText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\journalMasters.json"));
                var journalMasters = JsonConvert.DeserializeObject<List<JournalMaster>>(journalMasterText);
                journalMasters.ForEach(jm => jm.CompanyId = newCompany.Id);

                assignFiscalPeriodId = 0;
                foreach (var master in journalMasters)
                {
                    master.FiscalPeriodId = fiscalPeriods[assignFiscalPeriodId].Id;
                    assignFiscalPeriodId++;
                    if (assignFiscalPeriodId > fiscalPeriods.Count - 1) assignFiscalPeriodId = 0;
                }
                var journalMasterQueryProcessor =
                    (JournalMasterQueryProcessor) serviceProvider.GetService(typeof(IJournalMasterQueryProcessor));
                journalMasterQueryProcessor.SaveAllJournalMaster(journalMasters);
            #endregion

            #region Seed Purchase Orders

            //Seed Purchase Orders
            var purchaseOrderText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\purchaseOrders.json"));
            var purchaseOrders = JsonConvert.DeserializeObject<List<PurchaseOrder>>(purchaseOrderText);
            purchaseOrders.ForEach(po => po.CompanyId = newCompany.Id);

            assignFiscalPeriodId = 0;
            assignPaymentTermId = 0;
            assignDeliveryMethodId = 0;
            assignBillingAddressId = 0;
            assignShippingAddressId = 0;
            var assignVendorId = 0;
            var assignPurchaseRepId = 0;
            inc = 1;
            foreach (var order in purchaseOrders)
            {
                order.PurchaseOrderCode = "PI-201718-0000000" + inc++;
                order.FiscalPeriodId = fiscalPeriods[assignFiscalPeriodId].Id;
                assignFiscalPeriodId++;
                if (assignFiscalPeriodId > fiscalPeriods.Count - 1) assignFiscalPeriodId = 0;
                order.PaymentTermId = paymentTerms[assignPaymentTermId].Id;
                assignPaymentTermId++;
                if (assignPaymentTermId > paymentTerms.Count - 1) assignPaymentTermId++;
                order.DeliveryMethodId = deliveryMethods[assignDeliveryMethodId].Id;
                assignDeliveryMethodId++;
                if (assignDeliveryMethodId > deliveryMethods.Count - 1) assignDeliveryMethodId = 0;
                order.BillingAddressId = customerAddresses[assignBillingAddressId].Id;
                assignBillingAddressId++;
                if (assignBillingAddressId > customerAddresses.Count - 1) assignBillingAddressId = 0;
                order.ShippingAddressId = customerAddresses[assignShippingAddressId].Id;
                assignShippingAddressId++;
                if (assignShippingAddressId > customerAddresses.Count - 1) assignShippingAddressId = 0;
                order.PurchaseRepId = userMgr.Users.ToList()[assignPurchaseRepId].Id;
                assignPurchaseRepId++;
                if (assignPurchaseRepId > userMgr.Users.ToList().Count - 1) assignPurchaseRepId = 0;
                order.VendorId = vendors[assignVendorId].Id;
                assignVendorId++;
                if (assignVendorId > vendors.Count - 1) assignVendorId = 0;
            }

            var purchaseOrderQueryProcessor =
                (PurchaseOrderQueryProcessor) serviceProvider.GetService(typeof(IPurchaseOrderQueryProcessor));
            purchaseOrderQueryProcessor.SaveAll(purchaseOrders);

            #endregion

            #region Seed Purchase Order Lines

            //Seed PurchaseOrder Lines
            var purchaseOrderLinesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\PurchaseOrderLines.json"));
            var purchaseOrderLines = JsonConvert.DeserializeObject<List<PurchaseOrderLine>>(purchaseOrderLinesText);
            purchaseOrderLines.ForEach(cl => cl.CompanyId = newCompany.Id);
            assignTaxId = 0;
            var assignPurchaseOrderId = 0;
            assignProductId = 0;

            foreach (var purchaseOrderLine in purchaseOrderLines)
            {
                purchaseOrderLine.TaxId = taxes[assignTaxId].Id;
                assignTaxId++;
                if (assignTaxId > taxes.Count - 1) assignTaxId = 0;
                purchaseOrderLine.PurchaseOrderId = purchaseOrders[assignPurchaseOrderId].Id;
                //assignPurchaseOrderId++;
                if (assignPurchaseOrderId > purchaseOrders.Count - 1) assignPurchaseOrderId = 0;
                purchaseOrderLine.ProductId = products[assignProductId].Id;
                assignProductId++;
                if (assignProductId > products.Count - 1) assignProductId = 0;
            }

            var purchaseOrderLineQueryProcessor =
           (PurchaseOrderLineQueryProcessor)serviceProvider.GetService(typeof(IPurchaseOrderLineQueryProcessor));
            purchaseOrderLineQueryProcessor.SaveAll(purchaseOrderLines);

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

            //Seed PurchaseOrderStatus 
            var purchaseOrderStatusText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Enums\purchaseOrderStatus.json"));
            var purchaseOrderStatus = JsonConvert.DeserializeObject<List<PurchaseOrdersStatus>>(purchaseOrderStatusText);
            purchaseOrderStatus.ForEach(es => es.CompanyId = newCompany.Id);

            var purchaseOrderStatusQueryProcessor =
                (PurchaseOrdersStatusQueryProcessor)serviceProvider.GetService(typeof(IPurchaseOrdersStatusQueryProcessor));
            purchaseOrderStatusQueryProcessor.SaveAllPurchaseOrdersStatus(purchaseOrderStatus);


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

            #region Measure Unit Seeding

            //Seed Measure Units
            var measureUnitsText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\measureUnits.json"));
            var measureUnits = JsonConvert.DeserializeObject<List<MeasureUnit>>(measureUnitsText);
            measureUnits.ForEach(cl => cl.CompanyId = newCompany.Id);
            var measureUnitQueryProcessor =
            (MeasureUnitQueryProcessor)serviceProvider.GetService(typeof(IMeasureUnitQueryProcessor));
            measureUnitQueryProcessor.SaveAll(measureUnits);

            #endregion

            #region Item Measure Seeding

            //Seed Item Measures
            var itemMeasuresText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\itemMeasures.json"));
            var itemMeasures = JsonConvert.DeserializeObject<List<ItemMeasure>>(itemMeasuresText);
            itemMeasures.ForEach(cl => cl.CompanyId = newCompany.Id);

            assignProductId = 0;
            var assignMeasureUnitId = 0;

            foreach (var itemMeasure in itemMeasures)
            {
                itemMeasure.ProductId = products[assignProductId].Id;
                assignProductId++;
                if (assignProductId > products.Count - 1) assignProductId = 0;

                itemMeasure.MeasureUnitId = measureUnits[assignMeasureUnitId].Id;
                assignMeasureUnitId++;if (assignMeasureUnitId > measureUnits.Count - 1) assignMeasureUnitId = 0;
            }

            var itemMeasureQueryProcessor =
            (ItemMeasureQueryProcessor)serviceProvider.GetService(typeof(IItemMeasureQueryProcessor));
            itemMeasureQueryProcessor.SaveAll(itemMeasures);

            #endregion

            #region TaskManager Seeding
            // seed Task Manager values
            var taskManagerText =  File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\taskManager.json"));
            var tasks = JsonConvert.DeserializeObject<List<TaskManager>>(taskManagerText);
            tasks.ForEach(po => po.CompanyId = newCompany.Id);
            var taskManagerQueryProcessor = (TaskManagerQueryProcessor)serviceProvider.GetService(typeof(ITaskManagerQueryProcessor));
            taskManagerQueryProcessor.SaveAll(tasks);
            #endregion

            #region sales Opportunities seeding

            // seed Source Values
            var sourceText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Sources.json"));
            var sources = JsonConvert.DeserializeObject<List<Source>>(sourceText);
            sources.ForEach(s => s.CompanyId = newCompany.Id);
            sources.ForEach(s => s.Active = true);
            var sourceQueryProcessor = (SourceQueryProcessor)serviceProvider.GetService(typeof(ISourceQueryProcessor));
            sourceQueryProcessor.SaveAll(sources);

            // seed Grade Values
            var gradeText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Grades.json"));
            var grades = JsonConvert.DeserializeObject<List<Grade>>(gradeText);
            grades.ForEach(s => s.CompanyId = newCompany.Id);
            sources.ForEach(s => s.Active = true);
            var gradeQueryProcessor = (GradeQueryProcessor)serviceProvider.GetService(typeof(IGradeQueryProcessor));
            gradeQueryProcessor.SaveAll(grades);
            // seed ReasonClosed Values
            var closedReasonText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\ReasonClosed.json"));
            var closedReasons = JsonConvert.DeserializeObject<List<ReasonClosed>>(closedReasonText);
            closedReasons.ForEach(s => s.CompanyId = newCompany.Id);
            sources.ForEach(s => s.Active = true);
            var reasonClosedQueryProcessor = (ReasonClosedQueryProcessor)serviceProvider.GetService(typeof(IReasonClosedQueryProcessor));
            reasonClosedQueryProcessor.SaveAll(closedReasons);
            // seed Stage Values
            var stageText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\Stages.json"));
            var stages = JsonConvert.DeserializeObject<List<Stage>>(stageText);
            sources.ForEach(s => s.Active = true);
            stages.ForEach(s => s.CompanyId = newCompany.Id);
            var stageQueryProcessor = (StageQueryProcessor)serviceProvider.GetService(typeof(IStageQueryProcessor));
            stageQueryProcessor.SaveAll(stages);

            //seeed salesOpportunities Values
            var salesOpportunitiesText = File.ReadAllText(Path.Combine(envContentRootPath, @"SeedValues\SalesOpportunities.json"));
            var salesOpportunities = JsonConvert.DeserializeObject<List<SalesOpportunity>>(salesOpportunitiesText);
            var saleesRepresentative = 0;
            salesOpportunities.ForEach(so => so.Active = true);
            salesOpportunities.ForEach(so => so.CompanyId = newCompany.Id);
            foreach (var salesOpportunity in salesOpportunities)
            {
                salesOpportunity.SalesRepresentative = userMgr.Users.ToList()[saleesRepresentative].Id;
                saleesRepresentative++;
            }
                var salesOpportunityQueryProcessor = (SalesOpportunityQueryProcessor)serviceProvider.GetService(typeof(ISalesOpportunityQueryProcessor));
            salesOpportunityQueryProcessor.SaveAll(salesOpportunities);
            

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