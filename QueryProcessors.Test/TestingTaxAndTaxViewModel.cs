using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingTaxAndTaxViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Tax_And_TaxViewModel(int id)
        {
            var vm = new Tax()
            {
                Id = 1,
                TaxCode = "C001",
                Description = "A Text ",
                IsRecoverable = true,
                TaxRate = 121,
                TaxType = TaxCaculationType.Fixed,
                RecoverableCalculationType = 1,
                WebActive = true,
                CompanyId = 1

            };
            var mappedTaxVm = new HrevertCRM.Data.Mapper.TaxToTaxViewModelMapper().Map(vm);
            var tax = new HrevertCRM.Data.Mapper.TaxToTaxViewModelMapper().Map(mappedTaxVm);

            //Test Tax and mappedTax are same
            var res = true;

            PropertyInfo[] mappedproperties = tax.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(tax) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(tax) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(tax).Equals(propertyValuePair.GetValue(vm));
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
