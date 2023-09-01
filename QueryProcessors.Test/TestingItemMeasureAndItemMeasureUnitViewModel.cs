using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingItemMeasureAndItemMeasureUnitViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_ItemMeasure_And_ItemMeasureViewModel(int id)
        {
            var vm = new ItemMeasure()
            {
                Id = 1,
                ProductId = 1,
                MeasureUnitId = 1,
                Price = 100,
                WebActive = true,
                CompanyId = 1
            };
            var mappedItemMeasureVm = new HrevertCRM.Data.Mapper.ItemMeasureToItemMeasureViewModelMapper().Map(vm);
            var itemMeasure = new HrevertCRM.Data.Mapper.ItemMeasureToItemMeasureViewModelMapper().Map(mappedItemMeasureVm);

            //Test Account and mapped Account are same
            var res = true;

            PropertyInfo[] mappedproperties = itemMeasure.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(itemMeasure) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(itemMeasure) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(itemMeasure).Equals(propertyValuePair.GetValue(vm));
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
