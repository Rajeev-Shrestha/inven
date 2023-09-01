using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class BugLoggerToBugLoggerViewModelMapper : MapperBase<BugLogger, BugLoggerViewModel>
    {
        public override BugLogger Map(BugLoggerViewModel viewModel)
        {
            return new BugLogger
            {
                Id = viewModel.Id ?? 0,
                Message = viewModel.Message,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version
            };
        }

        public override BugLoggerViewModel Map(BugLogger entity)
        {
            return new BugLoggerViewModel
            {
                Id = entity.Id,
                Message = entity.Message,
                Active = entity.Active,
                CompanyId = entity.CompanyId,
                Version = entity.Version
            };
        }
    }
}
