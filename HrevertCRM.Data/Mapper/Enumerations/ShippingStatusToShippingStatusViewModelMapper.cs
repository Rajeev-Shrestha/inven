using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class ShippingStatusToShippingStatusViewModelMapper: MapperBase<ShippingStatus,ShippingStatusViewModel>
    {
        public override ShippingStatus Map(ShippingStatusViewModel viewModel)
        {
            return new ShippingStatus
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override ShippingStatusViewModel Map(ShippingStatus entity)
        {
            return new ShippingStatusViewModel
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
