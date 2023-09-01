using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class EcommerceSettingToEcommerceSettingViewModelMapper : MapperBase<EcommerceSetting, EcommerceSettingViewModel>
    {
        public override EcommerceSetting Map(EcommerceSettingViewModel viewModel)
        {
            return new EcommerceSetting
            {
                Id = viewModel.Id ?? 0,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                IncludeQuantityInSalesOrder = viewModel.IncludeQuantityInSalesOrder,
                DisplayOutOfStockItems = viewModel.DisplayOutOfStockItems,
                ProductPerCategory = viewModel.ProductPerCategory,
                DisplayQuantity = viewModel.DisplayQuantity,
                DecreaseQuantityOnOrder = viewModel.DecreaseQuantityOnOrder,
                ImageUrl = viewModel.ImageUrl,
                DueDatePeriod = viewModel.DueDatePeriod ?? 0
            };
        }

        public override EcommerceSettingViewModel Map(EcommerceSetting entity)
        {
            return new EcommerceSettingViewModel
            {
                Id = entity.Id,
                Active = entity.Active,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                WebActive = entity.WebActive,
                IncludeQuantityInSalesOrder = entity.IncludeQuantityInSalesOrder,
                DisplayOutOfStockItems = entity.DisplayOutOfStockItems,
                ProductPerCategory = entity.ProductPerCategory,
                DisplayQuantity = entity.DisplayQuantity,
                DecreaseQuantityOnOrder = entity.DecreaseQuantityOnOrder,
                ImageUrl = entity.ImageUrl,
                DueDatePeriod = entity.DueDatePeriod
            };
        }
    }
}
