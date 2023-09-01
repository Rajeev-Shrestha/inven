using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingCustomerAndCustomerLevelViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_CustomerLabel_And_CustomerLabelViewModel(int id)
        {
            var vm = new CustomerLevel()
            {
                Id = 1,
                Name = "Test 1",
                CompanyId = 1
            };
            var mappedCustomerLabelVm = new HrevertCRM.Data.Mapper.CustomerLevelToCustomerLevelViewModelMapper().Map(vm);
            var customerLabel = new HrevertCRM.Data.Mapper.CustomerLevelToCustomerLevelViewModelMapper().Map(mappedCustomerLabelVm);

            //Test CustomerLevel and mappedCustomerLevel are same
            var res = true;

            PropertyInfo[] mappedproperties = customerLabel.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(customerLabel) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(customerLabel) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(customerLabel).Equals(propertyValuePair.GetValue(vm));
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
