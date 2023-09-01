using System;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class AccountCashFlowTypeToAccountCashFlowTypeViewModelMapper : MapperBase<AccountCashFlowTypes, AccountCashFlowTypeViewModel>
    {
        public override AccountCashFlowTypes Map(AccountCashFlowTypeViewModel viewModel)
        {
            return new AccountCashFlowTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override AccountCashFlowTypeViewModel Map(AccountCashFlowTypes entity)
        {
            return new AccountCashFlowTypeViewModel
            {
                Id = entity.Id,
                Value = entity.Value,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
