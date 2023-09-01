using System.Collections.Generic;

namespace HrevertCRM.Data
{
    public static class SecurityDefinition
    {
        public static Dictionary<SecurityId, SecurityIdWithDefaultGroupPermisson> SecurityDictionary = new Dictionary<SecurityId, SecurityIdWithDefaultGroupPermisson>
        {
            {SecurityId.AssignSecurityRight, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Assign Security Right"} },

            {SecurityId.AddUser, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add User"} },
            {SecurityId.ViewUsers, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Users"} },
            {SecurityId.DeleteUser, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete User"}},
            {SecurityId.UpdateUser, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update User"} },

            {SecurityId.AddFiscalYear, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Fiscal Year"} },
            {SecurityId.ViewFiscalYears, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Fiscal Years"} },
            {SecurityId.DeleteFiscalYear, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Fiscal Year"} },
            {SecurityId.UpdateFiscalYear, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Fiscal Year"}},

            //{SecurityId.AddCompanies, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Company"} },
            //{SecurityId.ViewCompanies, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Companies"} },
            //{SecurityId.DeleteCompanies, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Company"} },
            //{SecurityId.UpdateCompanies, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Company"}},

            {SecurityId.AddProductCategory, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Product Category"} },
            {SecurityId.ViewProductCategories, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View  Product Categories"} },
            {SecurityId.DeleteProductCategory, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete  Product Category"} },
            {SecurityId.UpdateProductCategory, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update  Product Category"} },

            {SecurityId.AddProduct,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Product"} },
            {SecurityId.ViewProducts, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Products"} },
            {SecurityId.DeleteProduct, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete Product"} },
            {SecurityId.UpdateProduct,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update Product"} },

            {SecurityId.AddPurchaseOrder, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = false, Description = "Add Purchase Order"} },
            {SecurityId.ViewPurchaseOrders, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Purchase Orders"} },
            {SecurityId.DeletePurchaseOrder, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = false, Description = "Delete Purchase Order"}},
            {SecurityId.UpdatePurchaseOrder, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = false, Description = "Update Purchase Order"} },

            {SecurityId.AddSalesOrder, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = false,IsSales = true, Description = "Add Sales Order"} },
            {SecurityId.ViewSalesOrders, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = false,IsSales = true, Description = "View Sales Orders"} },
            {SecurityId.DeleteSalesOrder,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = true, Description = "Delete Sales Order"} },
            {SecurityId.UpdateSalesOrder, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = true, Description = "Update Sales Order"}  },

            {SecurityId.AddFiscalPeriod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add FiscalPeriod"} },
            {SecurityId.ViewFiscalPeriods, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View FiscalPeriods"} },
            {SecurityId.DeleteFiscalPeriod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete FiscalPeriod"} },
            {SecurityId.UpdateFiscalPeriod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update FiscalPeriod"}},

            {SecurityId.AddAddress, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Address"} },
            {SecurityId.ViewAddresses, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Addresses"} },
            {SecurityId.DeleteAddress, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Address"} },
            {SecurityId.UpdateAddress, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Address"}},

            {SecurityId.AddCustomer, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Customer"} },
            {SecurityId.ViewCustomers, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Customers"} },
            {SecurityId.DeleteCustomer, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Customer"} },
            {SecurityId.UpdateCustomer, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Customer"}},

            {SecurityId.AddCustomerLevel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Customer Level"} },
            {SecurityId.ViewCustomerLevels, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Customer Levels"} },
            {SecurityId.DeleteCustomerLevel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Customer Level"} },
            {SecurityId.UpdateCustomerLevel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Customer Level"}},

            {SecurityId.AddPaymentTerm, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Payment Term"} },
            {SecurityId.ViewPaymentTerms, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Payment Terms"} },
            {SecurityId.DeletePaymentTerm, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Payment Term"} },
            {SecurityId.UpdatePaymentTerm, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Payment Term"}},

            {SecurityId.AddDeliveryMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Delivery Method"} },
            {SecurityId.ViewDeliveryMethods, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Delivery Methods"} },
            {SecurityId.DeleteDeliveryMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Delivery Method"} },
            {SecurityId.UpdateDeliveryMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Delivery Method"}},

            {SecurityId.AddSecurity, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Security"} },
            {SecurityId.ViewSecurities, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Securities"} },
            {SecurityId.DeleteSecurity, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Security"} },
            {SecurityId.UpdateSecurity, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Security"}},

            {SecurityId.AddSecurityRight, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Security Right"} },
            {SecurityId.ViewSecurityRights, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Security Rights"} },
            {SecurityId.DeleteSecurityRight, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Security Right"} },
            {SecurityId.UpdateSecurityRight, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Security Right"}},

            {SecurityId.AddSecurityGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Security Group"} },
            {SecurityId.ViewSecurityGroups, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Security Groups"} },
            {SecurityId.DeleteSecurityGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Security Group"} },
            {SecurityId.UpdateSecurityGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Security Group"}},

            {SecurityId.AddSecurityGroupMember, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Security Group Member"} },
            {SecurityId.ViewSecurityGroupMembers, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Security Group Members"} },
            {SecurityId.DeleteSecurityGroupMember, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Security Group Member"} },
            {SecurityId.UpdateSecurityGroupMember, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Security Group Member"}},

            {SecurityId.AddTransactionLog, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Transaction Log"} },
            {SecurityId.ViewTransactionLogs, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Transaction Logs"} },
            {SecurityId.DeleteTransactionLog, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Transaction Log"} },
            {SecurityId.UpdateTransactionLog, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Transaction Log"}},

            {SecurityId.AddPaymentMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Payment Method"} },
            {SecurityId.ViewPaymentMethods, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Payment Methods"} },
            {SecurityId.DeletePaymentMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Payment Method"} },
            {SecurityId.UpdatePaymentMethod, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Payment Method"}},

            {SecurityId.AddAccount, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Account"} },
            {SecurityId.ViewAccounts, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Accounts"} },
            {SecurityId.DeleteAccount, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Account"} },
            {SecurityId.UpdateAccount, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Account"}},

            {SecurityId.AddTax, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Tax"} },
            {SecurityId.ViewTaxes, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Taxes"} },
            {SecurityId.DeleteTax, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Tax"} },
            {SecurityId.UpdateTax, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Tax"}},

            {SecurityId.AddSalesOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Sales Order Line"} },
            {SecurityId.ViewSalesOrderLines, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Sales Order Lines"} },
            {SecurityId.DeleteSalesOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Sales Order Line"} },
            {SecurityId.UpdateSalesOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Sales Order Line"}},

            {SecurityId.AddPurchaseOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Purchase Order Line"} },
            {SecurityId.ViewPurchaseOrderLines, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Purchase Order Lines"} },
            {SecurityId.DeletePurchaseOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Purchase Order Line"} },
            {SecurityId.UpdatePurchaseOrderLine, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Purchase Order Line"}},

            {SecurityId.AddVendor, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Vendor"} },
            {SecurityId.ViewVendors, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Vendors"} },
            {SecurityId.DeleteVendor, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Vendor"} },
            {SecurityId.UpdateVendor, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Vendor"}},

            {SecurityId.AddEmailSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Email Setting"} },
            {SecurityId.ViewEmailSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Email Settings"} },
            {SecurityId.DeleteEmailSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Email Setting"} },
            {SecurityId.UpdateEmailSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Email Setting"}},

            {SecurityId.AddJournalMaster, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Journal Master"} },
            {SecurityId.ViewJournalMasters, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Journal Masters"} },
            {SecurityId.DeleteJournalMaster, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Journal Master"} },
            {SecurityId.UpdateJournalMaster, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Journal Master"}},

            {SecurityId.AddShoppingCart, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Shopping Cart"} },
            {SecurityId.ViewShoppingCarts, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Shopping Carts"} },
            {SecurityId.DeleteShoppingCart, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Shopping Cart"} },
            {SecurityId.UpdateShoppingCart, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Shopping Cart"}},

            {SecurityId.AddShoppingCartDetail, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Shopping Cart Detail"} },
            {SecurityId.ViewShoppingCartDetails, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Shopping Cart Details"} },
            {SecurityId.DeleteShoppingCartDetail, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Shopping Cart Detail"} },
            {SecurityId.UpdateShoppingCartDetail, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Shopping Cart Detail"}},

            {SecurityId.AddProductMetadata, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Product Metadata"} },
            {SecurityId.ViewProductMetadatas, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Product Metadatas"} },
            {SecurityId.DeleteProductMetadata, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Product Metadata"} },
            {SecurityId.UpdateProductMetadata, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Product Metadata"}},

            {SecurityId.AddCustomerContactGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Customer Contact Group"} },
            {SecurityId.ViewCustomerContactGroups, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Customer Contact Groups"} },
            {SecurityId.DeleteCustomerContactGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Customer Contact Group"} },
            {SecurityId.UpdateCustomerContactGroup, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Customer Contact Group"}},

            {SecurityId.AddECommerceSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add ECommerce Setting"} },
            {SecurityId.ViewECommerceSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View ECommerce Settings"} },
            {SecurityId.DeleteECommerceSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete ECommerce Setting"} },
            {SecurityId.UpdateECommerceSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update ECommerce Setting"}},

            {SecurityId.AddCarousel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Carousel"} },
            {SecurityId.ViewCarousels, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Carousels"} },
            {SecurityId.DeleteCarousel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Carousel"} },
            {SecurityId.UpdateCarousel, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Carousel"}},

            {SecurityId.AddDeliveryRate, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Delivery Rate"} },
            {SecurityId.ViewDeliveryRates, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Delivery Rates"} },
            {SecurityId.DeleteDeliveryRate, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Delivery Rate"} },
            {SecurityId.UpdateDeliveryRate, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Delivery Rate"}},

            {SecurityId.AddDeliveryZone, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Delivery Zone"} },
            {SecurityId.ViewDeliveryZones, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Delivery Zones"} },
            {SecurityId.DeleteDeliveryZone, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Delivery Zone"} },
            {SecurityId.UpdateDeliveryZone, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Delivery Zone"}},

            {SecurityId.AddItemMeasure, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Item Measure"} },
            {SecurityId.ViewItemMeasures, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Item Measures"} },
            {SecurityId.DeleteItemMeasure, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Item Measure"} },
            {SecurityId.UpdateItemMeasure, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Item Measure"}},

            {SecurityId.AddMeasureUnit, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Add Measure Unit"} },
            {SecurityId.ViewMeasureUnits, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Measure Units"} },
            {SecurityId.DeleteMeasureUnit, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Measure Unit"} },
            {SecurityId.UpdateMeasureUnit, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Measure Unit"}},

            {SecurityId.AddProductPriceRule,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Product Price Rule"} },
            {SecurityId.ViewProductPriceRules, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Product Price Rules"} },
            {SecurityId.DeleteProductPriceRule, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete Product Price Rule"} },
            {SecurityId.UpdateProductPriceRule,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update Product Price Rule"} },

            {SecurityId.AddCompanyWebSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Company Web Setting"} },
            {SecurityId.ViewCompanyWebSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Company Web Settings"} },
            {SecurityId.DeleteCompanyWebSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete Company Web Setting"} },
            {SecurityId.UpdateCompanyWebSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update Company Web Setting"} },

            {SecurityId.AddDiscount,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Discount"} },
            {SecurityId.ViewDiscounts, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Discounts"} },
            {SecurityId.DeleteDiscount, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete Discount"} },
            {SecurityId.UpdateDiscount,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update Discount"} },

            {SecurityId.AddFeaturedItem,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add FeaturedItem"} },
            {SecurityId.ViewFeaturedItems, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View FeaturedItems"} },
            {SecurityId.DeleteFeaturedItem, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete FeaturedItem"} },
            {SecurityId.UpdateFeaturedItem,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update FeaturedItem"} },

            {SecurityId.AddFeaturedItemMetadata,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add FeaturedItemMetadata"} },
            {SecurityId.ViewFeaturedItemMetadatas, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View FeaturedItemMetadatas"} },
            {SecurityId.DeleteFeaturedItemMetadata, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete FeaturedItemMetadata"} },
            {SecurityId.UpdateFeaturedItemMetadata,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update FeaturedItemMetadata"} },

            {SecurityId.AddBugLogger,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add BugLogger"} },
            {SecurityId.ViewBugLoggers, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View BugLoggers"} },
            {SecurityId.DeleteBugLogger, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete BugLogger"} },
            {SecurityId.UpdateBugLogger,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update BugLogger"} },

            {SecurityId.AddCompanyLogo,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add CompanyLogo"} },
            {SecurityId.ViewCompanyLogos, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View CompanyLogos"} },
            {SecurityId.DeleteCompanyLogo, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete CompanyLogo"} },
            {SecurityId.UpdateCompanyLogo,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update CompanyLogo"} },

            {SecurityId.AddCountry,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Add Country"} },
            {SecurityId.ViewCountries, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Countries"} },
            {SecurityId.DeleteCountry, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Delete Country"} },
            {SecurityId.UpdateCountry,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = true,IsSales = true, Description = "Update Country"} },

            {SecurityId.AddTask,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "Add Task"} },
            {SecurityId.ViewTasks, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Tasks"} },
            {SecurityId.DeleteTask, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Task"} },
            {SecurityId.UpdateTask,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "Update Task"} },
            {SecurityId.AssignTask,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Assign Task"} },

            {SecurityId.AddRetailer,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "Add Retailer"} },
            {SecurityId.ViewRetailers, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Retailers"} },
            {SecurityId.DeleteRetailer, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "Delete Retailer"} },
            {SecurityId.UpdateRetailer,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "Update Retailer"} },

            {SecurityId.AddGrade, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Grade" } },
            {SecurityId.ViewGrades, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Grades"} },
            {SecurityId.DeleteGrade, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Grade"} },
            {SecurityId.UpdateGrade,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Grade"} },

            {SecurityId.AddSource, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Source" } },
            {SecurityId.ViewSources, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Sources"} },
            {SecurityId.DeleteSource, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Source"} },
            {SecurityId.UpdateSource,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Source"} },

            {SecurityId.AddReasonClosed, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add ReasonClosed" } },
            {SecurityId.ViewReasonClosed, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View ReasonClosed"} },
            {SecurityId.DeleteReasonClosed, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete ReasonClosed"} },
            {SecurityId.UpdateReasonClosed,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update ReasonClosed"} },

            {SecurityId.AddStage, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Stage" } },
            {SecurityId.ViewStages, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Stages"} },
            {SecurityId.DeleteStage, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Stage"} },
            {SecurityId.UpdateStage,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Stage"} },

            {SecurityId.AddSalesOpportunity, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Sales Opportunity" } },
            {SecurityId.ViewSalesOpportunities, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Sales Opportunities"} },
            {SecurityId.DeleteSalesOpportunity, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Sales Opportunity"} },
            {SecurityId.UpdateSalesOpportunity,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Sales Opportunity"} },

            {SecurityId.AddBrand, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Brand" } },
            {SecurityId.ViewBrands, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Brands"} },
            {SecurityId.DeleteBrand, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Brand"} },
            {SecurityId.UpdateBrand,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Brand"} },

            {SecurityId.AddFooterSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Footer Setting" } },
            {SecurityId.ViewFooterSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Footer Settings"} },
            {SecurityId.DeleteFooterSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Footer Setting"} },
            {SecurityId.UpdateFooterSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Footer Setting"} },

            {SecurityId.AddGeneralSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add General Setting" } },
            {SecurityId.ViewGeneralSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View General Settings"} },
            {SecurityId.DeleteGeneralSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete General Setting"} },
            {SecurityId.UpdateGeneralSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update General Setting"} },

            {SecurityId.AddHeaderSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Header Setting" } },
            {SecurityId.ViewHeaderSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Header Settings"} },
            {SecurityId.DeleteHeaderSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Header Setting"} },
            {SecurityId.UpdateHeaderSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Header Setting"} },

            {SecurityId.AddIndividualSlideSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Individual Slide Setting" } },
            {SecurityId.ViewIndividualSlideSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Individual Slide Settings"} },
            {SecurityId.DeleteIndividualSlideSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Individual Slide Setting"} },
            {SecurityId.UpdateIndividualSlideSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Individual Slide Setting"} },

            {SecurityId.AddLayoutSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Layout Setting" } },
            {SecurityId.ViewLayoutSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Layout Settings"} },
            {SecurityId.DeleteLayoutSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Layout Setting"} },
            {SecurityId.UpdateLayoutSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Layout Setting"} },

            {SecurityId.AddSlideSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Slide Setting" } },
            {SecurityId.ViewSlideSettings, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Slide Settings"} },
            {SecurityId.DeleteSlideSetting, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Slide Setting"} },
            {SecurityId.UpdateSlideSetting,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Slide Setting"} },

            {SecurityId.AddStoreLocator, new SecurityIdWithDefaultGroupPermisson {IsAdmin=true, IsDistributor=false, IsPurchase=false, IsSales=false, Description="Add Store Locator" } },
            {SecurityId.ViewStoreLocators, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = true,IsPurchase = true,IsSales = true, Description = "View Store Locators"}},
            {SecurityId.DeleteStoreLocator, new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Delete Store Locator"} },
            {SecurityId.UpdateStoreLocator,new SecurityIdWithDefaultGroupPermisson {IsAdmin = true,IsDistributor = false,IsPurchase = false,IsSales = false, Description = "Update Store Locator"} },
        };
    }

    public enum SecurityId
    {
        //Assign Security Right Line
        AssignSecurityRight = 100001,

        //User line-1000
        AddUser = 1001,
        ViewUsers = 1002,
        DeleteUser = 1003,
        UpdateUser = 1004,

        //Fiscal Year line-2000
        AddFiscalYear = 2001,
        ViewFiscalYears = 2002,
        DeleteFiscalYear = 2003,
        UpdateFiscalYear = 2004,

        //Company line-3000, available to Super Admin Only, Do not yet include in seed
        AddCompany = 3001,
        ViewCompanies = 3002,
        DeleteCompany = 3003,
        UpdateCompany = 3004,

        //Product Category line-4000
        AddProductCategory = 4001,
        ViewProductCategories = 4002,
        DeleteProductCategory = 4003,
        UpdateProductCategory = 4004,

        //Product line-5000
        AddProduct = 5001,
        ViewProducts = 5002,
        DeleteProduct = 5003,
        UpdateProduct = 5004,

        //Purchase Order line-6000
        AddPurchaseOrder = 6001,
        ViewPurchaseOrders = 6002,
        DeletePurchaseOrder = 6003,
        UpdatePurchaseOrder = 6004,

        //Sales Order line-7000
        AddSalesOrder = 7001,
        ViewSalesOrders = 7002,
        DeleteSalesOrder = 7003,
        UpdateSalesOrder = 7004,

        //Fiscal Period line-8000
        AddFiscalPeriod = 8001,
        ViewFiscalPeriods = 8002,
        DeleteFiscalPeriod = 8003,
        UpdateFiscalPeriod = 8004,

        //Address line-9000
        AddAddress = 9001,
        ViewAddresses = 9002,
        DeleteAddress = 9003,
        UpdateAddress = 9004,

        //Customer line-10000
        AddCustomer = 10001,
        ViewCustomers = 10002,
        DeleteCustomer = 10003,
        UpdateCustomer = 10004,

        //Customer Level line-11000
        AddCustomerLevel = 11001,
        ViewCustomerLevels = 11002,
        DeleteCustomerLevel = 11003,
        UpdateCustomerLevel = 11004,

        //Payment Term line-12000
        AddPaymentTerm = 12001,
        ViewPaymentTerms = 12002,
        DeletePaymentTerm = 12003,
        UpdatePaymentTerm = 12004,

        //Delivery Method line-13000
        AddDeliveryMethod = 13001,
        ViewDeliveryMethods = 13002,
        DeleteDeliveryMethod = 13003,
        UpdateDeliveryMethod = 13004,

        //Security line-14000
        AddSecurity = 14001,
        ViewSecurities = 14002,
        DeleteSecurity = 14003,
        UpdateSecurity = 14004,

        //Security Right line-15000
        AddSecurityRight = 15001,
        ViewSecurityRights = 15002,
        DeleteSecurityRight = 15003,
        UpdateSecurityRight = 15004,

        //Security Group line-16000
        AddSecurityGroup = 16001,
        ViewSecurityGroups = 16002,
        DeleteSecurityGroup = 16003,
        UpdateSecurityGroup = 16004,

        //Security Group Member line-17000
        AddSecurityGroupMember = 17001,
        ViewSecurityGroupMembers = 17002,
        DeleteSecurityGroupMember = 17003,
        UpdateSecurityGroupMember = 17004,

        //Transaction Log line-18000
        AddTransactionLog = 18001,
        ViewTransactionLogs = 18002,
        DeleteTransactionLog = 18003,
        UpdateTransactionLog = 18004,

        //Payment Method line-19000
        AddPaymentMethod = 19001,
        ViewPaymentMethods = 19002,
        DeletePaymentMethod = 19003,
        UpdatePaymentMethod = 19004,

        //Account line-20000
        AddAccount = 20001,
        ViewAccounts = 20002,
        DeleteAccount = 20003,
        UpdateAccount = 20004,

        //Tax line-21000
        AddTax = 21001,
        ViewTaxes = 21002,
        DeleteTax = 21003,
        UpdateTax = 21004,

        //Sales Order Lines line-22000
        AddSalesOrderLine = 22001,
        ViewSalesOrderLines = 22002,
        DeleteSalesOrderLine = 22003,
        UpdateSalesOrderLine = 22004,

        //Purchase Order Lines line-23000
        AddPurchaseOrderLine = 23001,
        ViewPurchaseOrderLines = 23002,
        DeletePurchaseOrderLine = 23003,
        UpdatePurchaseOrderLine = 23004,

        //Email Setting line-24000
        AddEmailSetting = 24001,
        ViewEmailSettings = 24002,
        DeleteEmailSetting = 24003,
        UpdateEmailSetting = 24004,

        //Journal Master line-25000
        AddJournalMaster = 25001,
        ViewJournalMasters = 25002,
        DeleteJournalMaster = 25003,
        UpdateJournalMaster = 25004,

        //Shopping Cart line-26000
        AddShoppingCart = 26001,
        ViewShoppingCarts = 26002,
        DeleteShoppingCart = 26003,
        UpdateShoppingCart = 26004,

        //Shopping Cart Detail line-27000
        AddShoppingCartDetail = 27001,
        ViewShoppingCartDetails = 27002,
        DeleteShoppingCartDetail = 27003,
        UpdateShoppingCartDetail = 27004,

        //Product Metadata line-28000
        AddProductMetadata = 28001,
        ViewProductMetadatas = 28002,
        DeleteProductMetadata = 28003,
        UpdateProductMetadata = 28004,

        //Vendor line-29000
        AddVendor = 29001,
        ViewVendors = 29002,
        DeleteVendor = 29003,
        UpdateVendor = 29004,

        //Customer Contact Group line-30000
        AddCustomerContactGroup = 30001,
        ViewCustomerContactGroups = 30002,
        DeleteCustomerContactGroup = 30003,
        UpdateCustomerContactGroup = 30004,

        //ECommerceSetting line-31000
        AddECommerceSetting = 31001,
        ViewECommerceSettings = 31002,
        DeleteECommerceSetting = 31003,
        UpdateECommerceSetting = 31004,

        //Carousel line-32000
        AddCarousel = 32001,
        ViewCarousels = 32002,
        DeleteCarousel = 32003,
        UpdateCarousel = 32004,

        //DeliveryRate line-33000
        AddDeliveryRate = 33001,
        ViewDeliveryRates = 33002,
        DeleteDeliveryRate = 33003,
        UpdateDeliveryRate = 33004,

        //Delivery Zone line-34000
        AddDeliveryZone = 34001,
        ViewDeliveryZones = 34002,
        DeleteDeliveryZone = 34003,
        UpdateDeliveryZone = 34004,

        //Item Measure line-35000
        AddItemMeasure = 35001,
        ViewItemMeasures = 35002,
        DeleteItemMeasure = 35003,
        UpdateItemMeasure = 35004,

        //Measure Unit line-36000
        AddMeasureUnit = 36001,
        ViewMeasureUnits = 36002,
        DeleteMeasureUnit = 36003,
        UpdateMeasureUnit = 36004,

        //ProductPriceRule line-37000
        AddProductPriceRule = 37001,
        ViewProductPriceRules = 37002,
        DeleteProductPriceRule = 37003,
        UpdateProductPriceRule = 37004,

        //CompanyWebSetting line-38000
        AddCompanyWebSetting = 38001,
        ViewCompanyWebSettings = 38002,
        DeleteCompanyWebSetting = 38003,
        UpdateCompanyWebSetting = 38004,

        //Discount line-39000
        AddDiscount = 39001,
        ViewDiscounts = 39002,
        DeleteDiscount = 39003,
        UpdateDiscount = 39004,

        // FeaturedItem line -40000
        AddFeaturedItem = 40001,
        ViewFeaturedItems = 40002,
        DeleteFeaturedItem = 40003,
        UpdateFeaturedItem = 40004,

        // FeaturedItemMetadata line -41000
        AddFeaturedItemMetadata = 41001,
        ViewFeaturedItemMetadatas = 41002,
        DeleteFeaturedItemMetadata = 41003,
        UpdateFeaturedItemMetadata = 41004,

        // BugLogger line -42000
        AddBugLogger = 42001,
        ViewBugLoggers = 42002,
        DeleteBugLogger = 42003,
        UpdateBugLogger = 42004,

        // CompanyLogo line -43000
        AddCompanyLogo = 43001,
        ViewCompanyLogos = 43002,
        DeleteCompanyLogo = 43003,
        UpdateCompanyLogo = 43004,

        // Country line -44000
        AddCountry = 44001,
        ViewCountries = 44002,
        DeleteCountry = 44003,
        UpdateCountry = 44004,

        // Task Manager line -45000
        AddTask = 45001,
        ViewTasks = 45002,
        DeleteTask = 45003,
        UpdateTask = 45004,
        AssignTask = 45005,

        // Retailer line - 46000
        AddRetailer = 46001,
        ViewRetailers = 46002,
        DeleteRetailer = 46003,
        UpdateRetailer = 46004,

        // Grade Line - 47000
        AddGrade = 47001,
        ViewGrades = 47002,
        DeleteGrade = 47003,
        UpdateGrade = 47004,

        // Source Line= 48000
        AddSource = 48001,
        ViewSources = 48002,
        DeleteSource = 48003,
        UpdateSource = 48004,

        // ReasonClosed Line= 49000
        AddReasonClosed = 49001,
        ViewReasonClosed = 49002,
        DeleteReasonClosed = 49003,
        UpdateReasonClosed = 49004,

        // Stage Line= 50000
        AddStage = 50001,
        ViewStages = 50002,
        DeleteStage = 50003,
        UpdateStage = 50004,

        // SalesOpportunity Line= 51000
        AddSalesOpportunity = 51001,
        ViewSalesOpportunities = 51002,
        DeleteSalesOpportunity = 51003,
        UpdateSalesOpportunity = 51004,

        // Brand Line= 52000
        AddBrand = 52001,
        ViewBrands = 52002,
        DeleteBrand = 52003,
        UpdateBrand = 52004,

        // FooterSetting Line= 53000
        AddFooterSetting = 53001,
        ViewFooterSettings = 53002,
        DeleteFooterSetting = 53003,
        UpdateFooterSetting = 53004,

        // GeneralSetting Line= 54000
        AddGeneralSetting = 54001,
        ViewGeneralSettings = 54002,
        DeleteGeneralSetting = 54003,
        UpdateGeneralSetting = 54004,

        // HeaderSetting Line= 55000
        AddHeaderSetting = 55001,
        ViewHeaderSettings = 55002,
        DeleteHeaderSetting = 55003,
        UpdateHeaderSetting = 55004,

        // IndividualSlideSetting Line= 56000
        AddIndividualSlideSetting = 56001,
        ViewIndividualSlideSettings = 56002,
        DeleteIndividualSlideSetting = 56003,
        UpdateIndividualSlideSetting = 56004,

        // LayoutSetting Line= 57000
        AddLayoutSetting = 57001,
        ViewLayoutSettings = 57002,
        DeleteLayoutSetting = 57003,
        UpdateLayoutSetting = 57004,

        // SlideSetting Line= 58000
        AddSlideSetting = 58001,
        ViewSlideSettings = 58002,
        DeleteSlideSetting = 58003,
        UpdateSlideSetting = 58004,

        // StoreLocator Line= 59000
        AddStoreLocator = 59001,
        ViewStoreLocators = 59002,
        DeleteStoreLocator = 59003,
        UpdateStoreLocator = 59004
    }
}

