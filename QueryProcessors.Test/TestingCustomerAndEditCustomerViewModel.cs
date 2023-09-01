using System.Collections.Generic;
using System.Reflection;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingCustomerAndEditCustomerViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Customer_And_EditCustomerViewModel(int id)
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

            var addList = new List<Address> { address };
            var vm = new Customer()
            {
                Id = 1,
                CustomerCode = "ABC201",
                BillingAddressId = 1,
                Password = "p@77w0rd",
                CustomerLevelId = 1,
                PaymentTermId = 1,
                PaymentMethodId = 12,
                OpeningBalance = 230,
                Note = "Test",
                TaxRegNo = "23244p0",
                DisplayName = "Raju",
                Addresses = addList,
                WebActive = true,
                CompanyId = 1
            };
            var mappedEditCustomerVm = new HrevertCRM.Data.Mapper.CustomerToEditCustomerViewModelMapper().Map(vm);
            var customerEdit = new HrevertCRM.Data.Mapper.CustomerToEditCustomerViewModelMapper().Map(mappedEditCustomerVm);

            //Test Customer  and mapped EditCustomer are same
            var res = true;

            PropertyInfo[] mappedproperties = customerEdit.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(customerEdit) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(customerEdit) == null)
                    {
                        res = false;
                        break;

                    }
                    if (propertyValuePair.Name.Equals("Addresses"))
                    {
                        break;
                    }

                    res = propertyValuePair.GetValue(customerEdit).Equals(propertyValuePair.GetValue(vm));
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
