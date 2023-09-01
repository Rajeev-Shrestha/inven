using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class PurchaseOrderStatusToPurchaseOrderStatusViewModelMapper : MapperBase<PurchaseOrdersStatus, PurchaseOrderStatusViewModel>
    {
        public override PurchaseOrdersStatus Map(PurchaseOrderStatusViewModel viewModel)
        {
            return new PurchaseOrdersStatus
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override PurchaseOrderStatusViewModel Map(PurchaseOrdersStatus entity)
        {
            return new PurchaseOrderStatusViewModel
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
