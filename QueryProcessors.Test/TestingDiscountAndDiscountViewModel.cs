using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingDiscountAndDiscountViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Discount_And_DiscountViewModel(int id)
        {
            var vm = new Discount()
            {
                Id = 1,
                ItemId = 1,
                CategoryId = 0,
                CustomerId = 0,
                CustomerLevelId = 0,
                DiscountType = Hrevert.Common.Enums.DiscountType.Fixed,
                DiscountValue = 10000,
                DiscountStartDate = DateTime.Now,
                DiscountEndDate = DateTime.Now.AddDays(15),
                MinimumQuantity = 10,
                WebActive = true,
                CompanyId = 1
            };
            var mappedDiscountVm = new HrevertCRM.Data.Mapper.DiscountToDiscountViewModelMapper().Map(vm);
            var discount = new HrevertCRM.Data.Mapper.DiscountToDiscountViewModelMapper().Map(mappedDiscountVm);

            //Test Account and mapped Account are same
            var res = true;

            PropertyInfo[] mappedproperties = discount.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(discount) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(discount) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(discount).Equals(propertyValuePair.GetValue(vm));
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
