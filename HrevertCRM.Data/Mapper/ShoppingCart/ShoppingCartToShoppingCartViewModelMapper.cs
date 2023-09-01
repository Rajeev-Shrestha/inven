using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ShoppingCartToShoppingCartViewModelMapper : MapperBase<ShoppingCart, ShoppingCartViewModel>
    {
        public override ShoppingCart Map(ShoppingCartViewModel viewModel)
        {
            var mapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();

            var sc = new ShoppingCart
            {
                Id = viewModel.Id ?? 0,
                CustomerId = viewModel.CustomerId,
                HostIp = viewModel.HostIp,
                IsCheckedOut = viewModel.IsCheckedOut,
                Amount = viewModel.Amount,
                BillingAddressId = viewModel.BillingAddressId,
                ShippingAddressId = viewModel.ShippingAddressId,
                PaymentTermId = viewModel.PaymentTermId,
                DeliveryMethodId = viewModel.DeliveryMethodId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId
            };

            if (viewModel.ShoppingCartDetails == null) return sc;
            var shoppingCartDetailList = viewModel.ShoppingCartDetails.Select(x => mapper.Map(x)).ToList();
            sc.ShoppingCartDetails = shoppingCartDetailList;
            return sc;
        }
        
        public override ShoppingCartViewModel Map(ShoppingCart entity)
        {
            var mapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();

            var sc = new ShoppingCartViewModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId ?? 0,
                HostIp=entity.HostIp,
                IsCheckedOut = entity.IsCheckedOut,
                Amount =entity.Amount,
                BillingAddressId = entity.BillingAddressId,
                ShippingAddressId = entity.ShippingAddressId,
                PaymentTermId = entity.PaymentTermId ?? 0,
                DeliveryMethodId = entity.DeliveryMethodId ?? 0,
                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId
            };

            if (entity.ShoppingCartDetails == null) return sc;
            var shoppingCartDetailList = entity.ShoppingCartDetails.Select(x => mapper.Map(x)).ToList();
            sc.ShoppingCartDetails = shoppingCartDetailList;
            return sc;
        }

    }

}
