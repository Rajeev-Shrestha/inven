using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class MeasureUnitToMeasureUnitViewModelMapper : MapperBase<MeasureUnit, MeasureUnitViewModel>
    {
        public override MeasureUnit Map(MeasureUnitViewModel viewModel)
        {
            var itemMeasureMapper = new ItemMeasureToItemMeasureViewModelMapper();
            var deliveryRateMapper = new DeliveryRateToDeliveryRateViewModelMapper();
            var measureUnit = new MeasureUnit
            {
                Id = viewModel.Id ?? 0,
                Measure = viewModel.Measure,
                MeasureCode = viewModel.MeasureCode,
                EntryMethod = viewModel.EntryMethod,
                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
            if (viewModel.ItemMeasures != null)
            {
                var itemMeasures = viewModel.ItemMeasures.Select(x => itemMeasureMapper.Map(x));
                measureUnit.ItemMeasures = itemMeasures.ToList();
            }
            if (viewModel.DeliveryRates == null) return measureUnit;
            var deliveryRates = viewModel.DeliveryRates.Select(x => deliveryRateMapper.Map(x));
            measureUnit.DeliveryRates = deliveryRates.ToList();
            return measureUnit;
        }

        public override MeasureUnitViewModel Map(MeasureUnit entity)
        {
            var itemMeasureMapper = new ItemMeasureToItemMeasureViewModelMapper();
            var deliveryRateMapper = new DeliveryRateToDeliveryRateViewModelMapper();

            var measureUnitViewModel = new MeasureUnitViewModel
            {
                Id = entity.Id,
                Measure = entity.Measure,
                MeasureCode = entity.MeasureCode,
                EntryMethod = entity.EntryMethod,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.ItemMeasures != null)
            {
                var itemMeasures = entity.ItemMeasures.Select(x => itemMeasureMapper.Map(x));
                measureUnitViewModel.ItemMeasures = itemMeasures.ToList();
            }
            if (entity.DeliveryRates == null) return measureUnitViewModel;
            var deliveryRates = entity.DeliveryRates.Select(x => deliveryRateMapper.Map(x));
            measureUnitViewModel.DeliveryRates = deliveryRates.ToList();
            return measureUnitViewModel;
        }
    }
}
