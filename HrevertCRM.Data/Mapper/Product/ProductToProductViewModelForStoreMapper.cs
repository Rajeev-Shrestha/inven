using System;
using System.Linq;
using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper.Product
{
    public class ProductToProductViewModelForStoreMapper : MapperBase<Entities.Product, ProductViewModelForStore>
    {
        public override Entities.Product Map(ProductViewModelForStore viewModel)
        {
            throw new NotImplementedException();
        }

        public override ProductViewModelForStore Map(Entities.Product entity)
        {
            if (entity == null) return null;
            var product = new ProductViewModelForStore
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

            if (entity.ProductsReferencedByKitAndAssembledTypes != null &&
                entity.ProductsReferencedByKitAndAssembledTypes.Count > 0)
                product.ProductsReferencedByKitAndAssembledType = entity.ProductsReferencedByKitAndAssembledTypes
                    .Select(x => x.ProductRefId).ToList();

            if (entity.ProductMetadatas == null || entity.ProductMetadatas.Count <= 0) return product;
            foreach (var entityProductMetadata in entity.ProductMetadatas)
            {
                var imageUrl = new ImageUrlBySizeViewModel
                {
                    //SmallImageUrl = entityProductMetadata.
                };
            }

            return product;
        }
    }
}
