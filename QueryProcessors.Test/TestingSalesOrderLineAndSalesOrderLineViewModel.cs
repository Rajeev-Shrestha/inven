using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingSalesOrderLineAndSalesOrderLineViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_SalesOrderLine_And_SalesOrderLineViewModel(int id)
        {
            var vm = new SalesOrderLine()
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
            var mappedSalesOrderLineVm = new HrevertCRM.Data.Mapper.SalesOrderLineToSalesOrderLineViewModelMapper().Map(vm);
            var salesOrderLine = new HrevertCRM.Data.Mapper.SalesOrderLineToSalesOrderLineViewModelMapper().Map(mappedSalesOrderLineVm);

            //Test SalesOrderLine and mappedSalesOrderLine are same
            var res = true;

            PropertyInfo[] mappedproperties = salesOrderLine.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(salesOrderLine) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(salesOrderLine) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(salesOrderLine).Equals(propertyValuePair.GetValue(vm));
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
