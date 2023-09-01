using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingDeliveryMethodsAndDeliveryMethodsViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_DeliveryMethods_And_DeliveryMethodesViewModel(int id)
        {
            var vm = new DeliveryMethod()
            {
                Id = 1,
                DeliveryCode = "101",
                Description = "Test 1",
                WebActive = true,
                CompanyId = 1
            };
            var mappedDeliveryMethodsVm = new HrevertCRM.Data.Mapper.DeliveryMethodToDeliveryMethodViewModelMapper().Map(vm);
            var deliveryMethods = new HrevertCRM.Data.Mapper.DeliveryMethodToDeliveryMethodViewModelMapper().Map(mappedDeliveryMethodsVm);

            //Test DeliveryMethods and mappedDeliveryMethods are same
            var res = true;

            PropertyInfo[] mappedproperties = deliveryMethods.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(deliveryMethods) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(deliveryMethods) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(deliveryMethods).Equals(propertyValuePair.GetValue(vm));
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
