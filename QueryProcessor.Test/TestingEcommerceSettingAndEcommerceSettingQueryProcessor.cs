using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QueryProcessor.Test
{
    public class TestingEcommerceSettingAndEcommerceSettingQueryProcessor
    {
        private readonly EcommerceSettingQueryProcessor _ecommerceSettingQueryProcessor;
        public TestingEcommerceSettingAndEcommerceSettingQueryProcessor(EcommerceSettingQueryProcessor ecommerceSettingQueryProcessor)
        {
            _ecommerceSettingQueryProcessor = ecommerceSettingQueryProcessor;
        }

        [Theory]
        [InlineData(1)]
        public void TestingEcommerceSettingSave(int id)
        {
            var vm = new EcommerceSetting
            {
                Id = 1,
                DisplayOutOfStockItems = false,
                IncludeQuantityInSalesOrder =true,
                DisplayQuantity = Hrevert.Common.Enums.DisplayQuantity.InStockOutStock,
                ProductPerCategory = 4,
                WebActive = true
            };
            
            var mappedEcommerSettingVm = _ecommerceSettingQueryProcessor.Save(vm);

            Assert.True(mappedEcommerSettingVm.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public void TestingEcommerceSettingUpdate(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingSaveAllEcommerceSettings(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingEcommerceSettingDeleteById(int id) {
            int i = 1;
            var mappedEcommerSettingVm = _ecommerceSettingQueryProcessor.GetEcommerceSetting(i);
            Assert.True(mappedEcommerSettingVm.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public void TestingGetEcommerceSettingById(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingGetAllEcommerceSettingViewModel(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingGetActiveEcommerceSettings(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingGetAllDeletedEcommerceSettings(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingGetAllEcommerceSettings(int id) { }

        [Theory]
        [InlineData(1)]
        public void TestingEcommerceSettingActivateById(int id) { }
    }
}
