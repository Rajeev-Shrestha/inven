using System.Collections.Generic;
using System.Reflection;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingVendorAndEditVendorViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Vendor_And_EditVendorViewModel(int id)
        {
            var address = new Address()
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
                VendorId = 121,
                IsDefault = true,
                CompanyId = 1
            };

            var addList = new List<Address> {address};
            var vm = new Vendor
            {
                Id = 1,
                Code = "XYZ121",
                CreditLimit = 201,
                Debit = 211,
                Credit = 220,
                ContactName = "Test 2",
                PaymentTermId = 1,
                PaymentMethodId = 121,
                Addresses = addList,
                WebActive = true,
                CompanyId = 1
            };

            var mappedEditVendorVm = new HrevertCRM.Data.Mapper.VendorToEditVendorViewModelMapper().Map(vm);
            var editVendor = new HrevertCRM.Data.Mapper.VendorToEditVendorViewModelMapper().Map(mappedEditVendorVm);

            //Test Edit Vendor and mapped Edit Vendor are same
            var res = true;

            var mappedproperties = editVendor.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (!propertyValuePair.CanWrite) continue;
                if (propertyValuePair.GetValue(editVendor) == null && propertyValuePair.GetValue(vm) == null)
                {
                    break;
                }
                if (propertyValuePair.GetValue(editVendor) == null)
                {
                    res = false;
                    break;

                }
                if (propertyValuePair.Name.Equals("Addresses"))
                {
                    //TODO: Ask Arjun Dai>>Since, Addresses has a list of address. It should be mapped property by property as well.
                    //for now we are just skipping it
                    break;
                }
                res = propertyValuePair.GetValue(editVendor).Equals(propertyValuePair.GetValue(vm));
                if (!res)
                {
                    break;
                }
            }
            Assert.True(res);

        
        }
    }
}
