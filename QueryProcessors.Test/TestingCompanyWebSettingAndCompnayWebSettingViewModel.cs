using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using HrevertCRM.Data.Mapper;
namespace ViewModels.Test
{
    public class TestingCompanyWebSettingAndCompnayWebSettingViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_CompanyWebSetting_And_CompanyWebSettingViewModel(int id)
        {
            var vm = new CompanyWebSetting()
            {
                Id = 1,
                ShippingCalculationType = Hrevert.Common.Enums.ShippingCalculationType.Maximum,
                DiscountCalculationType = Hrevert.Common.Enums.DiscountCalculationType.Maximum,
                SalesRepId = "1",
                AllowGuest = true,
                CustomerSerialNo = 12,
                VendorSerialNo = 12,
                WebActive = true,
                CompanyId = 1
            };
            var mappedCompanyVm = new CompanyWebSettingToCompanyWebSettingViewModelMapper().Map(vm);
            var company = new CompanyWebSettingToCompanyWebSettingViewModelMapper().Map(mappedCompanyVm);

            //Test Carousel and mapped Carousel are same
            var res = true;

            PropertyInfo[] mappedproperties = company.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(company) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(company) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(company).Equals(propertyValuePair.GetValue(vm));
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
