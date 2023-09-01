using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingFiscalPeriodAndFiscalPeriodViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_FiscalPeriod_And_FiscalPeriodViewModel(int id)
        {
            var vm = new FiscalPeriod()
            {
                Id = 1,
                FiscalYearId = 1,
                Name = "Xyz21",
                StartDate = Convert.ToDateTime("2015/11/11"),
                EndDate = Convert.ToDateTime("2016/11/11"),
                CompanyId = 1
            };
            var mappedFiscalPeriodvm = new HrevertCRM.Data.Mapper.FiscalPeriodToFiscalPeriodViewModelMapper().Map(vm);
            var fiscalPeriod = new HrevertCRM.Data.Mapper.FiscalPeriodToFiscalPeriodViewModelMapper().Map(mappedFiscalPeriodvm);

            //Test Fiscal Period and mapped Fiscal Period are same
            var res = true;

            PropertyInfo[] mappedproperties = fiscalPeriod.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(fiscalPeriod) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(fiscalPeriod) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(fiscalPeriod).Equals(propertyValuePair.GetValue(vm));
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
