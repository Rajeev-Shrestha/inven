using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class EncryptionTypeToEncryptionTypeViewModelMapper : MapperBase<EncryptionTypes, EncryptionTypeViewModel>
    {
        public override EncryptionTypes Map(EncryptionTypeViewModel viewModel)
        {
            return new EncryptionTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override EncryptionTypeViewModel Map(EncryptionTypes entity)
        {
            return new EncryptionTypeViewModel
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
