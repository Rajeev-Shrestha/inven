using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class FiscalPeriodToFiscalPeriodViewModelMapper : MapperBase<FiscalPeriod, FiscalPeriodViewModel>
    {
        public override FiscalPeriod Map(FiscalPeriodViewModel viewModel)
        {
            return new FiscalPeriod
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                FiscalYearId = viewModel.FiscalYearId,
                Active = viewModel.Active
            
            };
        }

        public override FiscalPeriodViewModel Map(FiscalPeriod entity)
        {
            return new FiscalPeriodViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                FiscalYearId = entity.FiscalYearId,
                Active = entity.Active
            };
        }
    }
}
