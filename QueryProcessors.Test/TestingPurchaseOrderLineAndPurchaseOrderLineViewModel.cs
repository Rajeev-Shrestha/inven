using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingPurchaseOrderLineAndPurchaseOrderLineViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_PurchaseOrderLine_And_PurchaseOrderLineViewModel(int id)
        {
            var vm = new PurchaseOrderLine()
            {
                Id = 1,
                Description = "ABC123",
                ItemPrice = 123,
                Shipped = true,
                ItemQuantity = 1211,
                ShippedQuantity = 121,
                LineOrder = 11,
                Discount = 121,
                TaxId = 121,
                PurchaseOrderId = 1,
                ProductId = 1,
                WebActive = true,
                CompanyId = 1
            };
            var mappedPurchaseOrderLineVm = new HrevertCRM.Data.Mapper.PurchaseOrderLineToPurchaseOrderLineViewModelMapper().Map(vm);
            var purchaseOrderLine = new HrevertCRM.Data.Mapper.PurchaseOrderLineToPurchaseOrderLineViewModelMapper().Map(mappedPurchaseOrderLineVm);

            //Test Purchase Order Line and mappedPurchaseOrderLine are same
            var res = true;

            PropertyInfo[] mappedproperties = purchaseOrderLine.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(purchaseOrderLine) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(purchaseOrderLine) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(purchaseOrderLine).Equals(propertyValuePair.GetValue(vm));
                    if (!res)
                    {
                        break;
                    }
                }

            }
            Assert.True(res);
        }
    }
}
