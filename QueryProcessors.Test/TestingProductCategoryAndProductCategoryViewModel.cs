using System.Reflection;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingProductCategoryAndProductCategoryViewModel
    {

        [Fact]
        public void Testing_ProductCategory_And_ProductCategoryViewModel()
        {
            var vm = new ProductCategory
            {
                Id = 1,
                Name = "A Product",
                Description = "Test 1",
                CategoryRank = 1,
                ParentId = 1,
                WebActive = true,
                CompanyId = 1
            };
            var mappedProductCategoryVm = new HrevertCRM.Data.Mapper.ProductCategoryToProductCategoryViewModelMapper().Map(vm);
            var productCategory = new HrevertCRM.Data.Mapper.ProductCategoryToProductCategoryViewModelMapper().Map(mappedProductCategoryVm);

            //Test ProductCategory and mappedProductCategory are same
            var res = true;

            var mappedproperties = productCategory.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite) continue;

                if (propertyValuePair.GetValue(productCategory) == null && propertyValuePair.GetValue(vm) == null)
                {
                    break;
                }
                if (propertyValuePair.GetValue(productCategory) == null)
                {
                    res = false;
                    break;
                }

                res = propertyValuePair.GetValue(productCategory).Equals(propertyValuePair.GetValue(vm));
                if (!res)
                {
                    break;
                }
            }
            Assert.True(res);
        }
    }
}
