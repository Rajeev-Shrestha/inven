using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class DeliveryZoneToDeliveryZoneViewModelMapper : MapperBase<DeliveryZone, DeliveryZoneViewModel>
    {
        public override DeliveryZone Map(DeliveryZoneViewModel viewModel)
        {
            var addressMapper = new AddressToAddressViewModelMapper();
            var deliveryRateMapper = new DeliveryRateToDeliveryRateViewModelMapper();

            var deliveryZone = new DeliveryZone
            {
                Id = viewModel.Id ?? 0,
                ZoneName = viewModel.ZoneName,
                ZoneCode = viewModel.ZoneCode,

                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
            if (viewModel.Addresses != null)
            {
                var addresses = viewModel.Addresses.Select(x => addressMapper.Map(x));
                deliveryZone.Addresses = addresses.ToList();
            }
            if (viewModel.DeliveryRates == null) return deliveryZone;
            var deliveryRates = viewModel.DeliveryRates.Select(x => deliveryRateMapper.Map(x));
            deliveryZone.DeliveryRates = deliveryRates.ToList();
            return deliveryZone;
        }

        public override DeliveryZoneViewModel Map(DeliveryZone entity)
        {
            var addressMapper = new AddressToAddressViewModelMapper();
            var deliveryRateMapper = new DeliveryRateToDeliveryRateViewModelMapper();

            var deliveryZoneViewmodel =  new DeliveryZoneViewModel
            {
                Id = entity.Id,
                ZoneName = entity.ZoneName,
                ZoneCode = entity.ZoneCode,

                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.Addresses != null)
            {
                var addresses = entity.Addresses.Select(x => addressMapper.Map(x));
                deliveryZoneViewmodel.Addresses = addresses.ToList();
            }
            if (entity.DeliveryRates == null) return deliveryZoneViewmodel;
            var deliveryRates = entity.DeliveryRates.Select(x => deliveryRateMapper.Map(x));
            deliveryZoneViewmodel.DeliveryRates = deliveryRates.ToList();
            return deliveryZoneViewmodel;
        }
    }
}
