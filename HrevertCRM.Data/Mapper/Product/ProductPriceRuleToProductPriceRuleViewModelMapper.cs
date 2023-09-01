using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductPriceRuleToProductPriceRuleViewModelMapper : MapperBase<ProductPriceRule, ProductPriceRuleViewModel>
    {
        public override ProductPriceRule Map(ProductPriceRuleViewModel viewModel)
        {
            return new ProductPriceRule
            {
                Id = viewModel.Id ?? 0,
                CustomerId = viewModel.CustomerId,
                ProductId = viewModel.ProductId,
                CategoryId = viewModel.CategoryId,
                Quantity = viewModel.Quantity,
                FreeQuantity = viewModel.FreeQuantity,
                FixedPrice = viewModel.FixedPrice,
                DiscountPercent = viewModel.DiscountPercent,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version
            };
        }

        public override ProductPriceRuleViewModel Map(ProductPriceRule entity)
        {
            return new ProductPriceRuleViewModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                ProductId = entity.ProductId,
                CategoryId = entity.CategoryId,
                Quantity = entity.Quantity,
                FreeQuantity = entity.FreeQuantity,
                FixedPrice = entity.FixedPrice,
                DiscountPercent = entity.DiscountPercent,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version
            };
        }
    }
}
