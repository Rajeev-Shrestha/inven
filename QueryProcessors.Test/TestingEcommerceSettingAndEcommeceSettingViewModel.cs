using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingEcommerceSettingAndEcommeceSettingViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Ecommerce_And_EcommerceViewModel(int id)
        {
            var vm = new EcommerceSetting()
            {
                Id = 1,
                DisplayQuantity= Hrevert.Common.Enums.DisplayQuantity.InStockOutStock,
                IncludeQuantityInSalesOrder =false,
                DisplayOutOfStockItems =false,
                ProductPerCategory =4,
                WebActive = true,
                CompanyId = 1
            };
            var mappedEcommerceSettingVm = new HrevertCRM.Data.Mapper.EcommerceSettingToEcommerceSettingViewModelMapper().Map(vm);
            var ecommerce = new HrevertCRM.Data.Mapper.EcommerceSettingToEcommerceSettingViewModelMapper().Map(mappedEcommerceSettingVm);

            //Test Account and mapped Account are same
            var res = true;

            PropertyInfo[] mappedproperties = ecommerce.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(ecommerce) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(ecommerce) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(ecommerce).Equals(propertyValuePair.GetValue(vm));
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
