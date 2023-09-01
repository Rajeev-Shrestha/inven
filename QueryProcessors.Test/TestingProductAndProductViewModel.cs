using System.Reflection;

using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingProductAndProductViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Product_And_ProductViewModel(int id)
        {
            var vm = new Product()
            {
                Id = 1,
                Code = "A",
                CommissionRate = 1,
                Commissionable = true,
                Name = "A Product",
                ShortDescription = "Test",
                QuantityOnHand = 5,
                QuantityOnOrder = 5,
                UnitPrice = 10,
                WebActive = true,
                CompanyId = 1
            };
            var mappedProductVm = new HrevertCRM.Data.Mapper.ProductToProductViewModelMapper().Map(vm);
            var product = new HrevertCRM.Data.Mapper.ProductToProductViewModelMapper().Map(mappedProductVm);

            //Test Product and mappedProduct are same
            var res = true;

            PropertyInfo[] mappedproperties = product.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(product) == null && propertyValuePair.GetValue(vm)==null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(product) == null)
                    {
                        res = false;
                        break;

                    }
                      

                    res = propertyValuePair.GetValue(product).Equals(propertyValuePair.GetValue(vm));
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
