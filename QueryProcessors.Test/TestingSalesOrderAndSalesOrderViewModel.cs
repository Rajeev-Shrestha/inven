using System;
using System.Collections.Generic;
using System.Reflection;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingSalesOrderAndSalesOrderViewModel
    {
        [Fact]
        public void Testing_SalesOrder_And_SalesOrderViewModel()
        {
            SalesOrderLine sales = new SalesOrderLine()
            {
                Id = 1,
                Description = "ABC12",
                DescriptionType = DescriptionType.Modified,
                DiscountType = DiscountType.Fixed,
                ItemPrice = 10,
                Shipped = true,
                ItemQuantity = 10,
                ShippedQuantity = 120,
                LineOrder = 120,
                Discount = 1210,
                SalesOrderId = 121,
                ProductId = 1,
                WebActive = true,
                CompanyId = 1
            };

            List<SalesOrderLine> salesOrderLinesList = new List<SalesOrderLine>();
            salesOrderLinesList.Add(sales);

            var vm = new SalesOrder()
            {
                Id = 1,
                PurchaseOrderNumber = "ABC12",
                DueDate = DateTime.Parse("2016/11/13"),
                Status = SalesOrderStatus.CreditOrder,
                SalesPolicy = "Test 1",
                IsWebOrder = true,
                FullyPaid = true,
                TotalAmount = 121,
                PaidAmount = 11,
                BillingAddressId = 121,
                ShippingAddressId = 1,
                PaymentTermId = 1,
                FiscalPeriodId = 1,
                OrderType = SalesOrderType.Invoice,
                DeliveryMethodId = 1,
                CustomerId = 1,
                SalesRepId = "XYZ",
                InvoicedDate = DateTime.Parse("2016/11/13"),
                PaymentDueOn = DateTime.Parse("2016/11/13"),
                WebActive = true,
                SalesOrderLines = salesOrderLinesList,
                CompanyId = 1

            };
            var mappedSalesOrderVm = new HrevertCRM.Data.Mapper.SalesOrderToSalesOrderViewModelMapper().Map(vm);
            var salesOrder = new HrevertCRM.Data.Mapper.SalesOrderToSalesOrderViewModelMapper().Map(mappedSalesOrderVm);

            //Test SalesOrder and mappedSalesOrder are same
            var res = true;

            var mappedproperties = salesOrder.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite) continue; ;

                if (propertyValuePair.GetValue(salesOrder) == null && propertyValuePair.GetValue(vm) == null)
                {
                    break;
                }
                if (propertyValuePair.GetValue(salesOrder) == null)
                {
                    res = false;
                    break;
                }

                res = propertyValuePair.GetValue(salesOrder).Equals(propertyValuePair.GetValue(vm));
                if (!res)
                {
                    break;
                }
            }
            Assert.True(res);
        }
    }
}
