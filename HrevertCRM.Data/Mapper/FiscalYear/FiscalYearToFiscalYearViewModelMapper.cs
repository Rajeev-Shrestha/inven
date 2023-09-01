using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class FiscalYearToFiscalYearViewModelMapper : MapperBase<FiscalYear, FiscalYearViewModel>
    {
        public override FiscalYear Map(FiscalYearViewModel fiscalYearViewModel)
        {
            return new FiscalYear
            {
                Id = fiscalYearViewModel.Id ?? 0,
                Name = fiscalYearViewModel.Name,
                StartDate = fiscalYearViewModel.StartDate,
                EndDate = fiscalYearViewModel.EndDate,
                CompanyId = fiscalYearViewModel.CompanyId,
                Active = fiscalYearViewModel.Active,
                Version = fiscalYearViewModel.Version,
                WebActive = fiscalYearViewModel.WebActive
            };
        }

        public override FiscalYearViewModel Map(FiscalYear entity)
        {
            return new FiscalYearViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                Active = entity.Active,
                WebActive = entity.WebActive
            };
        }
    }
}
