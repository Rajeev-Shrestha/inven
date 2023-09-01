using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ItemMeasureToItemMeasureViewModelMapper : MapperBase<ItemMeasure, ItemMeasureViewModel>
    {
        public override ItemMeasure Map(ItemMeasureViewModel viewModel)
        {
            return new ItemMeasure
            {
                Id = viewModel.Id ?? 0,
                ProductId = viewModel.ProductId,
                MeasureUnitId = viewModel.MeasureUnitId,
                Price = viewModel.Price,

                WebActive = viewModel.WebActive,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version
            };
        }

        public override ItemMeasureViewModel Map(ItemMeasure entity)
        {
            var itemMeasureVm = new ItemMeasureViewModel
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                MeasureUnitId = entity.MeasureUnitId,
                Price = entity.Price,
                WebActive = entity.WebActive,
                Active = entity.Active,
                CompanyId = entity.CompanyId,
                Version = entity.Version
            };
            if (entity.Product == null) return itemMeasureVm;
            itemMeasureVm.ItemName = entity.Product.Name;
            return itemMeasureVm;
        }
    }
}
