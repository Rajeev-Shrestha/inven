using HrevertCRM.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QueryProcessor.Test
{
    public class TestingDeliveryZoneAndDeliveryZoneQueryProcessor
    {
        private readonly DeliveryZoneQueryProcessor _deliveryZoneQueryProcessor; 
         public TestingDeliveryZoneAndDeliveryZoneQueryProcessor(DeliveryZoneQueryProcessor deliveryZoneQueryProcessor)
        {
            _deliveryZoneQueryProcessor = deliveryZoneQueryProcessor;
        }

        [Theory]
        [InlineData(1)]
        public void TestingGettingAllDeliveryZone()
        {

        }
    }
}
