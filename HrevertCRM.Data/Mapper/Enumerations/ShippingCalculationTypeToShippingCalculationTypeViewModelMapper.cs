using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class ShippingCalculationTypeToShippingCalculationTypeViewModelMapper:MapperBase<ShippingCalculationTypes,ShippingCalculationTypeViewModel>
    {
        public override ShippingCalculationTypes Map(ShippingCalculationTypeViewModel viewModel)
        {
            return new ShippingCalculationTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override ShippingCalculationTypeViewModel Map(ShippingCalculationTypes entity)
        {
            return new ShippingCalculationTypeViewModel
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
