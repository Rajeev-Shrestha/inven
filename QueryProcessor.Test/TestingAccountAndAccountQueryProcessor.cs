using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using Xunit;

namespace QueryProcessor.Test
{
    
    public class TestingAccountAndAccountQueryProcessor
    {
        private readonly IAccountQueryProcessor _accountQueryProcessor;
        public TestingAccountAndAccountQueryProcessor(IAccountQueryProcessor accountQueryProcessor)
        {
            _accountQueryProcessor = accountQueryProcessor;
        }

        [Theory]
        [InlineData(1)]
        public void Testing_Account_And_AccountQueryProcessor(int id)
        {
            var vm = new Account
            {
                Id = 1,
                AccountCode = "ACC-0001",
                AccountDescription = "Account Test",
                AccountType = Hrevert.Common.Enums.AccountType.Asset,
                AccountCashFlowType = Hrevert.Common.Enums.AccountCashFlowType.None,
                AccountDetailType = Hrevert.Common.Enums.AccountDetailType.AccountPayable,
                Level = Hrevert.Common.Enums.AccountLevel.Detail,
                ParentAccountId = 1,
                CurrentBalance = 201,
                BankAccount = false,
                MainAccount = false,
                WebActive = true
            };
          
          
            var mappedAccountVm = _accountQueryProcessor.Save(vm);

            Assert.True(mappedAccountVm.Id>0);


        }

        
    }
}
