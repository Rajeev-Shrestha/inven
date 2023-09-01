using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class DeliveryMethodToDeliveryMethodViewModelMapper : MapperBase<DeliveryMethod, DeliveryMethodViewModel>
    {
        public override DeliveryMethod Map(DeliveryMethodViewModel viewModel)
        {
            return new DeliveryMethod
            {
                Id = viewModel.Id ?? 0,
                DeliveryCode = viewModel.DeliveryCode,
                Description = viewModel.Description,
                Active =  viewModel.Active,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                WebActive = viewModel.WebActive
            };
        }

        public override DeliveryMethodViewModel Map(DeliveryMethod entity)
        {
            return new DeliveryMethodViewModel
            {
                Id = entity.Id,
                DeliveryCode = entity.DeliveryCode,
                Description = entity.Description,
                Active =  entity.Active,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                WebActive = entity.WebActive
            };
        }
    }
}
