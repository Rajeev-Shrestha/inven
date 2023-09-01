using System.Linq;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class ProductToProductViewModelMapper : MapperBase<Entities.Product, ProductViewModel>
    {
        public override Entities.Product Map(ProductViewModel viewModel)
        {
            return new Entities.Product {
                Id = viewModel.Id ?? 0,
                Code = viewModel.Code,
                Name = viewModel.Name,
                QuantityOnHand = viewModel.QuantityOnHand,
                QuantityOnOrder = viewModel.QuantityOnOrder,
                UnitPrice = viewModel.UnitPrice,
                ShortDescription = viewModel.ShortDescription,
                LongDescription = viewModel.LongDescription,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                Commissionable = viewModel.Commissionable,
                CommissionRate = viewModel.CommissionRate,
                Active = viewModel.Active,
                ProductType = viewModel.ProductType,
                CompanyId = viewModel.CompanyId,
                AllowBackOrder = viewModel.AllowBackOrder
            };
        }

        public override ProductViewModel Map(Entities.Product entity)
        {
            if (entity == null) return null; 
            var product = new ProductViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                QuantityOnHand = entity.QuantityOnHand,
                QuantityOnOrder = entity.QuantityOnOrder,
                UnitPrice = entity.UnitPrice,
                ShortDescription = entity.ShortDescription,
                LongDescription = entity.LongDescription,
                Version = entity.Version,
                WebActive = entity.WebActive,
                Commissionable = entity.Commissionable,
                CommissionRate = entity.CommissionRate,
                Active = entity.Active,
                ProductType = entity.ProductType,
                CompanyId = entity.CompanyId,
                AllowBackOrder = entity.AllowBackOrder
            };
            if (entity.ProductInCategories != null && entity.ProductInCategories.Count > 0)
                product.Categories = entity.ProductInCategories.Select(s => s.CategoryId).ToList();

            if (entity.TaxesInProducts != null && entity.TaxesInProducts.Count > 0)
                product.Taxes = entity.TaxesInProducts.Select(s => s.TaxId).ToList();

            if (entity.ProductsReferencedByKitAndAssembledTypes != null && entity.ProductsReferencedByKitAndAssembledTypes.Count > 0)
                product.ProductsReferencedByKitAndAssembledType = entity.ProductsReferencedByKitAndAssembledTypes.Select(x => x.ProductRefId).ToList();

            if (entity.ProductMetadatas == null || entity.ProductMetadatas.Count <= 0) return product;
            product.SmallImageUrls = entity.ProductMetadatas.Where(x => x.ImageSize == ImageSize.Small).Select(s => s.MediaUrl).Distinct().ToList();
            product.MediumImageUrls = entity.ProductMetadatas.Where(x => x.ImageSize == ImageSize.Medium).Select(s => s.MediaUrl).Distinct().ToList();
            product.LargeImageUrls = entity.ProductMetadatas.Where(x => x.ImageSize == ImageSize.Large).Select(s => s.MediaUrl).Distinct().ToList();

            return product;
        }
    }
}
