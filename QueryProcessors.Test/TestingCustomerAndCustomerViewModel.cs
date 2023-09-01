using System.Reflection;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingCustomerAndCustomerViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_Customer_And_CustomerViewModel(int id)
        {
            var vm = new Customer()
            {
                Id = 1,
                CustomerCode = "ABC201",
                Password = "p@77w0rd",
                ConfirmPassword = "p@77w0rd",
                CustomerLevelId = 1,
                PaymentTermId = 1,
                PaymentMethodId = 12,
                OpeningBalance = 230,
                Note = "Test",
                TaxRegNo = "23244p0",
                DisplayName = "Raju",
                OnAccountId = 1,
                IsCodEnabled = false,
                IsPrepayEnabled = false,
                CompanyId = 1,
                BillingAddress = null,
                SalesOrders = null,
                CustomerInContactGroups=null,
                WebActive = true,
            };
            var mappedCustomerVm = new HrevertCRM.Data.Mapper.CustomerToCustomerViewModelMapper().Map(vm);
            var customer = new HrevertCRM.Data.Mapper.CustomerToCustomerViewModelMapper().Map(mappedCustomerVm);

            //Test Customer  and mappedCustomer  are same
            var res = true;

            PropertyInfo[] mappedproperties = customer.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(customer) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(customer) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(customer).Equals(propertyValuePair.GetValue(vm));
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
