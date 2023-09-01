using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class AccountLevelTypeToAccountLevelTypeViewModelMapper : MapperBase<AccountLevelTypes, AccountLevelTypeViewModel>
    {
        public override AccountLevelTypes Map(AccountLevelTypeViewModel viewModel)
        {
            return new AccountLevelTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override AccountLevelTypeViewModel Map(AccountLevelTypes entity)
        {
            return new AccountLevelTypeViewModel
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
