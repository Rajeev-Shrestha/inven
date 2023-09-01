using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class PurchaseOrderTypeToPurchaseOrderTypeViewModelMapper : MapperBase<PurchaseOrderTypes, PurchaseOrderTypeViewModel>
    {
        public override PurchaseOrderTypes Map(PurchaseOrderTypeViewModel viewModel)
        {
            return new PurchaseOrderTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override PurchaseOrderTypeViewModel Map(PurchaseOrderTypes entity)
        {
            return new PurchaseOrderTypeViewModel
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
