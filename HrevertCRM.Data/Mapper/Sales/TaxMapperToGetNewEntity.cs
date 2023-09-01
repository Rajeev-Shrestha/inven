using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.Sales
{
    public class TaxMapperToGetNewEntity : MapperBase<Tax, TaxViewModel>
    {
        public override Tax Map(TaxViewModel viewModel)
        {
            return new Tax
            {
                Id = viewModel.Id ?? 0,
                TaxCode = viewModel.TaxCode,
                Description = viewModel.Description,
                IsRecoverable = viewModel.IsRecoverable,
                TaxType = viewModel.TaxType,
                TaxRate = viewModel.TaxRate,
                RecoverableCalculationType = viewModel.RecoverableCalculationType,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive
            };
        }

        public override TaxViewModel Map(Tax entity)
        {
            return new TaxViewModel
            {
                //Id = entity.Id,
                TaxCode = entity.TaxCode,
                Description = entity.Description,
                IsRecoverable = entity.IsRecoverable,
                TaxType = entity.TaxType,
                TaxRate = entity.TaxRate,
                RecoverableCalculationType = entity.RecoverableCalculationType,
                Active = entity.Active,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
