using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class DescriptionTypeToDescriptionTypeViewModelMapper : MapperBase<DescriptionTypes, DescriptionTypeViewModel>
    {
        public override DescriptionTypes Map(DescriptionTypeViewModel viewModel)
        {
            return new DescriptionTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override DescriptionTypeViewModel Map(DescriptionTypes entity)
        {
            return new DescriptionTypeViewModel
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
