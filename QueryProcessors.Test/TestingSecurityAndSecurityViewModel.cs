using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingSecurityAndSecurityViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Security_And_SecurityViewModel(int id)
        {
            var vm = new Security()
            {
                Id = 1,
                SecurityCode = 121,
                SecurityDescription = "Test 1",
                WebActive = true,
                CompanyId = 1
            };
            var mappedSecurityVm = new HrevertCRM.Data.Mapper.SecurityToSecurityViewModelMapper().Map(vm);
            var security = new HrevertCRM.Data.Mapper.SecurityToSecurityViewModelMapper().Map(mappedSecurityVm);

            //Test Security and mapped Security are same
            var res = true;

            PropertyInfo[] mappedproperties = security.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(security) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(security) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(security).Equals(propertyValuePair.GetValue(vm));
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
