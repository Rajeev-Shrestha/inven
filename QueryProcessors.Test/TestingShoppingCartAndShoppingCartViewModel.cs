using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingShoppingCartAndShoppingCartViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_ShoppingCart_And_ShoppingCartViewModel(int id)
        {
            var vm = new ShoppingCart()
            {
               Id = 1,
               CustomerId = 1,
               HostIp = "192.168.2.175",
               Amount = 275,
               WebActive = true,
                CompanyId = 1
            };
            var mappedShoppingCartVm = new HrevertCRM.Data.Mapper.ShoppingCartToShoppingCartViewModelMapper().Map(vm);
            var shoppingCart = new HrevertCRM.Data.Mapper.ShoppingCartToShoppingCartViewModelMapper().Map(mappedShoppingCartVm);
           
            //Test Shopping Cart and mapped Shopping Cart are same
            var res = true;

            PropertyInfo[] mappedproperties = shoppingCart.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(shoppingCart) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(shoppingCart) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(shoppingCart).Equals(propertyValuePair.GetValue(vm));
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
