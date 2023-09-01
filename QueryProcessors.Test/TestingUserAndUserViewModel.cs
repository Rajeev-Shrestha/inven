using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingUserAndUserViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_User_And_UserViewModel(int id)
        {
            var vm = new ApplicationUser()
            {
                FirstName="Test 1",
                MiddleName = "Test 2",
                LastName = "Test 3",
                Gender = 1,
                Address = "ABC",
                Phone = "12344",
                CompanyId = 1,
                Active = true,
                WebActive = true,
                DateModified = DateTime.Parse("2016/11/11"),
                DateCreated = DateTime.Parse("2016/11/11"),
                CreatedBy = "ABC",



            };
            var mappedUserVm = new HrevertCRM.Data.Mapper.UserToUserViewModelMapper().Map(vm);
            var user = new HrevertCRM.Data.Mapper.UserToUserViewModelMapper().Map(mappedUserVm);

            //Test ApplicationUser and mappedApplicationUser are same
            var res = true;

            PropertyInfo[] mappedproperties = user.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(user) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(user) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(user).Equals(propertyValuePair.GetValue(vm));
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
