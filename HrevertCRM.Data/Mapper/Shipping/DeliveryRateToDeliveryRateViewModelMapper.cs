using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class DeliveryRateToDeliveryRateViewModelMapper : MapperBase<DeliveryRate, DeliveryRateViewModel>
    {
        public override DeliveryRate Map(DeliveryRateViewModel viewModel)
        {
            return new DeliveryRate
            {
                Id = viewModel.Id ?? 0,
                DeliveryMethodId = viewModel.DeliveryMethodId,
                DeliveryZoneId = viewModel.DeliveryZoneId,
                WeightFrom = viewModel.WeightFrom,
                WeightTo = viewModel.WeightTo,
                ProductCategoryId = viewModel.ProductCategoryId,
                ProductId = viewModel.ProductId,
                MinimumRate = viewModel.MinimumRate,
                Rate = viewModel.Rate,
                DocTotalFrom = viewModel.DocTotalFrom,
                DocTotalTo = viewModel.DocTotalTo,
                UnitFrom = viewModel.UnitFrom,
                UnitTo = viewModel.UnitTo,
                MeasureUnitId = viewModel.MeasureUnitId,

                CompanyId = viewModel.CompanyId,
                WebActive = viewModel.WebActive,
                Active = viewModel.Active,
                Version = viewModel.Version,
            };
        }

        public override DeliveryRateViewModel Map(DeliveryRate entity)
        {
            var deliveryRate= new DeliveryRateViewModel
            {
                Id = entity.Id,
                DeliveryMethodId = entity.DeliveryMethodId,
                DeliveryZoneId = entity.DeliveryZoneId,
                WeightFrom = entity.WeightFrom,
                WeightTo = entity.WeightTo,
                ProductCategoryId = entity.ProductCategoryId,
                ProductId = entity.ProductId,
                MinimumRate = entity.MinimumRate,
                Rate = entity.Rate,
                DocTotalFrom = entity.DocTotalFrom,
                DocTotalTo = entity.DocTotalTo,
                UnitFrom = entity.UnitFrom,
                UnitTo = entity.UnitTo,
                MeasureUnitId = entity.MeasureUnitId,

                CompanyId = entity.CompanyId,
                WebActive = entity.WebActive,
                Active = entity.Active,
                Version = entity.Version,
            };
            if (entity.Product == null) return deliveryRate;
            deliveryRate.ProductCode = entity.Product.Code;
            deliveryRate.ProductName = entity.Product.Name;
            return deliveryRate;
        }
    }
}

