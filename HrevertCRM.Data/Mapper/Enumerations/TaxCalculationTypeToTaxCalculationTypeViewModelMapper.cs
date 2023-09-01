using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class TaxCalculationTypeToTaxCalculationTypeViewModelMapper : MapperBase<TaxCalculationTypes, TaxCalculationTypeViewModel>
    {
        public override TaxCalculationTypes Map(TaxCalculationTypeViewModel viewModel)
        {
            return new TaxCalculationTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override TaxCalculationTypeViewModel Map(TaxCalculationTypes entity)
        {
            return new TaxCalculationTypeViewModel
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
