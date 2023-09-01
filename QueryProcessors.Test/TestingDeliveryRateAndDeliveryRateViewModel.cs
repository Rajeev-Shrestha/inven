using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingDeliveryRateAndDeliveryRateViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_DeliveryRate_And_DeliveryRateViewModel(int id)
        {
            var vm = new DeliveryRate()
            {
                Id = 1,
                DeliveryMethodId = 1,
                DeliveryZoneId = 1,
                WeightFrom = 1,
                WeightTo = 1,
                ProductCategoryId = 1,
                ProductId = 1,
                DocTotalFrom =100,
                DocTotalTo = 100,
                UnitFrom = 1,
                UnitTo = 1,
                MeasureUnitId = 1,   
                WebActive = true,
                CompanyId = 1
            };
            var mappedDeliveryRateVm = new HrevertCRM.Data.Mapper.DeliveryRateToDeliveryRateViewModelMapper().Map(vm);
            var deliveryRate = new HrevertCRM.Data.Mapper.DeliveryRateToDeliveryRateViewModelMapper().Map(mappedDeliveryRateVm);

            //Test DeliveryRate and mappedDeliveryRate are same
            var res = true;

            PropertyInfo[] mappedproperties = deliveryRate.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(deliveryRate) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(deliveryRate) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(deliveryRate).Equals(propertyValuePair.GetValue(vm));
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
