using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class AddressTypeToAddressTypeViewModelMapper : MapperBase<AddressTypes, AddressTypeViewModel>
    {
        public override AddressTypes Map(AddressTypeViewModel viewModel)
        {
            return new AddressTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override AddressTypeViewModel Map(AddressTypes entity)
        {
            return new AddressTypeViewModel
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
