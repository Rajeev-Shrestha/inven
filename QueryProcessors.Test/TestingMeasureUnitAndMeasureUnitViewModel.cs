using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ViewModels.Test
{
    public class TestingMeasureUnitAndMeasureUnitViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_MeasureUnit_And_MeasureUnitViewModel(int id)
        {
            var vm = new MeasureUnit()
            {
                Id = 1,
                Measure = "Kilo",
                MeasureCode = "1",
                EntryMethod = Hrevert.Common.Enums.EntryMethod.Decimal,
                WebActive = true,
                CompanyId = 1
            };
            var mappedMeasureUnitVm = new HrevertCRM.Data.Mapper.MeasureUnitToMeasureUnitViewModelMapper().Map(vm);
            var measureUnit = new HrevertCRM.Data.Mapper.MeasureUnitToMeasureUnitViewModelMapper().Map(mappedMeasureUnitVm);

            //Test Account and mapped Account are same
            var res = true;

            PropertyInfo[] mappedproperties = measureUnit.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(measureUnit) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(measureUnit) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(measureUnit).Equals(propertyValuePair.GetValue(vm));
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
