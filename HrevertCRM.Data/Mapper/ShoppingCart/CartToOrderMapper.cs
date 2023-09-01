using System;
using System.Linq;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.Company
{
    public class CartToOrderMapper : MapperBase<ShoppingCart, SalesOrder>
    {
        public override ShoppingCart Map(SalesOrder viewModel)
        {
            throw new Exception("No Implemented");
            //return new ShoppingCartViewModel();
        }

        public override SalesOrder Map(ShoppingCart shoppingCart)
        {
            var order = new SalesOrder
            {
                TotalAmount = shoppingCart.Amount,
                CustomerId = shoppingCart.CustomerId ?? 0,
                BillingAddressId = shoppingCart.BillingAddressId ?? 0,
                ShippingAddressId = shoppingCart.ShippingAddressId ?? 0,
                PaymentTermId = shoppingCart.PaymentTermId ?? 0,
                DeliveryMethodId = shoppingCart.DeliveryMethodId ?? 0,
                Active = shoppingCart.Active,
                WebActive = shoppingCart.WebActive,
                Version = shoppingCart.Version
            };

            if (shoppingCart.ShoppingCartDetails == null || shoppingCart.ShoppingCartDetails.Count <= 0) return order;
            var orderlines = shoppingCart.ShoppingCartDetails
                .Select(s => new SalesOrderLine
                {
                    Description = s.Product.ShortDescription,
                    Discount = s.Discount,
                    ItemPrice = s.ProductCost,
                    ItemQuantity = s.Quantity,
                    LineOrder = 1, //TODO: Add line Order in cart details
                    ProductId = s.ProductId,
                    ShippedQuantity = 0,
                    Shipped = false,
                    TaxAmount = s.TaxAmount,
                    CompanyId = s.CompanyId,
                    Active = true,
                }
            );
            order.SalesOrderLines = orderlines.ToList();
            return order;
        }
    }
}
