using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingUserAndUserSummaryViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_User_And_UserSummaryViewModel(int id)
        {
            var vm = new ApplicationUser()
            {
                Id ="123",
                FirstName = "Test 1",
                MiddleName = "Test 2",
                LastName = "Test 3",
                Active = true,
                CompanyId = 1


            };
            var mappedUserSummaryVm = new HrevertCRM.Data.Mapper.UserToUserSummaryViewModelMapper().Map(vm);
            var userSummary = new HrevertCRM.Data.Mapper.UserToUserSummaryViewModelMapper().Map(mappedUserSummaryVm);

            //Test User Summary and mapped User Summary are same
            var res = true;

            PropertyInfo[] mappedproperties = userSummary.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(userSummary) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(userSummary) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(userSummary).Equals(propertyValuePair.GetValue(vm));
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
