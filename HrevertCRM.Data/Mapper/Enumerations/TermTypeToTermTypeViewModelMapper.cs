using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class TermTypeToTermTypeViewModelMapper : MapperBase<TermTypes, TermTypeViewModel>
    {
        public override TermTypes Map(TermTypeViewModel viewModel)
        {
            return new TermTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override TermTypeViewModel Map(TermTypes entity)
        {
            return new TermTypeViewModel
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
