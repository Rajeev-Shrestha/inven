
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class DiscountCalculationTypeToDiscountCalculationTypeViewModelMapper : MapperBase<DiscountCalculationTypes, DiscountCalculationTypeViewModel>
    {
        public override DiscountCalculationTypes Map(DiscountCalculationTypeViewModel viewModel)
        {
            return new DiscountCalculationTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override DiscountCalculationTypeViewModel Map(DiscountCalculationTypes entity)
        {
            return new DiscountCalculationTypeViewModel
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
