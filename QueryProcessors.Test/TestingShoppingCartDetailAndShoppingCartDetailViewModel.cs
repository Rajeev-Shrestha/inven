using System;
using System.Reflection;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingShoppingCartDetailAndShoppingCartDetailViewModel
    {
        [Fact]
        public void Testing_ShoppingCartDetail_And_ShoppingCartDetailViewModel()
        {
            var vm = new ShoppingCartDetail()
            {
                Id = 1,
                ProductId = 1,
                ProductCost = 256,
                TaxAmount = 543,
                Discount = 100,
                Quantity = 100,
                ShoppingCartId = 1,
                ShoppingDateTime = Convert.ToDateTime("2016/11/11"),
                WebActive = true,
                CompanyId = 1

            };

            var mappedShoppingCartDetailVm =
                new HrevertCRM.Data.Mapper.ShoppingCartDetailToShoppingCartDetailViewModelMapper().Map(vm);
            var shoppingCartDetail =
                new HrevertCRM.Data.Mapper.ShoppingCartDetailToShoppingCartDetailViewModelMapper().Map(
                    mappedShoppingCartDetailVm);

            // Test ShoppingCart Detail and mapped ShoppingCart Detail has to be same

            var result = true;

            var mappedProperties = shoppingCartDetail.GetType().GetProperties();

            foreach (var propertyValuePair in mappedProperties)
            {
                if (propertyValuePair.CanWrite) continue;

                if (propertyValuePair.GetValue(shoppingCartDetail) == null && propertyValuePair.GetValue(vm) == null)
                {
                    break;
                }
                if (propertyValuePair.GetValue(shoppingCartDetail) == null)
                {
                    result = false;
                    break;
                }
                result = propertyValuePair.GetValue(shoppingCartDetail).Equals(propertyValuePair.GetValue(vm));
            }
            Assert.True(result);
        }
    }
}
