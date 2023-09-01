using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class DiscountTypeToDiscountTypeViewModelMapper : MapperBase<DiscountTypes, DiscountTypeViewModel>
    {
        public override DiscountTypes Map(DiscountTypeViewModel viewModel)
        {
            return new DiscountTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override DiscountTypeViewModel Map(DiscountTypes entity)
        {
            return new DiscountTypeViewModel
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
