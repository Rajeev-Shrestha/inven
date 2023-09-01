using System;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class AccountNodeAndAccountTreeViewModelMapper : MapperBase<AccountNode, AccountTreeViewModel>
    {
        public override AccountNode Map(AccountTreeViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public override AccountTreeViewModel Map(AccountNode accountNode)
        {
            return new AccountTreeViewModel
            {
                Id = accountNode.Source.Id,
                AccountCode = accountNode.Source.AccountCode,
                AccountDescription = accountNode.Source.AccountDescription,
                AccountType = accountNode.Source.AccountType,
                ParentAccountId = accountNode.Source.ParentAccountId,
                CurrentBalance = accountNode.Source.CurrentBalance,
                BankAccount = accountNode.Source.BankAccount,
                MainAccount = accountNode.Source.MainAccount,
                Level = accountNode.Source.Level,
                WebActive = accountNode.Source.WebActive,
                CompanyId = accountNode.Source.CompanyId,
                Version = accountNode.Source.Version,
                AccountCashFlowType = accountNode.Source.AccountCashFlowType,
                AccountDetailType = accountNode.Source.AccountDetailType,
                Active = accountNode.Source.Active
            };
        }

        internal object Map(Account s)
        {
            throw new NotImplementedException();
        }
    }
}
