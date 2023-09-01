using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingSecurityAndSecurityGroupMemberViewModel
    {

        [Theory]
        [InlineData(1)]
        public void Testing_Security_And_SecurityGroupMemberViewModel(int id)
        {
            var vm = new SecurityGroupMember()
            {
                Id = 1,
                MemberId="C0001",
                SecurityGroupId =12, 
                WebActive = true,
                CompanyId = 1
            };
            var mappedSecurityGroupMemberVm = new HrevertCRM.Data.Mapper.SecurityGroupMemberToSecurityGroupMemberViewModelMapper().Map(vm);
            var securityGroupMember = new HrevertCRM.Data.Mapper.SecurityGroupMemberToSecurityGroupMemberViewModelMapper().Map(mappedSecurityGroupMemberVm);

            //Test SecurityGroupMember and mappedSecurityGroupMember are same
            var res = true;

            PropertyInfo[] mappedproperties = securityGroupMember.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(securityGroupMember) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(securityGroupMember) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(securityGroupMember).Equals(propertyValuePair.GetValue(vm));
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
