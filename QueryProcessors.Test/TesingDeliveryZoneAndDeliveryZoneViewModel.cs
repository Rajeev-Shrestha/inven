using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TesingDeliveryZoneAndDeliveryZoneViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_DeliveryZone_And_DeliveryZoneViewModel(int id)
        {
            var vm = new DeliveryZone()
            {
                Id = 1,
                ZoneCode ="KK",
                ZoneName ="Karma",
                WebActive = true,
                CompanyId= 1
            };
            var mappeddeliveryZoneVm = new HrevertCRM.Data.Mapper.DeliveryZoneToDeliveryZoneViewModelMapper().Map(vm);
            var deliveryZone = new HrevertCRM.Data.Mapper.DeliveryZoneToDeliveryZoneViewModelMapper().Map(mappeddeliveryZoneVm);

            //Test DeliveryZone and mappedDeliveryZone are same
            var res = true;

            PropertyInfo[] mappedproperties = deliveryZone.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(deliveryZone) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(deliveryZone) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(deliveryZone).Equals(propertyValuePair.GetValue(vm));
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
