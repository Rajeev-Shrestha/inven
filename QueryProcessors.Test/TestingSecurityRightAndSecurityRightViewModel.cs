using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingSecurityRightAndSecurityRightViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_SecurityRight_And_SecurityViewModel(int id)
        {
            var vm = new SecurityRight()
            {
                Id = 1,
                Allowed = true,
                UserId = "Test 1",
                SecurityId = 1,
                SecurityGroupId = 1,
                WebActive = true,
                CompanyId = 1
            };
            var mappedSecurityRightVm = new HrevertCRM.Data.Mapper.SecurityRightToSecurityRightViewModelMapper().Map(vm);
            var securityRight = new HrevertCRM.Data.Mapper.SecurityRightToSecurityRightViewModelMapper().Map(mappedSecurityRightVm);

            //Test SecurityRight and mapped SecurityRight are same
            var res = true;

            PropertyInfo[] mappedproperties = securityRight.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(securityRight) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(securityRight) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(securityRight).Equals(propertyValuePair.GetValue(vm));
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
