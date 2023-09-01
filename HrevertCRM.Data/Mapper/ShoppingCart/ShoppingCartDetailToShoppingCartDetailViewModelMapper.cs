using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ShoppingCartDetailToShoppingCartDetailViewModelMapper : MapperBase<ShoppingCartDetail, ShoppingCartDetailViewModel>
    {
        public override ShoppingCartDetail Map(ShoppingCartDetailViewModel viewModel)
        {
            return new ShoppingCartDetail
            {
               
                Id = viewModel.Id ?? 0,
                Guid = viewModel.Guid,
                CustomerId = viewModel.CustomerId,
                ProductId = viewModel.ProductId,
                ProductCost = viewModel.ProductCost,
                ProductType = viewModel.ProductType,
                Discount = viewModel.Discount,
                Quantity = viewModel.Quantity,
                TaxAmount = viewModel.TaxAmount,
                ShoppingDateTime = viewModel.ShoppingDateTime,
                ShoppingCartId = viewModel.ShoppingCartId,
                CompanyId = viewModel.CompanyId,    
                Active = viewModel.Active,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive
            };
        }

        public override ShoppingCartDetailViewModel Map(ShoppingCartDetail entity)
        {
            return new ShoppingCartDetailViewModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                ProductId = entity.ProductId,
                ProductCost = entity.ProductCost,
                ProductType = entity.ProductType,
                Discount = entity.Discount,
                Quantity = entity.Quantity,
                TaxAmount = entity.TaxAmount,
                ShoppingDateTime = entity.ShoppingDateTime,
                ShoppingCartId = entity.ShoppingCartId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
