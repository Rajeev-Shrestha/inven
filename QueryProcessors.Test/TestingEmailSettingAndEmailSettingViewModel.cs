using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingEmailSettingAndEmailSettingViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_EmailSetting_And_EmailSettingViewModel(int id)
        {
            var vm = new EmailSetting()
            {
                Id = 1,
                Host = "Xyz21",
                Port = 8080,
                Sender = "Test 1",
                UserName = "ABC",
                Password = "p@77w0rd",
                Name = "Test 2",
                RequireAuthentication = false,
                WebActive = true,
                CompanyId = 1
            };
            var mappedEmailSetingVm = new HrevertCRM.Data.Mapper.EmailSettingToEmailSettingViewModelMapper().Map(vm);
            var emailSetting = new HrevertCRM.Data.Mapper.EmailSettingToEmailSettingViewModelMapper().Map(mappedEmailSetingVm);

            //Test Email Setting and mapped Email Settings are same
            var res = true;

            PropertyInfo[] mappedproperties = emailSetting.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(emailSetting) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(emailSetting) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(emailSetting).Equals(propertyValuePair.GetValue(vm));
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
