using System.Reflection;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingAddressAndAddressViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Address_And_AddressViewModel(int id)
        {
            var vm = new Address()
            {
                Id = 1,
                UserId = "Xyz 123",
                AddressType = AddressType.Billing,
                FirstName = "Test 1",
                MiddleName = "Test 2",
                LastName = "Test 3",
                AddressLine1 = "Test 4",
                AddressLine2 = "Test 5",
                City = "Ktm",
                State = "Kansas",
                CountryId = 1,
                Telephone = "123455",
                ZipCode = "44021",
                MobilePhone = "98343234",
                Email = "abc@gmail.com",
                CustomerId = 1,
                VendorId = 2,
                IsDefault = true,
                CompanyId = 1

            };
            var mappedAddressVm = new HrevertCRM.Data.Mapper.AddressToAddressViewModelMapper().Map(vm);
            var address = new HrevertCRM.Data.Mapper.AddressToAddressViewModelMapper().Map(mappedAddressVm);

            //Test Address and mapped Address are same
            var res = true;

            PropertyInfo[] mappedproperties = address.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(address) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(address) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(address).Equals(propertyValuePair.GetValue(vm));
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
