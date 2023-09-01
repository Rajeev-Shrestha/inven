using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class PurchaseOrderLineToPurchaseOrderLineViewModelMapper : MapperBase<PurchaseOrderLine, PurchaseOrderLineViewModel>
    {
        public override PurchaseOrderLine Map(PurchaseOrderLineViewModel viewModel)
        {
            return new PurchaseOrderLine
            {
                Id = viewModel.Id ?? 0,
                Description = viewModel.Description,
                DescriptionType = viewModel.DescriptionType,
                ItemPrice = viewModel.ItemPrice,
                Shipped = viewModel.Shipped,
                ItemQuantity = viewModel.ItemQuantity,
                ShippedQuantity = viewModel.ShippedQuantity,
                LineOrder = viewModel.LineOrder,
                Discount = viewModel.Discount,
                TaxAmount = viewModel.TaxAmount,
                DiscountType = viewModel.DiscountType,
                TaxId = viewModel.TaxId,
                PurchaseOrderId = viewModel.PurchaseOrderId,
                ProductId = viewModel.ProductId,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive
            };
        }

        public override PurchaseOrderLineViewModel Map(PurchaseOrderLine entity)
        {
            return new PurchaseOrderLineViewModel
            {
                Id = entity.Id,
                Description = entity.Description,
                DescriptionType = entity.DescriptionType,
                ItemPrice = entity.ItemPrice,
                Shipped = entity.Shipped,
                ItemQuantity = entity.ItemQuantity,
                ShippedQuantity = entity.ShippedQuantity,
                LineOrder = entity.LineOrder,
                Discount = entity.Discount,
                TaxAmount = entity.TaxAmount,
                DiscountType = entity.DiscountType,
                TaxId = entity.TaxId,
                PurchaseOrderId = entity.PurchaseOrderId,
                ProductId = entity.ProductId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
