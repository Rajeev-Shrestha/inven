using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingPurchaseOrderAndPurchaseOrderViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_PurchaseOrder_And_PurchaseOrderViewModel(int id)
        {
            var vm = new PurchaseOrder()
            {
                Id = 1,
                SalesOrderNumber = "ABC123",
                OrderDate = DateTime.Parse("2016/11/11"),
                DueDate = DateTime.Parse("2016/11/11"),
                FiscalPeriodId = 1,
                PaymentTermId = 1,
                DeliveryMethodId = 1,
                WebActive = true,
                CompanyId = 1
            };
            var mappedPurchaseOrderVm = new HrevertCRM.Data.Mapper.PurchaseOrderToPurchaseOrderViewModelMapper().Map(vm);
            var purchaseOrder = new HrevertCRM.Data.Mapper.PurchaseOrderToPurchaseOrderViewModelMapper().Map(mappedPurchaseOrderVm);
           
            //Test Purchase Order and mappedPurchaseOrder are same
            var res = true;

            PropertyInfo[] mappedproperties = purchaseOrder.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(purchaseOrder) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(purchaseOrder) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(purchaseOrder).Equals(propertyValuePair.GetValue(vm));
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
