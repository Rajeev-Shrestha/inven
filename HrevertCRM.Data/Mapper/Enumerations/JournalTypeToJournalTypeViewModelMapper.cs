using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class JournalTypeToJournalTypeViewModelMapper : MapperBase<JournalTypes, JournalTypeViewModel>
    {
        public override JournalTypes Map(JournalTypeViewModel viewModel)
        {
            return new JournalTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override JournalTypeViewModel Map(JournalTypes entity)
        {
            return new JournalTypeViewModel
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
