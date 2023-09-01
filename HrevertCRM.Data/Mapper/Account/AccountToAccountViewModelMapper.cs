using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class AccountToAccountViewModelMapper : MapperBase<Account, AccountViewModel>
    {
        public override Account Map(AccountViewModel viewModel)
        {
            return new Account()
            {
                Id = viewModel.Id ?? 0,
                AccountCode = viewModel.AccountCode,
                AccountDescription = viewModel.AccountDescription,
                AccountType = viewModel.AccountType,
                ParentAccountId = viewModel.ParentAccountId,
                CurrentBalance = viewModel.CurrentBalance,
                BankAccount = viewModel.BankAccount,
                MainAccount = viewModel.MainAccount,
                Level = viewModel.Level,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                AccountCashFlowType = viewModel.AccountCashFlowType,
                AccountDetailType = viewModel.AccountDetailType
            };
        }

        public override AccountViewModel Map(Account entity)
        {
            return new AccountViewModel()
            {
                Id = entity.Id,
                AccountCode = entity.AccountCode,
                AccountDescription = entity.AccountDescription,
                AccountType = entity.AccountType,
                ParentAccountId = entity.ParentAccountId,
                CurrentBalance = entity.CurrentBalance,
                BankAccount = entity.BankAccount,
                MainAccount = entity.MainAccount,
                Level = entity.Level,
                Active = entity.Active,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                AccountCashFlowType = entity.AccountCashFlowType,
                AccountDetailType = entity.AccountDetailType
            };
        }
    }
}
