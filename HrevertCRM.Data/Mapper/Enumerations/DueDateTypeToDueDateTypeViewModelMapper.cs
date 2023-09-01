using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class DueDateTypeToDueDateTypeViewModelMapper : MapperBase<DueDateTypes, DueDateTypeViewModel>
    {
        public override DueDateTypes Map(DueDateTypeViewModel viewModel)
        {
            return new DueDateTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override DueDateTypeViewModel Map(DueDateTypes entity)
        {
            return new DueDateTypeViewModel
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
