using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingFeaturedItemAndFeaturedItemViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_FeaturedItem_And_FeaturedItemViewModel(int id)
        {
            var vm = new FeaturedItem()
            {
                Id = 1,
                ItemId = 1,
                ImageType = Hrevert.Common.Enums.ImageType.FullWidthImage,
                SortOrder =false,
                WebActive = true,
                CompanyId = 1
            };
            var mappedFeaturedItemVm = new HrevertCRM.Data.Mapper.FeaturedItemToFeaturedItemViewModelMapper().Map(vm);
            var featuredItem = new HrevertCRM.Data.Mapper.FeaturedItemToFeaturedItemViewModelMapper().Map(mappedFeaturedItemVm);

            //Test FeaturedItem and mapped FeaturedItem are same
            var res = true;

            PropertyInfo[] mappedproperties = featuredItem.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(featuredItem) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(featuredItem) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(featuredItem).Equals(propertyValuePair.GetValue(vm));
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
