using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class JournalMasterToJournalMasterViewModelMapper : MapperBase<JournalMaster, JournalMasterViewModel>
    {
        public override JournalMaster Map(JournalMasterViewModel viewModel)
        {
            return new JournalMaster
            {
                Id = viewModel.Id ?? 0,
                JournalType = viewModel.JournalType,
                Closed = viewModel.Closed ?? false,
                Description = viewModel.Description,
                Credit = viewModel.Credit,
                Debit = viewModel.Debit,
                FiscalPeriodId = viewModel.FiscalPeriodId,
                Posted = viewModel.Posted ?? false,
                Note = viewModel.Note,
                PostedDate = viewModel.PostedDate,
                Printed = viewModel.Printed ?? false,
                Cancelled = viewModel.Cancelled ?? false,
                IsSystem = viewModel.IsSystem,
                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version,
                Active = viewModel.Active
                
            };
        }

        public override JournalMasterViewModel Map(JournalMaster entity)
        {
            return new JournalMasterViewModel
            {
                Id = entity.Id,
                JournalType = entity.JournalType,
                Closed = entity.Closed,
                Description = entity.Description,
                Credit = entity.Credit,
                Debit = entity.Debit,
                FiscalPeriodId = entity.FiscalPeriodId,
                Posted = entity.Posted,
                Note = entity.Note,
                PostedDate = entity.PostedDate,
                Printed = entity.Printed,
                Cancelled = entity.Cancelled,
                IsSystem = entity.IsSystem,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                Active = entity.Active
            };
        }
    }
}
