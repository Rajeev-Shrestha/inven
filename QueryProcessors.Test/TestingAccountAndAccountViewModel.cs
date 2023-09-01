using System.Reflection;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingAccountAndAccountViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Account_And_AccountViewModel(int id)
        {
            var vm = new Account()
            {
                Id = 1,
                AccountCode = "Xyz21",
                AccountDescription = "Test",
                ParentAccountId = 1,
                CurrentBalance = 201,
                BankAccount = false,
                MainAccount = false,
                WebActive = true,
                CompanyId = 1,
            };
            var mappedAccountVm = new HrevertCRM.Data.Mapper.AccountToAccountViewModelMapper().Map(vm);
            var account = new HrevertCRM.Data.Mapper.AccountToAccountViewModelMapper().Map(mappedAccountVm);

            //Test Account and mapped Account are same
            var res = true;

            PropertyInfo[] mappedproperties = account.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(account) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(account) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(account).Equals(propertyValuePair.GetValue(vm));
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
