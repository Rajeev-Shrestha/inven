using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingCompanyAndCompanyViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Comapny_And_CompanyViewModel(int id)
        {
            var vm = new Company()
            {
                Id = 1,
                Name = "Test 1",
                WebActive = true,
                CompanyId = 1
            };
            var mappedCompanyVm = new HrevertCRM.Data.Mapper.CompanyToCompanyViewModelMapper().Map(vm);
            var company = new HrevertCRM.Data.Mapper.CompanyToCompanyViewModelMapper().Map(mappedCompanyVm);

            //Test Company and mapped Company are same
            var res = true;

            PropertyInfo[] mappedproperties = company.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(company) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(company) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(company).Equals(propertyValuePair.GetValue(vm));
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
