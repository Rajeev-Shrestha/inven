using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingVendorAndVendorViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Vendor_And_VendorViewModel(int id)
        {
            var vm = new Vendor()
            {
                Id = 1,
                Code ="XYZ121",
                CreditLimit = 201,
                Debit=211,
                Credit = 220,
                ContactName = "Test 2",
                PaymentTermId = 1,
                PaymentMethodId = 121,
                Active = true,
                WebActive = true,
                CompanyId = 1
            };
            var mappedVendorVm = new HrevertCRM.Data.Mapper.VendorToVendorViewModelMapper().Map(vm);
            var vendor = new HrevertCRM.Data.Mapper.VendorToVendorViewModelMapper().Map(mappedVendorVm);

            //Test Vendor and mappedVendor are same
            var res = true;

            PropertyInfo[] mappedproperties = vendor.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(vendor) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(vendor) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(vendor).Equals(propertyValuePair.GetValue(vm));
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
