using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class AccountDetailTypeToAccountDetailTypeViewModelMapper : MapperBase<AccountDetailTypes, AccountDetailTypeViewModel>
    {
        public override AccountDetailTypes Map(AccountDetailTypeViewModel viewModel)
        {
            return new AccountDetailTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override AccountDetailTypeViewModel Map(AccountDetailTypes entity)
        {
            return new AccountDetailTypeViewModel
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
